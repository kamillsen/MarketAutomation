using System.Collections.Generic;
using BarcodeMarketApp.Models;
using Npgsql;

namespace BarcodeMarketApp.Database
{
    public class DatabaseService
    {
        // Veritabanı bağlantı nesnesi
        private readonly NpgsqlConnection connection;

        // Constructor: bağlantıyı başlatır
        public DatabaseService()
        {
            string connString = "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=barcode_db";
            connection = new NpgsqlConnection(connString);
            connection.Open();
        }

        // ✅ Barkoda göre ürün getir
        public Product GetProductByBarcode(string barcode)
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM product WHERE barcode = @b", connection);
            cmd.Parameters.AddWithValue("@b", barcode);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Product
                {
                    Id = reader.GetInt32(0),
                    Barcode = reader.GetString(1),
                    Name = reader.GetString(2),
                    Price = reader.GetDecimal(3),
                    Stock = reader.GetInt32(4)
                };
            }

            return null;
        }

        // ✅ Tüm ürünleri getir
        public List<Product> GetAllProducts()
        {
            var products = new List<Product>();
            using var cmd = new NpgsqlCommand("SELECT id, barcode, name, price, stock FROM product", connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                products.Add(new Product
                {
                    Id = reader.GetInt32(0),
                    Barcode = reader.GetString(1),
                    Name = reader.GetString(2),
                    Price = reader.GetDecimal(3),
                    Stock = reader.GetInt32(4)
                });
            }
            return products;
        }

        // ✅ Stok azaltma
        public bool ReduceStock(string barcode, int quantity)
        {
            using var cmd = new NpgsqlCommand(
                "UPDATE product SET stock = stock - @q WHERE barcode = @b AND stock >= @q", connection);
            cmd.Parameters.AddWithValue("@b", barcode);
            cmd.Parameters.AddWithValue("@q", quantity);

            return cmd.ExecuteNonQuery() > 0;
        }

        // ✅ Yeni ürün ekleme
        public void InsertProduct(Product product)
        {
            using var cmd = new NpgsqlCommand(
                "INSERT INTO product (barcode, name, price, stock) VALUES (@b, @n, @p, @s)", connection);
            cmd.Parameters.AddWithValue("@b", product.Barcode);
            cmd.Parameters.AddWithValue("@n", product.Name);
            cmd.Parameters.AddWithValue("@p", product.Price);
            cmd.Parameters.AddWithValue("@s", product.Stock);

            cmd.ExecuteNonQuery();
        }

        // ✅ YENİ: sales tablosuna satış kaydı (sadece toplam tutar)
        public int InsertSale(decimal totalPrice)
        {
            using var cmd = new NpgsqlCommand(
                "INSERT INTO sales (total_price) VALUES (@total) RETURNING id", connection);
            cmd.Parameters.AddWithValue("@total", totalPrice);
            return (int)cmd.ExecuteScalar();
        }

        // ✅ YENİ: sale_items tablosuna ürün detayları kaydı
        public void InsertSaleItem(int saleId, string barcode, int quantity, decimal price)
        {
            using var cmd = new NpgsqlCommand(@"
                INSERT INTO sale_items (sale_id, barcode, quantity, price)
                VALUES (@s, @b, @q, @p)", connection);

            cmd.Parameters.AddWithValue("@s", saleId);
            cmd.Parameters.AddWithValue("@b", barcode);
            cmd.Parameters.AddWithValue("@q", quantity);
            cmd.Parameters.AddWithValue("@p", price);
            cmd.ExecuteNonQuery();
        }
    }
}

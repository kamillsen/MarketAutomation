using System.Collections.Generic;
using BarcodeMarketApp.Models;
using Npgsql;

namespace BarcodeMarketApp.Database
{
    public class DatabaseService
    {
        // Veritabanı bağlantı nesnesi
        private readonly NpgsqlConnection connection;

        // Yapıcı metot (constructor): bağlantıyı başlatır
        public DatabaseService()
        {
            // PostgreSQL bağlantı dizesi — kendi bilgilerine göre güncelle!
            string connString = "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=barcode_db";
            connection = new NpgsqlConnection(connString);
            connection.Open();
        }

        // Barkoda göre ürün getirme
        public Product GetProductByBarcode(string barcode)
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM product WHERE barcode = @b", connection);
            cmd.Parameters.AddWithValue("@b", barcode);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Product
                {
                    Id = reader.GetInt32(0),         // id INTEGER
                    Barcode = reader.GetString(1),   // barcode TEXT
                    Name = reader.GetString(2),      // name TEXT
                    Price = reader.GetDecimal(3),    // price NUMERIC
                    Stock = reader.GetInt32(4)       // stock INTEGER
                };
            }

            return null;
        }



        // Tüm ürünleri listele
        public List<Product> GetAllProducts()
        {
            var products = new List<Product>();

            using var cmd = new NpgsqlCommand("SELECT id, barcode, name, price, stock FROM product", connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                products.Add(new Product
                {
                    Id = reader.GetInt32(0),          // ✅ id INTEGER
                    Barcode = reader.GetString(1),    // ✅ barcode TEXT
                    Name = reader.GetString(2),       // ✅ name TEXT
                    Price = reader.GetDecimal(3),     // ✅ price NUMERIC
                    Stock = reader.GetInt32(4)        // ✅ stock INTEGER
                });
            }

            return products;
        }


        // Stoktan düşme işlemi
        public bool ReduceStock(string barcode, int quantity)
        {
            using var cmd = new NpgsqlCommand("UPDATE product SET stock = stock - @q WHERE barcode = @b AND stock >= @q", connection);
            cmd.Parameters.AddWithValue("@b", barcode);
            cmd.Parameters.AddWithValue("@q", quantity);

            return cmd.ExecuteNonQuery() > 0;
        }

        // Satış kaydı oluştur
        public void InsertSale(string barcode, int quantity, decimal totalPrice)
        {
            using var cmd = new NpgsqlCommand("INSERT INTO sales (barcode, quantity, total_price) VALUES (@b, @q, @t)", connection);
            cmd.Parameters.AddWithValue("@b", barcode);
            cmd.Parameters.AddWithValue("@q", quantity);
            cmd.Parameters.AddWithValue("@t", totalPrice);

            cmd.ExecuteNonQuery();
        }

        public void InsertProduct(Product product)
        {
            using var cmd = new NpgsqlCommand("INSERT INTO product (barcode, name, price, stock) VALUES (@b, @n, @p, @s)", connection);
            cmd.Parameters.AddWithValue("@b", product.Barcode);
            cmd.Parameters.AddWithValue("@n", product.Name);
            cmd.Parameters.AddWithValue("@p", product.Price);
            cmd.Parameters.AddWithValue("@s", product.Stock);

            cmd.ExecuteNonQuery();
        }

    }
}

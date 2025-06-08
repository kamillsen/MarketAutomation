using System;
using System.Windows.Forms;
using BarcodeMarketApp.Models;
using BarcodeMarketApp.Database;
using BarcodeMarketApp.Services;

namespace BarcodeMarketApp.Forms
{
    public partial class MainForm : Form
    {
        // Veritabanı servis sınıfı
        private readonly DatabaseService dbService = new();

        // Sepet (geçici alışveriş listesi) yönetim sınıfı
        private readonly CartService cartService = new();

        public MainForm()
        {
            InitializeComponent();
        }

        // Kullanıcı barkod girdiğinde Enter tuşuyla ürün getirilir
        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtBarcode.Text))
            {
                string barcode = txtBarcode.Text.Trim();

                var product = dbService.GetProductByBarcode(barcode);

                if (product != null)
                {
                    cartService.AddProduct(product);
                    UpdateCartView(); // sepeti yenile
                }
                else
                {
                    MessageBox.Show("Bu barkoda ait ürün bulunamadı!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                txtBarcode.Clear(); // kutuyu temizle
            }
        }

        // Sepetteki ürünleri ListView'de göster
        private void UpdateCartView()
        {
            lvCart.Items.Clear();

            foreach (var item in cartService.Items)
            {
                var row = new ListViewItem(item.Product.Name); // Ürün Adı
                row.SubItems.Add(item.Quantity.ToString()); // Adet
                row.SubItems.Add(item.Product.Price.ToString("C2")); // Birim Fiyat
                row.SubItems.Add(item.TotalPrice.ToString("C2")); // Toplam

                lvCart.Items.Add(row);
            }

            lblTotal.Text = $"Toplam: {cartService.GetTotal():C2}"; // Toplam güncelle
        }

        // "Tüm Ürünleri Göster" butonu: veritabanındaki tüm ürünleri getir
        private void btnShowProducts_Click(object sender, EventArgs e)
        {
            lvCart.Items.Clear();

            var products = dbService.GetAllProducts();

            if (products.Count == 0)
            {
                MessageBox.Show("Veritabanında hiç ürün bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (var product in products)
            {
                var row = new ListViewItem(product.Name);
                row.SubItems.Add("-"); // Adet bilinmiyor
                row.SubItems.Add(product.Price.ToString("C2"));
                row.SubItems.Add("Stok: " + product.Stock.ToString());
                lvCart.Items.Add(row);
            }

            lblTotal.Text = "Toplam: ₺0,00 (sadece ürün listesi)";
        }

        // ListView üzerinde çift tıklayınca: adet güncelle
        private void lvCart_DoubleClick(object sender, EventArgs e)
        {
            if (lvCart.SelectedItems.Count == 0) return;

            var selectedItem = lvCart.SelectedItems[0];
            string productName = selectedItem.Text;

            string input = Microsoft.VisualBasic.Interaction.InputBox(
                $"'{productName}' ürününün yeni adetini girin:", "Adet Güncelle", selectedItem.SubItems[1].Text);

            if (int.TryParse(input, out int newQty) && newQty > 0)
            {
                cartService.UpdateQuantity(productName, newQty);
                UpdateCartView();
            }
            else
            {
                MessageBox.Show("Geçerli bir sayı girilmelidir.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Satışı tamamla butonuna tıklanınca: stok düş, satış kaydet, sepeti temizle
        private void btnCompleteSale_Click(object sender, EventArgs e)
        {
            // 1. Stok kontrolü
            foreach (var item in cartService.Items)
            {
                bool success = dbService.ReduceStock(item.Product.Barcode, item.Quantity);
                if (!success)
                {
                    MessageBox.Show($"'{item.Product.Name}' için yeterli stok yok!", "Stok Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // 2. Toplam fiyat
            decimal total = cartService.GetTotal();

            // 3. Yeni satış kaydı oluştur
            int saleId = dbService.InsertSale(total);

            // 4. Her ürünü sale_items tablosuna yaz
            foreach (var item in cartService.Items)
            {
                dbService.InsertSaleItem(saleId, item.Product.Barcode, item.Quantity, item.Product.Price);
            }

            // 5. Temizle ve bilgilendir
            cartService.ClearCart();
            UpdateCartView();

            MessageBox.Show("Satış başarıyla tamamlandı!", "Satış Tamam", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }



    }
}

using System;
using System.Windows.Forms;
using BarcodeMarketApp.Database;
using BarcodeMarketApp.Forms;
using BarcodeMarketApp.Models;
using Npgsql; // MainForm burada yer alacak (senin form adýn buysa)

namespace BarcodeMarketApp
{
    internal static class Program
    {
        /// <summary>
        /// Uygulamanýn ana giriþ noktasýdýr.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // ! Veritabaný baþlatýlýyor ve gerekli tablolar oluþturuluyor
            var dbService = new DatabaseService();
            // Test ürünü ekle (eðer o barkod yoksa)
            var testProduct = new Product
            {
                Barcode = "1234567890123",
                Name = "Deneme Ürünü",
                Price = 99.99m,
                Stock = 10
            };

            // Ürün zaten var mý kontrol etmeden ekleyelim (istenirse kontrol de eklenir)
            try
            {
                dbService.InsertProduct(testProduct);
            }
            catch (PostgresException ex) when (ex.SqlState == "23505") // unique_violation
            {
                // Ayný barkodlu ürün varsa, hata verme
            }


            // Uygulama varsayýlan ayarlarla baþlatýlýr
            ApplicationConfiguration.Initialize();

            // Ana form baþlatýlýr (MainForm senin formunun adýdýr)
            Application.Run(new MainForm());
        }
    }
}

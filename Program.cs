using System;
using System.Windows.Forms;
using BarcodeMarketApp.Database;
using BarcodeMarketApp.Forms;
using BarcodeMarketApp.Models;
using Npgsql; // MainForm burada yer alacak (senin form ad�n buysa)

namespace BarcodeMarketApp
{
    internal static class Program
    {
        /// <summary>
        /// Uygulaman�n ana giri� noktas�d�r.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // ! Veritaban� ba�lat�l�yor ve gerekli tablolar olu�turuluyor
            var dbService = new DatabaseService();
            // Test �r�n� ekle (e�er o barkod yoksa)
            var testProduct = new Product
            {
                Barcode = "1234567890123",
                Name = "Deneme �r�n�",
                Price = 99.99m,
                Stock = 10
            };

            // �r�n zaten var m� kontrol etmeden ekleyelim (istenirse kontrol de eklenir)
            try
            {
                dbService.InsertProduct(testProduct);
            }
            catch (PostgresException ex) when (ex.SqlState == "23505") // unique_violation
            {
                // Ayn� barkodlu �r�n varsa, hata verme
            }


            // Uygulama varsay�lan ayarlarla ba�lat�l�r
            ApplicationConfiguration.Initialize();

            // Ana form ba�lat�l�r (MainForm senin formunun ad�d�r)
            Application.Run(new MainForm());
        }
    }
}

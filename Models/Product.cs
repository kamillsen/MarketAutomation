using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcodeMarketApp.Models
{
    public class Product
    {
        public int Id { get; set; }               // Otomatik artan ID
        public string Barcode { get; set; }       // Barkod numarası (benzersiz)
        public string Name { get; set; }          // Ürün adı
        public decimal Price { get; set; }        // Fiyat
        public int Stock { get; set; }            // Stok miktarı
    }
}

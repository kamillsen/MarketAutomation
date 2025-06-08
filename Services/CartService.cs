using System.Collections.Generic;
using System.Linq;
using BarcodeMarketApp.Models;

namespace BarcodeMarketApp.Services
{
    public class CartService
    {
        public List<CartItem> Items { get; } = new();

        public void AddProduct(Product product)
        {
            var existingItem = Items.FirstOrDefault(i => i.Product.Barcode == product.Barcode);

            if (existingItem != null)
                existingItem.Quantity++;
            else
                Items.Add(new CartItem { Product = product, Quantity = 1 });
        }

        public void ClearCart() => Items.Clear();

        public decimal GetTotal() => Items.Sum(i => i.TotalPrice);


        public void UpdateQuantity(string productName, int newQuantity)
        {
            var item = Items.FirstOrDefault(i => i.Product.Name == productName);
            if (item != null)
            {
                item.Quantity = newQuantity;
                // TotalPrice zaten otomatik hesaplanıyor, elle set etmene gerek yok!
            }
        }

    }
}

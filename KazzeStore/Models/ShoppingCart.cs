using System.Collections.Generic;
using System.Linq;

namespace KazzeStore.Models
{
    public class ShoppingCart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public decimal Total => Items.Sum(i => i.Precio * i.Cantidad);

        public void AddItem(CartItem item)
        {
            var existing = Items.FirstOrDefault(i => i.ProductId == item.ProductId && i.Talla == item.Talla);
            if (existing != null)
            {
                existing.Cantidad += item.Cantidad;
            }
            else
            {
                Items.Add(item);
            }
        }

        public void RemoveItem(int productId)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                Items.Remove(item);
            }
        }

        public void Clear() => Items.Clear();
    }
}
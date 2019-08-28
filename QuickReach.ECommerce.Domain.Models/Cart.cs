using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickReach.ECommerce.Domain.Models
{
    public class Cart : EntityBase
    {
        public int CustomerId { get; set; }
        public List<CartItem> Items { get; set; }

        public Cart (int customerId)
        {
            CustomerId = customerId;
            Items = new List<CartItem>();
        }

        public void AddCartItem(CartItem cartItem)
        {
            ((ICollection<CartItem>)this.Items).Add(cartItem);
        }



        public void AddCartItem(Product product, CartItem cartItem)
        {
            cartItem.ProductId = product.ID.ToString();
            cartItem.ProductName = product.Name;

            ((ICollection<CartItem>)this.Items).Add(cartItem);
        }
    }
}


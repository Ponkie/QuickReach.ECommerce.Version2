using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickReach.ECommerce.API.ViewModel
{
    public class CustomerBasket
    {
        public int CustomerId { get; set; }
        public List<CartItem> Items { get; set; }

        public CustomerCart(int customerId)
        {
            CustomerId = customerId;
            Items = new List<CartItem>();
        }
    }
}


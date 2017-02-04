using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceBasket.Business.Models
{
    public class BasketItem
    {
        private readonly string name ;
        public BasketItem(string name)
        {
            this.name = name;
        }
        public string Name => name;
        public decimal Price { get; }
    }
}

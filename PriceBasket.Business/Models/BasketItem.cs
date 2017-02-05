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
        private readonly int count;
        public BasketItem(string name, int count)
        {
            this.name = name;
            if( count <=0) throw new ArgumentException(string.Format("BasketItem {0} cannot be initialized with {1}",name, count));
            this.count = count;
        }
        public string Name => name;
        public decimal? Value { get; set; }
        public decimal? Discount { get; set; }

        public int Count => count;
    }
}

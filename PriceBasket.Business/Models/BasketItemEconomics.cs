using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceBasket.Business.Models
{
    public class BasketItemEconomics : IEqualityComparer<BasketItemEconomics>
    {
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }

        public bool Equals(BasketItemEconomics x, BasketItemEconomics y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(BasketItemEconomics obj)
        {
            BasketItemEconomics bie = (BasketItemEconomics)obj;
            return bie.Name.GetHashCode();
        }
    }
}

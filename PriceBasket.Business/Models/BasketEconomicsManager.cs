using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceBasket.Business.Models
{
    public class BasketEconomicsManager
    {
        private readonly ConcurrentDictionary<string, BasketItemEconomics> basketItemEconomicses;

        public BasketEconomicsManager()
        {
            basketItemEconomicses = new ConcurrentDictionary<string, BasketItemEconomics>();
        }
        public bool TryAddItem(string itemName, BasketItemEconomics item)
        {
            return true;
        }
    }
}

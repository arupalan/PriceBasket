using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace PriceBasket.Business.Models
{
    public class BasketEconomicsManager : IBasketEconomicsManager
    {
        private readonly ConcurrentDictionary<string, BasketItemEconomics> basketItemEconomicses;
        private readonly ILog logger;

        public BasketEconomicsManager(ILog logger)
        {
            this.logger = logger;
            logger.Info("BasketEconomicsManager Constructor Initialized");
            basketItemEconomicses = new ConcurrentDictionary<string, BasketItemEconomics>();
        }

        public bool TryAddEconomics(string itemName, BasketItemEconomics item) => basketItemEconomicses.TryAdd(itemName, item);

        public BasketItemEconomics AddOrUpdateEconomics(string itemName, Func<string, BasketItemEconomics> addValueFactory,
            Func<string, BasketItemEconomics, BasketItemEconomics> updateValueFactory)
            => basketItemEconomicses.AddOrUpdate(itemName, addValueFactory, updateValueFactory);

        public int Count => basketItemEconomicses.Count;

        public bool TryGetEconomics(string itemName, out BasketItemEconomics itemEconomics)
            => basketItemEconomicses.TryGetValue(itemName, out itemEconomics);
    }
}

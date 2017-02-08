using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using log4net;
using PriceBasket.Business.Models;
using System.Threading.Tasks;

namespace PriceBasket.Business.Economics
{
    /// <summary>
    /// Encapsulates the active list of BasketItemEconomics. 
    /// Internally the active list of economics are maintained inside a ConcurrentDictionary.
    /// This also allows to reset (add,update) the internal BasketItemEconomics
    /// </summary>
    public class BasketEconomicsManager : IBasketEconomicsManager
    {
        private readonly ConcurrentDictionary<string, BasketItemEconomics> basketItemEconomicses;
        private readonly ILog logger;

        public BasketEconomicsManager(ILog logger)
        {
            this.logger = logger;
            logger.Debug("BasketEconomicsManager Constructor Initialized");
            basketItemEconomicses = new ConcurrentDictionary<string, BasketItemEconomics>();
        }

        /// <summary>
        /// Allows reset (add,update) the internal BasketItemEconomics
        /// </summary>
        /// <param name="economicItems"></param>
        /// <returns></returns>
        public async Task ResetItemEconomicsAsync(List<BasketItemEconomics> economicItems)
        {
            foreach (var item in economicItems)
            {
                AddOrUpdateEconomics(item.Name, key => item, (key, oldValue) => item);
            }
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using PriceBasket.Business.Models;


namespace PriceBasket.Business.Pricing
{
    public class BasketPricer : IBasketPricer
    {

        public readonly IBasketEconomicsManager basketEconomicsManager;
        private readonly ILog logger;

        public BasketPricer(IBasketEconomicsManager basketEconomicsManager, ILog logger)
        {
            this.logger = logger;
            logger.Info("BasketPricer Constructor Initialized");
            this.basketEconomicsManager = basketEconomicsManager;
        }

        public async Task<List<BasketItem>> PriceAsync(List<BasketItem> basketItems)
        {
            var result = new List<BasketItem>();
            foreach (var item in basketItems)
            {
                BasketItemEconomics basketItemEconomics;
                if (basketEconomicsManager.TryGetEconomics(item.Name, out basketItemEconomics))
                {
                    item.Value = basketItemEconomics.Price*item.Count*(1 - basketItemEconomics.Discount);
                    result.Add(item);
                }
                else
                {
                    throw new Exception(string.Format("No Economics setup for Item {0}. Please configure economics",item.Name));
                }
            }
            return await Task.FromResult(result);
        }
    }
}

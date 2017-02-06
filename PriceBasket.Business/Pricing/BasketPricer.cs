using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using PriceBasket.Business.Economics;
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

        public async Task<List<BasketResultItem>> PriceAsync(List<BasketRequestItem> basketItems)
        {
            var result = new List<BasketResultItem>();
            foreach (var item in basketItems)
            {
                BasketItemEconomics basketItemEconomics;
                if (basketEconomicsManager.TryGetEconomics(item.Name, out basketItemEconomics))
                {
                    var resultItem = new BasketResultItem(item)
                    {
                        Value = basketItemEconomics.Price*item.Unit,
                        Discount = basketItemEconomics.Discount
                    };
                    result.Add(resultItem);
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

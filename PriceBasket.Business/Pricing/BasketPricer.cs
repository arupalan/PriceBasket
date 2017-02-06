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

        public async Task<Basket> PriceAsync(List<BasketRequestItem> basketItems)
        {
            var result = new List<BasketResultItem>();
            decimal? subTotal = 0.0M;
            foreach (var item in basketItems)
            {
                BasketItemEconomics basketItemEconomics;
                if (basketEconomicsManager.TryGetEconomics(item.Name, out basketItemEconomics))
                {
                    var resultItem = new BasketResultItem(item)
                    {
                        Value = basketItemEconomics.Price*item.Unit,
                        Discount = basketItemEconomics.Discount,
                        DiscountPence = Decimal.ToInt32((basketItemEconomics.Discount??0.0M) * (basketItemEconomics.Price??0.0M) * item.Unit * 100)
                    };
                    subTotal = decimal.Add(subTotal.Value, resultItem.Value??0.0M);
                    result.Add(resultItem);
                }
                else
                {
                    throw new Exception(string.Format("No Economics setup for Item {0}. Please configure economics",item.Name));
                }
            }
            var total = subTotal;
            foreach (var basketResultItem in result.Where(basketResultItem => basketResultItem.Discount.HasValue && basketResultItem.Value.HasValue && basketResultItem.Discount.Value * basketResultItem.Value.Value > 0.00009M))
            {
                total = Decimal.Subtract(total.Value,
                    (basketResultItem.Discount??0.0M) * (basketResultItem.Value??0.0M));
            }
            return await Task.FromResult(new Basket(result,subTotal,total));
        }
    }
}

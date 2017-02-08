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
    /// <summary>
    /// Encapsulates the Pricing Logic for of a list of requested Basket Items.
    /// The List of Items are enumerated for Active MultipackDiscount. If one is active 
    /// then the discount is set to the MultiPackDiscount else defaults to normal discount
    /// </summary>
    public class BasketPricer : IBasketPricer
    {

        public readonly IBasketEconomicsManager basketEconomicsManager;
        private readonly ILog logger;

        public BasketPricer(IBasketEconomicsManager basketEconomicsManager, ILog logger)
        {
            this.logger = logger;
            logger.Debug("BasketPricer Constructor Initialized");
            this.basketEconomicsManager = basketEconomicsManager;
        }

        /// <summary>
        /// Pricing Logic for BasketItems. The List of Items are enumerated for Active MultipackDiscount. If one is active 
        /// then the discount is set to the MultiPackDiscount else defaults to normal discount.  
        /// </summary>
        /// <param name="basketItems"></param>
        /// <returns>Basket with computed SubTotal , Discount and Total </returns>
        public async Task<Basket> PriceAsync(List<BasketRequestItem> basketItems)
        {
            var result = new List<BasketResultItem>();
            decimal? subTotal = 0.0M;
            foreach (var item in basketItems)
            {
                BasketItemEconomics basketItemEconomics;
                if (basketEconomicsManager.TryGetEconomics(item.Name, out basketItemEconomics))
                {
                    var discount = (basketItemEconomics.MultiPackDiscount != null) &&
                                   IsValidMultiPack(basketItemEconomics.MultiPackDiscount, basketItems)
                        ? basketItemEconomics.MultiPackDiscount.Discount
                        : basketItemEconomics.Discount;
                    var resultItem = new BasketResultItem(item)
                    {
                        Value = basketItemEconomics.Price*item.Unit,
                        Discount = discount,
                        DiscountPence = Decimal.ToInt32((discount ?? 0.0M)*(basketItemEconomics.Price ?? 0.0M)* item.Unit*100)
                    };
                    subTotal = decimal.Add(subTotal.Value, resultItem.Value ?? 0.0M);
                    result.Add(resultItem);
                }
                else
                    throw new Exception($"No Economics setup for Item {item.Name}. Please configure economics");
            }
            var total = subTotal;
            foreach (var basketResultItem in result.Where(basketResultItem => basketResultItem.Discount.HasValue && basketResultItem.Value.HasValue && basketResultItem.Discount.Value * basketResultItem.Value.Value > 0.00009M))
            {
                total = Decimal.Subtract(total.Value, (basketResultItem.Discount??0.0M) * (basketResultItem.Value??0.0M));
            }
            return await Task.FromResult(new Basket(result,subTotal,total));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="multipackDiscount"></param>
        /// <param name="basketItems"></param>
        /// <returns>true if the basket has a multipack item and the number of units else false</returns>
        private bool IsValidMultiPack(MultiDiscount multipackDiscount, List<BasketRequestItem> basketItems)
        {
            return
                basketItems.Any(item => item.Name == multipackDiscount.ItemName && item.Unit >= multipackDiscount.ItemUnit);
        }
    }
}

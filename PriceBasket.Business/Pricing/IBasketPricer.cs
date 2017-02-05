using System.Collections.Generic;
using System.Threading.Tasks;
using PriceBasket.Business.Models;

namespace PriceBasket.Business.Pricing
{
    public interface IBasketPricer
    {
        Task<List<BasketResultItem>> PriceAsync(List<BasketRequestItem> basketItems);
    }
}
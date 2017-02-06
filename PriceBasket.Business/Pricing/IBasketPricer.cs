using System.Collections.Generic;
using System.Threading.Tasks;
using PriceBasket.Business.Models;

namespace PriceBasket.Business.Pricing
{
    public interface IBasketPricer
    {
        Task<Basket> PriceAsync(List<BasketRequestItem> basketItems);
    }
}
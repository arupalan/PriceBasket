using System;

namespace PriceBasket.Business.Models
{
    public interface IBasketEconomicsManager
    {
        bool TryAddEconomics(string itemName, BasketItemEconomics item);

        BasketItemEconomics AddOrUpdateEconomics(string itemName, Func<string, BasketItemEconomics> addValueFactory,
            Func<string, BasketItemEconomics, BasketItemEconomics> updateValueFactory);
        int Count { get; }
    }
}
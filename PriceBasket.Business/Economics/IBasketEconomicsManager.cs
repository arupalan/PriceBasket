using System;
using PriceBasket.Business.Models;

namespace PriceBasket.Business.Economics
{
    public interface IBasketEconomicsManager
    {
        bool TryAddEconomics(string itemName, BasketItemEconomics item);

        BasketItemEconomics AddOrUpdateEconomics(string itemName, Func<string, BasketItemEconomics> addValueFactory,
            Func<string, BasketItemEconomics, BasketItemEconomics> updateValueFactory);
        int Count { get; }
        bool TryGetEconomics(string itemName, out BasketItemEconomics itemEconomics);
    }
}
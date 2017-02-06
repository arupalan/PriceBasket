using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PriceBasket.Business.Models;

namespace PriceBasket.Business.Economics
{
    public interface IBasketEconomicsManager
    {
        Task ResetItemEconomicsAsync(List<BasketItemEconomics> economicItems);
        bool TryAddEconomics(string itemName, BasketItemEconomics item);

        BasketItemEconomics AddOrUpdateEconomics(string itemName, Func<string, BasketItemEconomics> addValueFactory,
            Func<string, BasketItemEconomics, BasketItemEconomics> updateValueFactory);
        int Count { get; }
        bool TryGetEconomics(string itemName, out BasketItemEconomics itemEconomics);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PriceBasket.Business.Models;

namespace PriceBasket.Business.Pricing
{
    public class BasketPricer
    {

        public readonly IBasketEconomicsManager BasketEconomicsManager;

        public BasketPricer(IBasketEconomicsManager basketEconomicsManager)
        {
            this.BasketEconomicsManager = basketEconomicsManager;
        }
    }
}

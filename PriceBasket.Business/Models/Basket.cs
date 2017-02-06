using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceBasket.Business.Models
{
    public class Basket
    {
        private readonly List<BasketResultItem> basketItems;
        private readonly decimal? subTotal;
        private readonly decimal? total;

        public Basket(List<BasketResultItem> basketItems , decimal? subTotal, decimal? total )
        {
            this.basketItems = basketItems;
            this.total = total;
            this.subTotal = subTotal;
        }

        public decimal? SubTotal => subTotal;
        public decimal? Total => total;

        public List<BasketResultItem> BasketItems => basketItems;

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceBasket.Business.Models
{
    /// <summary>
    /// Encapsulates the list of items in the basket. 
    /// The basket SubTotal, and the Total and the constituent items. 
    /// Note the properties are readonly so that once a basket is created the 
    /// computed prices cannot be changed which is specific to the constituents of the basket. 
    /// Note that the basket is immutable
    /// </summary>
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

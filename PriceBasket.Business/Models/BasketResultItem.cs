using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PriceBasket.Business.Models
{
    public class BasketResultItem 
    {
        private readonly BasketRequestItem requestItem;
        public BasketResultItem(BasketRequestItem requestItem)
        {
            this.requestItem = requestItem;
        }
        public decimal? Value { get; set; }
        public decimal? Discount { get; set; }
    }
}

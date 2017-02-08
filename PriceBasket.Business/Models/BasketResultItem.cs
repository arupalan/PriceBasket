using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PriceBasket.Business.Models
{
    /// <summary>
    /// Encapsulates a priced BasketRequestItem which is requested to be added to the basket. 
    /// For simplicity of json; it is as it is but will be advantageous to have the Name and Unit as readonly. 
    /// You can leverage json schema validation and a factory to provide this immutability.
    /// </summary>
    public class BasketResultItem 
    {
        private readonly BasketRequestItem requestItem;
        public BasketResultItem(BasketRequestItem requestItem)
        {
            this.requestItem = requestItem;
        }
        public decimal? Value { get; set; }
        public decimal? Discount { get; set; }
        public Int32? DiscountPence { get; set; }
        public BasketRequestItem RequestItem => requestItem;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceBasket.Business.Models
{
    /// <summary>
    /// Encapsulates Discount which is active only at basket level.
    /// ItemName is the name of another item whose number of unit has to
    /// match to be equal or greater than at the basket level for 
    /// this Discount to be applied
    /// </summary>
    public class MultiDiscount
    {
        public string ItemName { get; set; }
        public int ItemUnit { get; set; }
        public decimal? Discount { get; set; }
    }
    /// <summary>
    /// Encapsulates a product economics like Price , Discount and MultiPackDiscount. Note the IEqualityComparer uses
    /// the Name field. In production implementation it will be useful to have
    /// additional identified like barcode sku etc.
    /// For simplicity of json; it is as it is but will be advantageous to have the economics as readonly.
    /// You can leverage json schema validation and a factory to provide immutability.  
    /// </summary>
    public class BasketItemEconomics : IEqualityComparer<BasketItemEconomics>
    {
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }
        public MultiDiscount MultiPackDiscount { get; set; }

        public bool Equals(BasketItemEconomics x, BasketItemEconomics y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(BasketItemEconomics obj)
        {
            BasketItemEconomics bie = (BasketItemEconomics)obj;
            return bie.Name.GetHashCode();
        }
    }
}

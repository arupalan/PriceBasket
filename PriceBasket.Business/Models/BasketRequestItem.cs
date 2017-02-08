namespace PriceBasket.Business.Models
{
    /// <summary>
    /// Encapsulates a BasketRequestItem which is requested to be added to the basket
    /// For simplicity of json; it is as it is but will be advantageous to have the Name and Unit as readonly.
    /// You can leverage json schema validation and a factory to provide this immutability.  
    /// </summary>
    public class BasketRequestItem
    {
        public string Name { get; set; }
        public int Unit { get; set; }
    }
}
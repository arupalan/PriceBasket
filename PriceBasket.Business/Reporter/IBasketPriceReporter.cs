using System.Collections.Generic;
using System.Threading.Tasks;
using PriceBasket.Business.Models;

namespace PriceBasket.Business.Reporter
{
    public interface IBasketPriceReporter
    {
        Task ReportAsync(Basket basket);
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using PriceBasket.Business.Models;
using Newtonsoft.Json;

namespace PriceBasket.Business.Reporter
{
    public class BasketPriceReporter : IBasketPriceReporter
    {
        private readonly ILog logger;
        public BasketPriceReporter(ILog logger)
        {
            this.logger = logger;
        }
        public async Task ReportAsync(List<BasketResultItem> result)
        {
            logger.Debug(string.Format("Generating report for result {0}", JsonConvert.SerializeObject(result)));
            var subtotal = result.Aggregate( (accumulated, next) =>
            {
                var accumulator = new BasketResultItem(null) {Value = 0.0M};
                accumulator.Value =
                    (accumulated.Value.HasValue ? accumulated.Value.Value : 0.0M) +
                    (next.Value.HasValue ? next.Value.Value : 0.0M);
                return accumulator;
            });
            NumberFormatInfo nfi = new CultureInfo("en-GB", false).NumberFormat;
            nfi.PercentDecimalDigits = 0;
            Console.WriteLine("Subtotal: {0}", subtotal.Value.Value.ToString("C2", CultureInfo.CreateSpecificCulture("en-GB")));
            var total = subtotal.Value.Value;
            foreach (var basketResultItem in result.Where(basketResultItem => basketResultItem.Discount.HasValue && basketResultItem.Value.HasValue && basketResultItem.Discount.Value*basketResultItem.Value.Value > 0.00009M))
            {
                Console.WriteLine("{0} {1} off: -{2}P", basketResultItem.RequestItem.Name,
                    (basketResultItem.Discount.Value).ToString("P",nfi),
                    Decimal.ToInt32(basketResultItem.Discount.Value*basketResultItem.Value.Value*100));
                total = Decimal.Subtract(total,
                    basketResultItem.Discount.Value*basketResultItem.Value.Value);
            }
            if(Decimal.Equals(subtotal.Value.Value,total)) Console.WriteLine("(No offers available)");
            Console.WriteLine("Total: {0}", total.ToString("C2", CultureInfo.CreateSpecificCulture("en-GB")));
        } 
    }
}

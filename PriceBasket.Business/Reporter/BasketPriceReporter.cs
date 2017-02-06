﻿using System;
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
        public async Task ReportAsync(Basket basket)
        {
            logger.Debug(string.Format("Generating report for result {0}", JsonConvert.SerializeObject(basket.BasketItems)));
            var nfi = new CultureInfo("en-GB", false).NumberFormat;
            nfi.PercentDecimalDigits = 0;
            Console.WriteLine("Subtotal: {0}", (basket.SubTotal??0.0M).ToString("C2", CultureInfo.CreateSpecificCulture("en-GB")));
            foreach (var basketResultItem in basket.BasketItems.Where(basketResultItem => basketResultItem.DiscountPence > 0))
            {
                Console.WriteLine("{0} {1} off: -{2}P", basketResultItem.RequestItem.Name,
                    (basketResultItem.Discount ?? 0.0M).ToString("P", nfi), basketResultItem.DiscountPence );
            }
            if(Decimal.Equals(basket.SubTotal??0.0M,basket.Total??0.0M)) Console.WriteLine("(No offers available)");
            Console.WriteLine("Total: {0}", (basket.Total??0.0M).ToString("C2", CultureInfo.CreateSpecificCulture("en-GB")));
        } 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using PriceBasket.Business.Pricing;
using PriceBasket.Service.DependencyInjection;
using CommandLine;
using CommandLine.Text;
using Newtonsoft.Json;
using PriceBasket.Business.Economics;
using PriceBasket.Business.Models;
using PriceBasket.Business.Verbs;

namespace PriceBasket.Service
{

    class Program
    {
        static void Main(string[] args)
        {
            var logger = DependencyInjector.Resolve<ILog>();
            try
            {
                Initialize();
                var commandProcessor = DependencyInjector.Resolve<ICommandProcessor>();
                var process = commandProcessor.ProcessVerbs(args);

                while (process)
                {
                    process = commandProcessor.ProcessVerbs();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Fatal Exception!!",ex);
            }
        }

        private static void Initialize()
        {
            var logger = DependencyInjector.Resolve<ILog>();
            logger.Info("PriceBasket Service Starting...");
            logger.Info("Initializing Basket Economics Manager...");
            var basketEconomicsManager = DependencyInjector.Resolve<IBasketEconomicsManager>();
            var itemEconomics = new List<BasketItemEconomics>
            {
                new BasketItemEconomics {Name = "Apple", Discount = 0.1M, Price = 1.00M},
                new BasketItemEconomics {Name = "Bread", Discount = null, MultiPackDiscount = new MultiDiscount
                {
                    Discount = 0.50M,
                    ItemName = "Soup",
                    ItemUnit = 2
                }, Price = 0.80M},
                new BasketItemEconomics {Name = "Milk", Discount = null, Price = 1.30M},
                new BasketItemEconomics {Name = "Soup", Discount = null, Price = 0.65M}
            };
            Task.Run(async () =>
            {
                try
                {
                    await basketEconomicsManager.ResetItemEconomicsAsync(itemEconomics);
                }
                catch (AggregateException ex)
                {
                    logger.Error("Fatal Exception ResetItemEconomicsAsync", ex);
                }
                catch (Exception ex)
                {
                    logger.Error("Fatal Exception ResetItemEconomicsAsync", ex);
                }
            }).Wait();
            logger.Info(string.Format("Current Active Economics...{0}", JsonConvert.SerializeObject(itemEconomics)));
        }
    }
}

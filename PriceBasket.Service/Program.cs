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
            var appleItemEconomics = new BasketItemEconomics() {Name = "Apple", Discount = 0.1M, Price = 1.00M};
            var breadItemEconomics = new BasketItemEconomics() {Name = "Bread", Discount = null, Price = 0.80M};
            var milkItemEconomics = new BasketItemEconomics() {Name = "Milk", Discount = null, Price = 1.30M};
            var soupItemEconomics = new BasketItemEconomics() {Name = "Soup", Discount = null, Price = 0.65M};
            basketEconomicsManager.AddOrUpdateEconomics("Apple", key => appleItemEconomics, (key, oldValue) => appleItemEconomics);
            basketEconomicsManager.AddOrUpdateEconomics("Bread", key => breadItemEconomics, (key, oldValue) => breadItemEconomics);
            basketEconomicsManager.AddOrUpdateEconomics("Milk", key => milkItemEconomics, (key, oldValue) => milkItemEconomics);
            basketEconomicsManager.AddOrUpdateEconomics("Soup", key => soupItemEconomics, (key, oldValue) => soupItemEconomics);
            logger.Info("Current Active Economics...");
            logger.Info(JsonConvert.SerializeObject(new List<BasketItemEconomics> {appleItemEconomics,breadItemEconomics,milkItemEconomics,soupItemEconomics}));
        }
    }
}

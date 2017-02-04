using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using PriceBasket.Business.Models;
using PriceBasket.Service.DependencyInjection;

namespace PriceBasket.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = DependencyInjector.Resolve<ILog>();
            logger.Info("PriceBasket Service Started");
            var basketEconomicsManager = DependencyInjector.Resolve<IBasketEconomicsManager>();
            Console.WriteLine(basketEconomicsManager.Count);
            String command;
                Boolean quitNow = false;
                while (!quitNow)
                {
                    Console.Write(">");
                    command = Console.ReadLine();
                    Console.WriteLine(command);
                    switch (command)
                    {
                        case "help":
                            Console.WriteLine("This should be help.");
                            break;

                        case "/version":
                            Console.WriteLine("This should be version.");
                            break;

                        case "quit":
                            quitNow = true;
                            break;

                        default:
                            Console.WriteLine("Unknown Command " + command);
                            break;
                    }
                }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using PriceBasket.Business.Economics;
using PriceBasket.Business.Models;
using PriceBasket.Business.Pricing;
using PriceBasket.Business.Reporter;

namespace PriceBasket.Business.Verbs
{
    /// <summary>
    /// Encapsulates Command Verbs processing and converts valids verb json datastream into objects.
    /// Json is used for passing data as it provides immense felxibility and is an industry standard
    /// Most of the enterprise distributed products like elasticsearch , stackifyy and many others uses json as standard.
    /// Also most browsers can interpret json natively 
    /// </summary>
    public class CommandProcessor : ICommandProcessor
    {
        private readonly ILog logger;
        private readonly IBasketPricer pricer;
        private readonly IBasketPriceReporter reporter;
        private readonly IBasketEconomicsManager economicsManager;

        public CommandProcessor(ILog logger, IBasketPricer pricer,IBasketEconomicsManager economicsManager, IBasketPriceReporter reporter)
        {
            this.logger = logger;
            this.pricer = pricer;
            this.reporter = reporter;
            this.economicsManager = economicsManager;
        }

        public bool ProcessVerbs()
        {
            Console.Write("command>");
            var command = Console.ReadLine();
            var argReg = new Regex(@"[^""\\]*(?:\\.[^""\\]*)*");
            var cmds = new List<string>();
            foreach (var temp in argReg.Matches(command).Cast<object>().Select(enumer => (string)enumer.ToString()).Where(temp => temp != string.Empty))
            {
                if (temp.Contains("--"))
                {
                    cmds.AddRange(temp.Split(' ').Where( e => !e.Trim().Equals(string.Empty)).ToArray());
                    continue;
                }
                cmds.Add(temp);
            }
            return ProcessVerbs(cmds.ToArray());
        }

        public bool ProcessVerbs(string[] args)
        {
            string invokedVerb = "";
            object invokedVerbInstance = new object();

            var options = new Options();

            if (!CommandLine.Parser.Default.ParseArguments(args, options,
              (verb, subOptions) => {
                  // if parsing succeeds the verb name and correct instance
                  // will be passed to onVerbCommand delegate (string,object)
                  invokedVerb = verb;
                  invokedVerbInstance = subOptions;
                  logger.Debug("Commandline Parsing succeeded");
                  logger.Debug(invokedVerb);
                  logger.Debug(JsonConvert.SerializeObject(invokedVerbInstance));
              }))
            {
                logger.Error("Invalid command. Type a valid command or type quit to exit.");
                return true;
                //Environment.Exit(CommandLine.Parser.DefaultExitCodeFail);
            }

            switch (invokedVerb)
            {
                case "pricebasket":
                    var pricebasketSubOptions = (PriceBasketSubOptions)invokedVerbInstance;
                    logger.Debug(JsonConvert.SerializeObject(pricebasketSubOptions.Basket));
                    var requestItems = JsonConvert.DeserializeObject<List<BasketRequestItem>>(pricebasketSubOptions.Basket);
                    logger.Debug(JsonConvert.SerializeObject(requestItems));
                    Task.Run(async () =>
                    {
                        try
                        {
                            var result = await pricer.PriceAsync(requestItems);
                            await reporter.ReportAsync(result);
                        }
                        catch (Exception ex)
                        {
                            logger.Error("Fatal Exception",ex);
                        }

                    }).Wait();
                    return true;
                case "puteconomics":
                    var puteconomicsSubOptions = (PutEconomicsSubOptions)invokedVerbInstance;
                    logger.Debug(JsonConvert.SerializeObject(puteconomicsSubOptions.ItemEconomics));
                    var itemEconomics = JsonConvert.DeserializeObject<List<BasketItemEconomics>>(puteconomicsSubOptions.ItemEconomics);
                    logger.Debug(JsonConvert.SerializeObject(itemEconomics));
                    Task.Run(async () =>
                    {
                        try
                        {
                            await economicsManager.ResetItemEconomicsAsync(itemEconomics);
                            logger.Info($"Current Active Economics...{JsonConvert.SerializeObject(itemEconomics)}");
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
                    return true;
                case "quit":
                    return false;
            }
            return true;
        }
    }
}

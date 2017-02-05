using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using log4net;
using Newtonsoft.Json;
using PriceBasket.Business.Models;

namespace PriceBasket.Business.Verbs
{
    public class CommandProcessor : ICommandProcessor
    {
        private readonly ILog logger;
        public CommandProcessor(ILog logger)
        {
            this.logger = logger;
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
                    var commitSubOptions = (PriceBasketSubOptions)invokedVerbInstance;
                    logger.Debug(JsonConvert.SerializeObject(commitSubOptions.Basket));
                    var requestItems = JsonConvert.DeserializeObject<List<BasketRequestItem>>(commitSubOptions.Basket);
                    logger.Debug(JsonConvert.SerializeObject(requestItems));
                    return true;
                case "puteconomics":
                    logger.Info(string.Format("{0} command not available in current version. This is features in v2.0.0.0",invokedVerb));
                    return true;
                case "quit":
                    return false;
            }
            return true;
        }
    }
}

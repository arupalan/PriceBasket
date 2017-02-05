using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using PriceBasket.Business.Pricing;
using PriceBasket.Service.DependencyInjection;
using CommandLine;
using Newtonsoft.Json;

namespace PriceBasket.Service
{
    class CommitSubOptions
    {
        [Option('a', "all", HelpText = "Tell the command to automatically stage files.")]
        public bool All { get; set; }
        public bool Patch { get; set; }
        // Remainder omitted
    }

    class PushSubOptions
    {
        // Remainder omitted
    }

    class TagSubOptions
    {
        // Remainder omitted 
    }

    class Options
    {
        public Options()
        {
            // Since we create this instance the parser will not overwrite it
            CommitVerb = new CommitSubOptions { };
        }

        [VerbOption("commit", HelpText = "Record changes to the repository.")]
        public CommitSubOptions CommitVerb { get; set; }

        [VerbOption("push", HelpText = "Update remote refs along with associated objects.")]
        public PushSubOptions AddVerb { get; set; }

        [VerbOption("tag", HelpText = "Update remote refs along with associated objects.")]
        public TagSubOptions TagVerb { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var logger = DependencyInjector.Resolve<ILog>();
            logger.Info("PriceBasket Service Started");
            var basketEconomicsManager = DependencyInjector.Resolve<IBasketPricer>();

            string invokedVerb = "";
            object invokedVerbInstance = new object();

            var options = new Options();

            if (!CommandLine.Parser.Default.ParseArgumentsStrict(args, options,
              (verb, subOptions) => {
              // if parsing succeeds the verb name and correct instance
              // will be passed to onVerbCommand delegate (string,object)
              invokedVerb = verb;
              invokedVerbInstance = subOptions;
              logger.Info("Commandline Parsing succeeded");
              logger.Info(invokedVerb);
              logger.Info(JsonConvert.SerializeObject(invokedVerbInstance));

              })) {
                logger.Error("Parsing failed");
                Environment.Exit(CommandLine.Parser.DefaultExitCodeFail);
            }

            if (invokedVerb == "commit")
            {
                var commitSubOptions = (CommitSubOptions)invokedVerbInstance;
                logger.Info(JsonConvert.SerializeObject(commitSubOptions));
            }

            //String command;
            //    Boolean quitNow = false;
            //    while (!quitNow)
            //    {
            //        Console.Write(">");
            //        command = Console.ReadLine();
            //        Console.WriteLine(command);
            //        switch (command.ToLower())
            //        {
            //            case "help":
            //                Console.WriteLine("This should be help.");
            //                break;

            //            case "pricebasket":
            //            {
            //                Console.Write("PriceBasket>");
            //                command = Console.ReadLine();
            //                break;
            //            }
            //            case "quit":
            //                quitNow = true;
            //                break;

            //            default:
            //                Console.WriteLine("Unknown Command " + command);
            //                break;
            //        }
            //    }
        }
    }
}

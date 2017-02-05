using CommandLine;
using CommandLine.Text;

namespace PriceBasket.Business.Verbs
{
    class Options
    {
        public Options()
        {
            // Since we create this instance the parser will not overwrite it
            //PriceBasketVerb = new PriceBasketSubOptions { };
        }

        [VerbOption("pricebasket", HelpText = "verb to price a basket of Items. This shows basket Subtotal , discount and Total")]
        public PriceBasketSubOptions PriceBasketVerb { get; set; }

        [VerbOption("puteconomics", HelpText = "verb to add or update basket item economics.")]
        public PutEconomicsSubOptions PutEconomicsVerb { get; set; }

        [VerbOption("quit", HelpText = "verb to quit")]
        public QuitSubOptions QuitVerb { get; set; }

        [HelpVerbOption]
        public string GetUsage(string verb)
        {
            return HelpText.AutoBuild(this, verb);
        }
    }
}
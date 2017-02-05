using CommandLine;

namespace PriceBasket.Business.Verbs
{
    class PriceBasketSubOptions
    {
        [Option('b', "basket", Required = true,
            HelpText = "Json formatted string representing a basket to price formatted as [{\"Name\":<name_item1>,\"Unit\":<unit_item1>},{\"Name\":<name_item2>,\"Unit\":<unit_item2>}]. Usage eg pricebasket --basket \"[{'Name':'Apple', 'Unit':5},{'Name':'Oil', 'Unit':5}]\"")]
        public string Basket { get; set; }

    }
}
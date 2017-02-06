using CommandLine;

namespace PriceBasket.Business.Verbs
{
    class PutEconomicsSubOptions
    {
        [Option('i', "itemeconomics", Required = true,
            HelpText = "Json formatted string representing a item economics [{\"Name\":<name_item1>,\"Price\":<unit_price1>,\"Discount\":<unit_discount1>},{\"Name\":<name_item2>,\"Price\":<unit_price2>,\"Discount\":<unit_discount2>}]. Usage eg puteconomics --itemeconomics \"[{'Name':'Apple','Price':1.00,'Discount':0.1},{'Name':'Bread','Price':0.80,'Discount':null},{'Name':'Milk','Price':1.30,'Discount':null},{'Name':'Soup','Price':0.65,'Discount':null}]\"")]
        public string ItemEconomics { get; set; }
    }
}
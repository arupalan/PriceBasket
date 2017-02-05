namespace PriceBasket.Business.Verbs
{
    public interface ICommandProcessor
    {
        bool ProcessVerbs(string[] args);
        bool ProcessVerbs();
    }
}
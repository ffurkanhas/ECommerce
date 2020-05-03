namespace ECommerce.Interfaces
{
    public interface IDeliveryCostCalculator
    {
        double CalculateFor(IShoppingCart cart);
    }
}

namespace ECommerce
{
    public interface IDeliveryCostCalculator
    {
        double CalculateFor(IShoppingCart cart);
    }
}

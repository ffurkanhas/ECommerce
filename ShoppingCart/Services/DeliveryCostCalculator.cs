using ECommerce.Interfaces;
using System;

namespace ECommerce.Services
{
    public class DeliveryCostCalculator : IDeliveryCostCalculator
    {
        public DeliveryCostCalculator(double costPerDelivery, double costPerProduct, double fixedCost)
        {
            CostPerDelivery = costPerDelivery;
            CostPerProduct = costPerProduct;
            FixedCost = fixedCost;
        }

        public double CostPerDelivery { get; private set; }

        public double CostPerProduct { get; private set; }

        public double FixedCost { get; private set; }

        public double CalculateFor(IShoppingCart cart)
        {
            if (cart == null)
            {
                throw new ArgumentNullException("Cart cannot be null");
            }

            return (cart.GetTotalDeliveryAmount() * CostPerDelivery) + (cart.GetTotalItemAmount() * CostPerProduct) + FixedCost;
        }
    }
}

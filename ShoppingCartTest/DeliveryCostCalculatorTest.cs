using ECommerce;
using ECommerce.Interfaces;
using ECommerce.Services;
using Moq;
using System;
using Xunit;

namespace ShoppingCartTest
{
    public class DeliveryCostCalculatorTest
    {
        [Fact]
        public void DeliveryCostCalculatorShouldHaveCosts()
        {
            DeliveryCostCalculator deliveryCostCalculator = new DeliveryCostCalculator(10.0, 5.0, 2.99);

            Assert.Equal(10.0, deliveryCostCalculator.CostPerDelivery);
            Assert.Equal(5.0, deliveryCostCalculator.CostPerProduct);
            Assert.Equal(2.99, deliveryCostCalculator.FixedCost);
        }

        [Fact]
        public void DeliveryCostCalculatorShouldThrowsExceptionWhenCartIsNull()
        {

            DeliveryCostCalculator deliveryCostCalculator = new DeliveryCostCalculator(10.0, 5.0, 2.99);

            Assert.Throws<ArgumentNullException>(() => deliveryCostCalculator.CalculateFor(null));
        }

        [Fact]
        public void DeliveryCostCalculatorShouldReturnFixedCostWhenCartHasNoItems()
        {
            Mock<IShoppingCart> cart = new Mock<IShoppingCart>();

            DeliveryCostCalculator deliveryCostCalculator = new DeliveryCostCalculator(10.0, 5.0, 2.99);
            cart.Setup(x => x.GetTotalItemAmount()).Returns(0);
            cart.Setup(x => x.GetTotalDeliveryAmount()).Returns(0);

            Assert.Equal(2.99, deliveryCostCalculator.CalculateFor(cart.Object));
        }

        [Fact]
        public void CalculateFor_CartWithNoProduct_ReturnsFixedCost()
        {
            Mock<IShoppingCart> cart = new Mock<IShoppingCart>();

            DeliveryCostCalculator deliveryCostCalculator = new DeliveryCostCalculator(10.0, 5.0, 2.99);
            cart.Setup(x => x.GetTotalItemAmount()).Returns(2);
            cart.Setup(x => x.GetTotalDeliveryAmount()).Returns(1);

            Assert.Equal(22.99, deliveryCostCalculator.CalculateFor(cart.Object), 2);
        }
    }
}

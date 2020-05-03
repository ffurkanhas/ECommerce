using ECommerce;
using Moq;
using System;
using Xunit;

namespace ShoppingCartTest
{
    public class ShoppingCartTest
    {
        [Fact]
        public void ShoppingCartShouldHaveItemListCampaignList()
        {
            Mock<IDeliveryCostCalculator> deliveryCostCalculator = new Mock<IDeliveryCostCalculator>();

            ShoppingCart shoppingCart = new ShoppingCart();

            Assert.NotNull(shoppingCart.Items);
            Assert.NotNull(shoppingCart.Campaigns);
            Assert.Null(shoppingCart.Coupon);
        }

        [Fact]
        public void ShoppingCartShouldNotAddNullProducts()
        {
            Product product = null;

            ShoppingCart shoppingCart = new ShoppingCart();

            shoppingCart.AddItem(product, 5);

            Assert.Empty(shoppingCart.Items);
        }

        [Fact]
        public void ShoppingCartShouldNotAddAmountLowerThan0()
        {
            Category category = new Category("Food");

            Product product = new Product("Apple", 15.0, category);

            ShoppingCart shoppingCart = new ShoppingCart();

            shoppingCart.AddItem(product, -1);

            Assert.Empty(shoppingCart.Items);
        }

        [Fact]
        public void ShoppingCartShouldHaveItemsWithCorrectAmounts()
        {
            Category category = new Category("Food");

            Product apple = new Product("Apple", 15.0, category);
            Product banana = new Product("Banana", 10.0, category);

            ShoppingCart shoppingCart = new ShoppingCart();

            shoppingCart.AddItem(apple, 3);
            shoppingCart.AddItem(apple, 5);
            shoppingCart.AddItem(banana, 5);

            Assert.Equal(8, shoppingCart.Items[apple]);
            Assert.Equal(5, shoppingCart.Items[banana]);
            Assert.Equal(170.0, shoppingCart.TotalAmount);
        }

        [Fact]
        public void ShoppingCartShouldNotAddNullCampaings()
        {
            ShoppingCart shoppingCart = new ShoppingCart();

            Assert.Throws<ArgumentNullException>(() => shoppingCart.ApplyDiscounts(null));
        }

        [Fact]
        public void ShoppingCartShouldApplyDiscounts()
        {
            Category category = new Category("Food");

            ShoppingCart shoppingCart = new ShoppingCart();

            Campaign campaign = new Campaign(category, 10, 100, DiscountType.Rate);
            Campaign campaign2 = new Campaign(category, 2, 100, DiscountType.Amount);

            shoppingCart.ApplyDiscounts(campaign, null, campaign2);

            Assert.Equal(2, shoppingCart.Campaigns.Count);
        }

        [Fact]
        public void ShoppingCartShouldNotAddNullCoupon()
        {
            ShoppingCart shoppingCart = new ShoppingCart();

            Assert.Throws<ArgumentNullException>(() => shoppingCart.ApplyCoupon(null));
        }

        [Fact]
        public void ShoppingCartShouldApplyCoupon()
        {
            ShoppingCart shoppingCart = new ShoppingCart();

            Coupon coupon = new Coupon(100, 20.0, DiscountType.Rate);

            shoppingCart.ApplyCoupon(coupon);

            Assert.Equal(100, shoppingCart.Coupon.MinimumAmount);
        }

        [Fact]
        public void ShoppingCartShouldNotApplyDiscountWhenThereIsNoCampaing()
        {
            Category category = new Category("Food");

            Product product = new Product("Apple", 10.0, category);

            ShoppingCart shoppingCart = new ShoppingCart();

            shoppingCart.AddItem(product, 5);

            Assert.Equal(0, shoppingCart.GetCampaignDiscount());
        }

        [Fact]
        public void ShoppingCartShouldThrowExceptionWhenThereIsNoItem()
        {
            Category category = new Category("Food");

            ShoppingCart shoppingCart = new ShoppingCart();

            Campaign campaign = new Campaign(category, 10, 3, DiscountType.Rate);

            shoppingCart.ApplyDiscounts(campaign);

            Assert.Equal(0, shoppingCart.GetCampaignDiscount());
        }

        [Fact]
        public void ShoppingCartShouldNotApplyMaximumDiscountWithNoItemsForCampaing()
        {
            Category category = new Category("Food");

            ShoppingCart shoppingCart = new ShoppingCart();

            Campaign campaign = new Campaign(category, 10, 3, DiscountType.Rate);

            shoppingCart.ApplyDiscounts(campaign);

            Assert.Equal(0, shoppingCart.GetCampaignDiscount());
        }

        [Fact]
        public void ShoppingCartShouldApplyMaximumDiscountWithRateForCampaing()
        {
            Category category = new Category("Food");
            Category category2 = new Category("Toys");

            Product apple = new Product("Apple", 10.0, category);
            Product rcCar = new Product("Rc Car", 1.0, category2);

            ShoppingCart shoppingCart = new ShoppingCart();

            shoppingCart.AddItem(apple, 5);
            shoppingCart.AddItem(rcCar, 3);

            Campaign campaign = new Campaign(category, 10, 3, DiscountType.Rate);
            Campaign campaign2 = new Campaign(category2, 50, 3, DiscountType.Rate);

            shoppingCart.ApplyDiscounts(campaign, campaign2);

            Assert.Equal(5, shoppingCart.GetCampaignDiscount());
        }

        [Fact]
        public void ShoppingCartShouldApplyMaximumDiscountWithAmountForCampaing()
        {
            Category category = new Category("Food");
            Category category2 = new Category("Toys");

            Product apple = new Product("Apple", 10.0, category);
            Product rcCar = new Product("Rc Car", 1.0, category2);

            ShoppingCart shoppingCart = new ShoppingCart();

            shoppingCart.AddItem(apple, 5);
            shoppingCart.AddItem(rcCar, 3);

            Campaign campaign = new Campaign(category, 2, 3, DiscountType.Amount);
            Campaign campaign2 = new Campaign(category2, 1, 3, DiscountType.Amount);

            shoppingCart.ApplyDiscounts(campaign, campaign2);

            Assert.Equal(20.0, shoppingCart.GetCampaignDiscount());
        }

        [Fact]
        public void ShoppingCartShouldNotAppylDiscountForCouponWhenThereIsNoCoupon()
        {
            ShoppingCart shoppingCart = new ShoppingCart();

            Assert.Equal(0, shoppingCart.GetCouponDiscount());
        }

        [Fact]
        public void ShoppingCartShouldNotAppylDiscountForCouponWhenTotalAmountLessThanMinimumAmount()
        {
            Category category = new Category("Food");
            Category category2 = new Category("Toys");

            Product apple = new Product("Apple", 10.0, category);
            Product rcCar = new Product("Rc Car", 1.0, category2);

            ShoppingCart shoppingCart = new ShoppingCart();

            shoppingCart.AddItem(apple, 5);
            shoppingCart.AddItem(rcCar, 3);

            Coupon coupon = new Coupon(100, 10, DiscountType.Amount);

            shoppingCart.ApplyCoupon(coupon);

            Assert.Equal(0, shoppingCart.GetCouponDiscount());
        }

        [Fact]
        public void ShoppingCartShouldAppylDiscountWithAmountForCoupon()
        {
            Category category = new Category("Food");
            Category category2 = new Category("Toys");

            Product apple = new Product("Apple", 10.0, category);
            Product rcCar = new Product("Rc Car", 1.0, category2);

            ShoppingCart shoppingCart = new ShoppingCart();

            shoppingCart.AddItem(apple, 5);
            shoppingCart.AddItem(rcCar, 3);

            Coupon coupon = new Coupon(50, 10, DiscountType.Amount);

            shoppingCart.ApplyCoupon(coupon);

            Assert.Equal(10, shoppingCart.GetCouponDiscount());
        }

        [Fact]
        public void ShoppingCartShouldAppylDiscountWithRateForCoupon()
        {
            Category category = new Category("Food");
            Category category2 = new Category("Toys");

            Product apple = new Product("Apple", 10.0, category);
            Product rcCar = new Product("Rc Car", 1.0, category2);

            ShoppingCart shoppingCart = new ShoppingCart();

            shoppingCart.AddItem(apple, 5);
            shoppingCart.AddItem(rcCar, 3);

            Coupon coupon = new Coupon(50, 10, DiscountType.Rate);

            shoppingCart.ApplyCoupon(coupon);

            Assert.Equal(5.3, shoppingCart.GetCouponDiscount());
        }

        [Fact]
        public void ShoppingCartShouldGetTotalDiscountAmount()
        {
            Category category = new Category("Food");
            Category category2 = new Category("Toys");

            Product apple = new Product("Apple", 10.0, category);
            Product rcCar = new Product("Rc Car", 1.0, category2);

            ShoppingCart shoppingCart = new ShoppingCart();

            shoppingCart.AddItem(apple, 10);
            shoppingCart.AddItem(rcCar, 3);

            Campaign campaign = new Campaign(category, 2, 3, DiscountType.Amount);
            Campaign campaign2 = new Campaign(category2, 1, 3, DiscountType.Amount);

            shoppingCart.ApplyDiscounts(campaign, campaign2);

            Coupon coupon = new Coupon(50, 10, DiscountType.Amount);

            shoppingCart.ApplyCoupon(coupon);

            Assert.Equal(73, shoppingCart.GetTotalAmountAfterDiscounts());
        }

        [Fact]
        public void ShoppingCartShouldReturnNumberOfItems()
        {
            Category category = new Category("Food");
            Category category2 = new Category("Toys");

            Product apple = new Product("Apple", 10.0, category);
            Product rcCar = new Product("Rc Car", 1.0, category2);

            ShoppingCart shoppingCart = new ShoppingCart();

            shoppingCart.AddItem(apple, 10);
            shoppingCart.AddItem(rcCar, 3);

            Assert.Equal(13, shoppingCart.GetTotalItemAmount());
        }

        [Fact]
        public void ShoppingCartShouldReturnNumberOfCategory()
        {
            Category category = new Category("Food");
            Category category2 = new Category("Toys");
            Category category3 = new Category("Drink");

            Product apple = new Product("Apple", 10.0, category);
            Product rcCar = new Product("Rc Car", 1.0, category2);
            Product milk = new Product("Milk", 1.5, category3);

            ShoppingCart shoppingCart = new ShoppingCart();

            shoppingCart.AddItem(apple, 10);
            shoppingCart.AddItem(rcCar, 3);
            shoppingCart.AddItem(milk, 5);

            Assert.Equal(3, shoppingCart.GetTotalDeliveryAmount());
        }

        [Fact]
        public void ShoppingCartShouldReturnDeliveryCost()
        {
            Category category = new Category("Food");
            Category category2 = new Category("Toys");
            Category category3 = new Category("Drink");

            Product apple = new Product("Apple", 10.0, category);
            Product rcCar = new Product("Rc Car", 1.0, category2);
            Product milk = new Product("Milk", 1.5, category3);

            ShoppingCart shoppingCart = new ShoppingCart();

            shoppingCart.AddItem(apple, 10);
            shoppingCart.AddItem(rcCar, 3);
            shoppingCart.AddItem(milk, 5);

            Assert.Equal(122.99, shoppingCart.GetDeliveryCost());
        }
    }
}

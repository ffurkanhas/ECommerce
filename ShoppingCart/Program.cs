namespace ECommerce
{
    class Program
    {
        static void Main(string[] args)
        {
            Category food = new Category("food");
            
            Product apple = new Product("Apple", 100.5, food);
            Product almond = new Product("Almonds", 150.0, food);

            ShoppingCart cart = new ShoppingCart();
            cart.AddItem(apple, 3);
            cart.AddItem(almond, 1);

            Campaign campaign1 = new Campaign(food, 20.0, 3, DiscountType.Rate);
            Campaign campaign2 = new Campaign(food, 50.0, 5, DiscountType.Rate);
            Campaign campaign3 = new Campaign(food, 5.0, 5, DiscountType.Amount);

            cart.ApplyDiscounts(campaign1, campaign2, campaign3);

            // Firstly apply campaings after that apply coupons
            Coupon coupon = new Coupon(100, 10, DiscountType.Rate);
            cart.ApplyCoupon(coupon);

            // Delivery part
            // Formula = (CostPerDelivery * NumberOfDeliveries) + (CostPerProduct * NumberOfProducts) + FixedCost
            // FixedCost = 2.99
            // NumberOfDeliveries = # of distinct categories in the cart
            // NumberOfProducts = # of different products in the cart, not the quantity of products
            DeliveryCostCalculator deliveryCostCalculator = new DeliveryCostCalculator(10.0, 5.0, 2.99);

            // Double
            deliveryCostCalculator.CalculateFor(cart);

            cart.GetTotalAmountAfterDiscounts();
            cart.GetCampaignDiscount();
            cart.GetCouponDiscount();
            cart.GetDeliveryCost();

            // Group products by Category and Print the CategoryName, ProductName, Quantity, Unit Price, Total Price, Total Discount
            cart.Print();
            
        }
    }
}

using ECommerce.Enums;

namespace ECommerce.Entities
{
    public class Coupon : DiscountBase
    {
        public Coupon(int minimumAmount, double discount, DiscountType discountType)
            : base(discount, minimumAmount, discountType)
        {
        }

        public double CalculateDiscount(double totalAmount)
        {
            if (totalAmount <= MinimumAmount)
            {
                return 0;
            }

            switch (DiscountType)
            {
                case DiscountType.Rate:
                    return totalAmount * Discount / 100;
                case DiscountType.Amount:
                    return Discount;
                default:
                    return 0;
            }
        }
    }
}

using ECommerce.Enums;

namespace ECommerce.Entities
{
    public abstract class DiscountBase
    {
        public DiscountBase(double discount, int minimumAmount, DiscountType discountType)
        {
            Discount = discount;
            MinimumAmount = minimumAmount;
            DiscountType = discountType;
        }

        public double Discount { get; private set; }

        public int MinimumAmount { get; private set; }

        public DiscountType DiscountType { get; private set; }
    }
}

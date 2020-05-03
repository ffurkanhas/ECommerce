using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce
{
    public class Coupon
    {
        public Coupon(int minimumAmount, double discount, DiscountType discountType)
        {
            MinimumAmount = minimumAmount;
            Discount = discount;
            DiscountType = discountType;
        }
        public int MinimumAmount { get; private set; }

        public double Discount { get; private set; }

        public DiscountType DiscountType { get; private set; }
    }
}

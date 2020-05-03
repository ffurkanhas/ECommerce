using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce
{
    public class Campaign
    {
        public Campaign(Category category, double discount, int minimumAmount, DiscountType discountType)
        {
            Category = category;
            Discount = discount;
            MinimumAmount = minimumAmount;
            DiscountType = discountType;
        }

        public Category Category { get; private set; }

        public double Discount { get; private set; }

        public int MinimumAmount { get; private set; }

        public DiscountType DiscountType { get; private set; }
    }
}

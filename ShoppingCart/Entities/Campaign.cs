using ECommerce.Enums;
using System.Collections.Generic;

namespace ECommerce.Entities
{
    public class Campaign : DiscountBase
    {
        public Campaign(Category category, double discount, int minimumAmount, DiscountType discountType)
            : base(discount, minimumAmount, discountType)
        {
            Category = category;
        }

        public Category Category { get; private set; }

        public double CalculateDiscount(HashSet<ShoppingCartItem> items)
        {
            double discount = 0;
            foreach (ShoppingCartItem item in items)
            {
                if (item.Amount >= MinimumAmount)
                {
                    switch (DiscountType)
                    {
                        case DiscountType.Rate:
                            discount += (item.Amount * item.Product.Price) * Discount / 100;
                            break;
                        case DiscountType.Amount:
                            discount += (item.Product.Price * Discount);
                            break;
                        default:
                            break;
                    }
                }
            }

            return discount;
        }
    }
}

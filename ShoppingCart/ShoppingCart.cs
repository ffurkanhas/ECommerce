using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECommerce
{
    public class ShoppingCart : IShoppingCart
    {
        public ShoppingCart()
        {
            Items = new Dictionary<Product, int>();
            Campaigns = new List<Campaign>();
        }

        public IDictionary<Product, int> Items { get; private set; }

        public IList<Campaign> Campaigns { get; private set; }

        public Coupon Coupon { get; set; }

        public double TotalAmount { get; private set; }

        // Add product to cart with amount, if product added before just increase amount
        public void AddItem(Product product, int amount)
        {
            if (product == null || amount <= 0)
            {
                return;
            }

            if (Items.ContainsKey(product))
            {
                Items[product] = Items[product] + amount;
            }
            else
            {
                Items.Add(product, amount);
            }

            TotalAmount += product.Price * amount;
        }

        // Just adds campaigns to shopping cart
        public void ApplyDiscounts(params Campaign[] campaigns)
        {
            if (campaigns == null)
            {
                throw new ArgumentNullException("Campaigns cannot be null");
            }

            foreach (Campaign campaign in campaigns)
            {
                if (campaign == null)
                {
                    continue;
                }

                Campaigns.Add(campaign);
            }
        }

        public void ApplyCoupon(Coupon coupon)
        {
            if (coupon == null)
            {
                throw new ArgumentNullException("Coupon cannot be null");
            }

            Coupon = coupon;
        }

        public double GetCampaignDiscount()
        {
            if (!Campaigns.Any())
            {
                return 0;
            }

            double maximumDiscount = 0;
            foreach (Campaign campaign in Campaigns)
            {
                IDictionary<Product, int> productsInSameCategoryWithCampaign = FindProductsByCategory(campaign.Category);
                double discount = 0;
                if (productsInSameCategoryWithCampaign != null && productsInSameCategoryWithCampaign.Any())
                {
                    foreach (KeyValuePair<Product, int> keyValuePair in productsInSameCategoryWithCampaign)
                    {
                        if (keyValuePair.Value >= campaign.MinimumAmount)
                        {
                            switch (campaign.DiscountType)
                            {
                                case DiscountType.Rate:
                                    discount += (keyValuePair.Value * keyValuePair.Key.Price) * campaign.Discount / 100;
                                    break;
                                case DiscountType.Amount:
                                    discount += (keyValuePair.Key.Price * campaign.Discount);
                                    break;
                                default:
                                    break;
                            }

                            if (discount >= maximumDiscount)
                            {
                                maximumDiscount = discount;
                            }
                        }
                    }
                }
            }

            return maximumDiscount;
        }

        public double GetCouponDiscount()
        {
            if (Coupon == null)
            {
                return 0;
            }

            if (TotalAmount < Coupon.MinimumAmount)
            {
                return 0;
            }

            switch (Coupon.DiscountType)
            {
                case DiscountType.Rate:
                    return TotalAmount * Coupon.Discount / 100;
                case DiscountType.Amount:
                    return Coupon.Discount;
                default:
                    return 0;
            }
        }

        public double GetTotalAmountAfterDiscounts()
        {
            TotalAmount -= GetCampaignDiscount();
            TotalAmount -= GetCouponDiscount();
            return TotalAmount;
        }

        public int GetTotalItemAmount()
        {
            return Items.Sum(x => x.Value);
        }

        public int GetTotalDeliveryAmount()
        {
            return Items.GroupBy(x => x.Key.Category).Count();
        }

        public double GetDeliveryCost()
        {
            IDeliveryCostCalculator deliveryCostCalculator = new DeliveryCostCalculator(10.0, 5.0, 2.99); // This is wrong for SRP but I dont want to change your design
            return deliveryCostCalculator.CalculateFor(this);
        }

        private IDictionary<Product, int> FindProductsByCategory(Category category)
        {
            if (!Items.Any())
            {
                return null;
            }

            return Items.Where(x => x.Key.Category.Title == category.Title).ToDictionary(x => x.Key, x => x.Value);
        }

        public void Print()
        {
            StringBuilder builder = new StringBuilder();
            var items = Items.GroupBy(x => x.Key.Category.Title).ToDictionary(x => x.Key, x => x.ToList());

            builder.AppendLine(string.Format("{0,-20} | {1,-20} | {2,-10} | {3,-10} | {4,-11} |", "CategoryName", "ProductName", "Quantity", "Unit Price", "Total Price"));
            builder.AppendLine("-------------------------------------------------------------------------------------");
            foreach (var item in items)
            {
                foreach (var product in item.Value)
                {
                    builder.AppendLine(string.Format("{0,-20} | {1,-20} | {2,-10} | {3,-10} | {4,-11} |", item.Key, product.Key.Title, product.Value, product.Key.Price, product.Value * product.Key.Price));
                }
            }
            builder.AppendLine("-------------------------------------------------------------------------------------");
            builder.AppendLine(string.Format("{0,-20} | {1,-20}", "Total Amount", "Delivery Cost"));
            builder.AppendLine(string.Format("{0,-20} | {1,-20}", GetTotalAmountAfterDiscounts(), GetDeliveryCost()));

            Console.WriteLine(builder.ToString());
        }
    }
}

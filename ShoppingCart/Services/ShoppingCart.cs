using ECommerce.Entities;
using ECommerce.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECommerce.Services
{
    public class ShoppingCart : IShoppingCart
    {
        public ShoppingCart()
        {
            Items = new HashSet<ShoppingCartItem>();
            Campaigns = new HashSet<Campaign>();
        }

        public HashSet<ShoppingCartItem> Items { get; private set; }

        public HashSet<Campaign> Campaigns { get; private set; } = new HashSet<Campaign>();

        public Coupon Coupon { get; private set; }

        public double TotalAmount { get; private set; }

        // Add product to cart with amount, if product added before just increase amount
        public void AddItem(Product product, int amount)
        {
            if (product == null || amount <= 0)
            {
                return;
            }

            ShoppingCartItem item = Items.FirstOrDefault(x => x.Product.Title == product.Title);

            if (item == null)
            {
                Items.Add(new ShoppingCartItem(product, amount));
            }
            else
            {
                item.Amount += amount;
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
            Coupon = coupon ?? throw new ArgumentNullException("Coupon cannot be null");
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
                var discount = campaign.CalculateDiscount(FindProductsByCategory(campaign.Category));
                if (discount > maximumDiscount)
                {
                    maximumDiscount = discount;
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

            return Coupon.CalculateDiscount(TotalAmount);
        }

        public double GetTotalAmountAfterDiscounts()
        {
            TotalAmount -= GetCampaignDiscount();
            TotalAmount -= GetCouponDiscount();
            return TotalAmount;
        }

        public int GetTotalItemAmount()
        {
            return Items.Sum(x => x.Amount);
        }

        public int GetTotalDeliveryAmount()
        {
            return Items.GroupBy(x => x.Product.Category).Count();
        }

        public double GetDeliveryCost()
        {
            IDeliveryCostCalculator deliveryCostCalculator = new DeliveryCostCalculator(10.0, 5.0, 2.99); // This is wrong for SRP but I dont want to change your design
            return deliveryCostCalculator.CalculateFor(this);
        }

        private HashSet<ShoppingCartItem> FindProductsByCategory(Category category)
        {
            if (!Items.Any())
            {
                return Enumerable.Empty<ShoppingCartItem>().ToHashSet(); //Empty list
            }

            return Items.Where(x => x.Product.Category.Title == category.Title).ToHashSet();
        }

        public void Print()
        {

            StringBuilder builder = new StringBuilder();
            var items = Items.GroupBy(x => x.Product.Category.Title);

            builder.AppendLine(string.Format("{0,-20} | {1,-20} | {2,-10} | {3,-10} | {4,-11} |", "CategoryName", "ProductName", "Quantity", "Unit Price", "Total Price"));
            builder.AppendLine("-------------------------------------------------------------------------------------");
            foreach (var item in items)
            {
                foreach (var productItem in item.Select(x => x).ToList())
                {
                    builder.AppendLine(string.Format("{0,-20} | {1,-20} | {2,-10} | {3,-10} | {4,-11} |", item.Key, productItem.Product.Title, productItem.Amount, productItem.Product.Price, productItem.Amount * productItem.Product.Price));
                }
            }
            builder.AppendLine("-------------------------------------------------------------------------------------");
            builder.AppendLine(string.Format("{0,-20} | {1,-20}", "Total Amount", "Delivery Cost"));
            builder.AppendLine(string.Format("{0,-20} | {1,-20}", GetTotalAmountAfterDiscounts(), GetDeliveryCost()));

            Console.WriteLine(builder.ToString());
        }
    }
}

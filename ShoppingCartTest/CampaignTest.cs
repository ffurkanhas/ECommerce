using ECommerce;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ShoppingCartTest
{
    public class CampaignTest
    {
        [Fact]
        public void CampaignShouldHaveCategoryPercentageAmountType()
        {
            Category category = new Category("Food");

            Campaign campaign = new Campaign(category, 20.0, 3, DiscountType.Rate);

            Assert.Equal(category, campaign.Category);
            Assert.Equal(20.0, campaign.Discount);
            Assert.Equal(3, campaign.MinimumAmount);
            Assert.Equal(DiscountType.Rate, campaign.DiscountType);
        }
    }
}

using ECommerce;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ShoppingCartTest
{
    public class CouponTest
    {
        [Fact]
        public void CouponShouldHaveMinimumAmountPercentageType()
        {
            Coupon coupon = new Coupon(100, 2, DiscountType.Amount);

            Assert.Equal(100, coupon.MinimumAmount);
            Assert.Equal(2, coupon.Discount);
            Assert.Equal(DiscountType.Amount, coupon.DiscountType);
        }
    }
}

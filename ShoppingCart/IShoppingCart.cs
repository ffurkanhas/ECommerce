namespace ECommerce
{
    public interface IShoppingCart
    {
        void AddItem(Product product, int amount);

        void ApplyDiscounts(params Campaign[] campaigns);

        void ApplyCoupon(Coupon coupon);

        double GetCampaignDiscount();

        double GetCouponDiscount();

        double GetTotalAmountAfterDiscounts();

        int GetTotalItemAmount();

        int GetTotalDeliveryAmount();

        void Print();
    }
}
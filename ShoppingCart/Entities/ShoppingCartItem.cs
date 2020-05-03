namespace ECommerce.Entities
{
    public class ShoppingCartItem
    {
        public ShoppingCartItem(Product product, int amount)
        {
            Product = product;
            Amount = amount;
        }

        public Product Product { get; set; }

        public int Amount { get; set; }
    }
}

using System;

namespace ECommerce
{
    public class Product
    {
        public Product(string title, double price, Category category)
        {
            if (string.IsNullOrWhiteSpace(title) || price <= 0)
            {
                throw new Exception();
            }

            Title = title;
            Price = price;
            Category = category;
        }

        public string Title { get; private set; }

        public double Price { get; private set; }

        public Category Category { get; private set; }
    }
}

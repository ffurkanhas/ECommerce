using ECommerce;
using ECommerce.Entities;
using System;
using Xunit;

namespace ShoppingCartTest
{
    public class ProductTest
    {
        [Fact]
        public void ProductShouldHaveTitlePriceAndCategory()
        {
            Category category = new Category("Food");

            Product product = new Product("Apple", 15.0, category);

            Assert.Equal("Apple", product.Title);
            Assert.Equal(15.0, product.Price);
            Assert.Equal(category, product.Category);
        }
    }
}

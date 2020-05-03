using ECommerce;
using System;
using Xunit;

namespace ShoppingCartTest
{
    public class ProductTest
    {
        [Fact]
        public void ProductShouldThrowExceptionWhenTitleIsEmpty()
        {
            Category category = new Category("Food");

            Assert.Throws<Exception>(() => new Product("", 10.0, category));
        }

        [Fact]
        public void ProductShouldThrowExceptionWhenPriceLowerThanZero()
        {
            Category category = new Category("Food");

            Assert.Throws<Exception>(() => new Product("Apple", -5, category));
        }

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

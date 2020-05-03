using ECommerce;
using ECommerce.Entities;
using System;
using Xunit;

namespace ShoppingCartTest
{
    public class CategoryTest
    {
        [Fact]
        public void CategoryShouldHaveTitle()
        {
            Category category = new Category("Food");

            Assert.Equal("Food", category.Title);
        }

        [Fact]
        public void CategoryMayHaveParentCategory()
        {
            Category category = new Category("ParentCategory");

            Category childCategory = new Category("ChildCategory", category);

            Assert.Equal(category, childCategory.ParentCategory);
        }
    }
}

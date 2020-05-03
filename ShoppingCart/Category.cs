using System;

namespace ECommerce
{
    public class Category
    {
        public Category(string title, Category parentCategory = null)
        {
            Title = title;
            ParentCategory = parentCategory;
        }

        public string Title { get; private set; }

        public Category ParentCategory { get; private set; }
    }
}

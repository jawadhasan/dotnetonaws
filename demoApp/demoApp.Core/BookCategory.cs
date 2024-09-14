using System;

namespace demoApp.Core
{
    public class BookCategory
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
    }

    public class Book
    {
        public int Id { get; set; }
        public int BookCategoryId { get; set; }
        public string BookName { get; set; }
        public int Price { get; set; }
        public DateTime Created { get; set; }
    }
}

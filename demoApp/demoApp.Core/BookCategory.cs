using System;
using System.Collections.Generic;

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

        public List<Author> Authors { get; set; } = new List<Author>();
    }


    public class Author
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
    }

    public class BookAuthor
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int AuthorId { get; set; }

    }
}

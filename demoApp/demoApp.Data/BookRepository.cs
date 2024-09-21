using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using demoApp.Core;
using System.Linq;

namespace demoApp.Data
{
    public class BookRepository
    {
        private readonly IDbConnection _db;

        //ctor
        public BookRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Book>> GetBooks()
        {
            return await _db.QueryAsync<Book>("SELECT * FROM book");
        }

        public dynamic GetById(int id)
        {
            // var sql = @"SELECT * FROM book WHERE id = @id";
            var sql = @"
                        SELECT 
                            b.id AS id,
                            b.bookname AS bookname,
                            b.bookcategoryid, b.price,
                            ARRAY_AGG(ba.authorid) AS author_ids
                        FROM 
                            book b
                        INNER JOIN 
                            bookauthor ba ON b.id = ba.bookid

                        WHERE 
                            b.id = @id
                        GROUP BY 
                            b.id, b.bookname;";



            return _db.Query<dynamic>(sql, new { id }).SingleOrDefault();
        }

        public async Task<IEnumerable<Book>> GetBooksByCategory(int catId)
        {
            var sql = @"SELECT * FROM book WHERE bookcategoryid = @catId";

            return await _db.QueryAsync<Book>(sql, new { catId });

        }

        public async Task<IEnumerable<BookCategory>> GetCategories()
        {
            return await _db.QueryAsync<BookCategory>("SELECT * FROM bookcategory");
        }


        public async Task<Author> GetAuthor(int id)
        {
            var sql = "SELECT * FROM author WHERE id = @id";
            return await _db.QueryFirstOrDefaultAsync<Author>(sql, new {id});
        }


        public async Task<IEnumerable<Book>> GetBooksWithAuthorAsync()
        {
            var sql = @"
        SELECT 
            b.id, b.bookname, 
            a.id, a.authorname 
        FROM book b
        INNER JOIN bookauthor ba ON b.id = ba.bookid
        INNER JOIN author a ON ba.authorid = a.id";


            var bookDictionary = new Dictionary<int, Book>();

            var result = _db.Query<Book, Author, Book>(sql,
                (book, author) =>
                {
                    if (!bookDictionary.TryGetValue(book.Id, out var bookEntry))
                    {
                        bookEntry = book;
                        bookEntry.Authors = new List<Author>();
                        bookDictionary.Add(book.Id, bookEntry);
                    }

                    bookEntry.Authors.Add(author);
                    return bookEntry;
                },
                splitOn: "id");

            var books = bookDictionary.Values;

            return books;

        }


        //PostgreSQL’s ARRAY_AGG function. This function allows you to aggregate multiple rows into a single array,
        public async Task<IEnumerable<dynamic>> GetBookWithAuthorIdsAsync(int catId)
        {
            var sql = @"
                        SELECT 
                            b.id AS id,
                            b.bookname AS bookname,
                            ARRAY_AGG(ba.authorid) AS author_ids
                        FROM 
                            book b
                        INNER JOIN 
                            bookauthor ba ON b.id = ba.bookid

                        WHERE 
                            b.bookcategoryid = @catId
                        GROUP BY 
                            b.id, b.bookname;";

            return await _db.QueryAsync<dynamic>(sql, new {catId});
        }

    }
}

using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using demoApp.Core;

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

        public async Task<IEnumerable<Book>> GetBooksByCategory(int catId)
        {
            var sql = @"SELECT * FROM book WHERE bookcategoryid = @catId";

            return await _db.QueryAsync<Book>(sql, new { catId });

        }

        public async Task<IEnumerable<BookCategory>> GetCategories()
        {
            return await _db.QueryAsync<BookCategory>("SELECT * FROM bookcategory");
        }
    }
}

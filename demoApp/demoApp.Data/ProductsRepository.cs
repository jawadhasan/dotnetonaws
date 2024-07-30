using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace demoApp.Data
{
    public class ProductsRepository
    {
        private readonly IDbConnection _db;

        //ctor
        public ProductsRepository(IDbConnection db)
        {
            //_db = new NpgsqlConnection(connectionString);
            _db = db;
        }

        public List<dynamic> GetAll()
        {
            return _db.Query("SELECT * FROM products").ToList();
        }
    }
}

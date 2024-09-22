using Dapper;
using demoApp.Core;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace demoApp.Data
{
    public class UserRepository
    {
        private readonly IDbConnection _db;

        //ctor
        public UserRepository(IDbConnection db)
        {
            _db = db;
        }


        public List<User> GetAll()
        {
            return _db.Query<User>("SELECT * FROM users").ToList();
        }

        public List<User> GetUsersByName(string searchTerm)
        {
            var sql = "SELECT * FROM users WHERE (firstname || ' ' || lastname) ILIKE @NamePattern";

            return _db.Query<User>(sql,
                new
                {
                    NamePattern = $"{searchTerm}%" // Add % at the end for names starting with the search term
                }).ToList(); 
        }


        public User Insert(User newUser)
        {
            var sql = @"INSERT INTO users(email,firstname,lastname) 
                        VALUES(@email,@firstname, @lastname) RETURNING id;";//SELECT CAST(SCOPE_IDENTITY() as int) for SQL Server

            var id = _db.Query<int>(sql, newUser).Single();
            newUser.Id = id;
            return newUser;
        }

        public User GetById(int id)
        {
            var sql = @"SELECT * FROM users WHERE id = @id";

            return _db.Query<User>(sql, new { id }).SingleOrDefault();
        }

        public User Update(User user)
        {
            var sql = @"UPDATE users SET 
                        email = @Email, firstname = @FirstName, lastName = @LastName 
                        WHERE id = @Id";
            _db.Execute(sql, user);
            return user;
        }

        public void RemoveById(int id)
        {

            var sql = "DELETE FROM users WHERE id = @Id";
            _db.Execute(sql, new { id });
        }

        public List<Post> GetPosts()
        {
            return _db.Query<Post>("SELECT * FROM userposts").ToList();
        }

        public List<Post> GetPostsForUser(int userId)
        {
            var sql = "SELECT * FROM userposts WHERE userid = @userId";
            return _db.Query<Post>(sql, new { userId }).ToList();
        }
    }
}

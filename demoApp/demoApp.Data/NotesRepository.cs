using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using demoApp.Core;
using Npgsql;

namespace demoApp.Data
{
    public class NotesRepository
    {
        private readonly IDbConnection _db;

        //ctor
        public NotesRepository(string connectionString)
        {
            _db = new NpgsqlConnection(connectionString);
        }

        public List<Note> GetAll()
        {
            return _db.Query<Note>("SELECT * FROM notes").ToList();
        }

        public Note GetById(int id)
        {
            var sql = @"SELECT * FROM notes WHERE id = @id";

            return _db.Query<Note>(sql, new { id }).SingleOrDefault();
        }
        public Note Insert(Note newNote)
        {
            var sql = @"INSERT INTO notes(title,content,details, categoryid, userid) 
                        VALUES(@title,@content, @details, @categoryId, @userId) RETURNING id;";//SELECT CAST(SCOPE_IDENTITY() as int) for SQL Server

            var id = _db.Query<int>(sql, newNote).Single();
            newNote.Id = id;
            return newNote;
        }
        public Note Update(Note note)
        {
            var sql = @"UPDATE notes SET 
                        title = @Title, content = @Content, details = @Details , categoryid = @CategoryId, userid = @userId
                        WHERE id = @Id";
            _db.Execute(sql, note);
            return note;
        }

        public void RemoveById(int id)
        {

            var sql = "DELETE FROM notes WHERE id = @Id";
            _db.Execute(sql, new { id });
        }
    }
}

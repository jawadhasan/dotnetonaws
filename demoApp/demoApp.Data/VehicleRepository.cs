using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using demoApp.Core;
using Npgsql;

namespace demoApp.Data
{
    public class VehicleRepository
    {
        private readonly IDbConnection _db;

        public VehicleRepository(string connString)
        {
            _db = new NpgsqlConnection(connString);
        }

        public List<Vehicle> GetAll()
        {
            return _db.Query<Vehicle>("SELECT * FROM vehicle").ToList();
        }
        public Vehicle GetById(int id)
        {
            var sql = @"SELECT * FROM vehicle WHERE id = @id";

            return _db.Query<Vehicle>(sql, new { id }).SingleOrDefault();
        }
        public Vehicle Insert(Vehicle newVehicle)
        {
            var sql = @"INSERT INTO vehicle(licenseplate,temperauture,lat,lon) 
                        VALUES(@licensePlate,@temperature, @lat, @lon) RETURNING id;";//SELECT CAST(SCOPE_IDENTITY() as int) for SQL Serverlat
            var id = _db.Query<int>(sql, newVehicle).Single();
            newVehicle.Id = id;
            return newVehicle;
        }

        public void RemoveById(int id)
        {

            var sql = "DELETE FROM vehicle WHERE id = @Id";
            _db.Execute(sql, new { id });
        }
    }


    public class sampVehicleRepository
    {
        private readonly IDbConnection _connection;

        public sampVehicleRepository(IDbConnection dbConnection)
        {
            _connection = dbConnection;
        }
    }
}

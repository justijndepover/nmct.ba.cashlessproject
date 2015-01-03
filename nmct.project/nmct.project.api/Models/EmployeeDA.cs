using nmct.project.api.Helper;
using nmct.project.model;
using nmct.project.model.dbKlant;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace nmct.project.api.Models
{
    public class EmployeeDA
    {
        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "dblogin").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "dbpass").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "dbname").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", @"JUSTIJN\SQLEXPRESS", Cryptography.Decrypt(dbname), Cryptography.Decrypt(dblogin), Cryptography.Decrypt(dbpass));
        }

        public static List<Employee> GetEmployees(IEnumerable<Claim> claims)
        {
            List<Employee> list = new List<Employee>();
            string sql = "SELECT ID, EmployeeName, Address, Email, Phone, RijksID FROM Employee";
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);
            while (reader.Read())
            {
                list.Add(CreateEmployee(reader));
            }
            reader.Close();
            return list;
        }

        private static Employee CreateEmployee(IDataRecord record)
        {
            return new Employee()
            {
                ID = Int32.Parse(record["ID"].ToString()),
                EmployeeName = record["EmployeeName"].ToString(),
                Address = record["Address"].ToString(),
                Email = record["Email"].ToString(),
                Phone = record["Phone"].ToString(),
                RijksregisterNummer = long.Parse(record["RijksID"].ToString())
            };
        }
        public static void DeleteEmployee(int id, IEnumerable<Claim> claims)
        {
            int rowsAffected = 0;
            string sql = "DELETE FROM Employee WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("ConnectionString", "@ID", id);
            rowsAffected += Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1);
        }

        public static void EditEmployee(Employee e, IEnumerable<Claim> claims)
        {
            string sql = "UPDATE Employee SET EmployeeName = @employeeName, Address = @address, Email = @email, Phone = @phone, RijksID=@rijksid WHERE ID=@id";
            DbParameter par1 = Database.AddParameter("ConnectionString", "@id", e.ID);
            DbParameter par2 = Database.AddParameter("ConnectionString", "@employeeName", e.EmployeeName);
            DbParameter par3 = Database.AddParameter("ConnectionString", "@address", e.Address);
            DbParameter par4 = Database.AddParameter("ConnectionString", "@email", e.Email);
            DbParameter par5 = Database.AddParameter("ConnectionString", "@phone", e.Phone);
            DbParameter par6 = Database.AddParameter("ConnectionString", "@rijksid", e.RijksregisterNummer);
            Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4, par5, par6);
        }

        public static void SaveEmployee(Employee e, IEnumerable<Claim> claims)
        {
            string sql = "INSERT INTO Employee (EmployeeName, Address, Email, Phone, RijksID) VALUES (@employeeName, @address, @email, @phone, @rijksid)";
            DbParameter par1 = Database.AddParameter("ConnectionString", "@employeeName", e.EmployeeName);
            DbParameter par2 = Database.AddParameter("ConnectionString", "@address", e.Address);
            DbParameter par3 = Database.AddParameter("ConnectionString", "@email", e.Email);
            DbParameter par4 = Database.AddParameter("ConnectionString", "@phone", e.Phone);
            DbParameter par5 = Database.AddParameter("ConnectionString", "@rijksid", e.RijksregisterNummer);
            Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4, par5);
        }

        public static Employee GetEmployee(string nummer, IEnumerable<Claim> claims)
        {
            Employee e = new Employee();
            string sql = "SELECT * FROM Employee WHERE RijksID=@rijksid";
            DbParameter par1 = Database.AddParameter("ConnectionString", "@rijksid", nummer);
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, par1);
            if (reader.HasRows == false)
            {
                return null;
            }
            reader.Read();
            e = CreateEmployee(reader);
            reader.Close();
            return e;
        }
    }
}
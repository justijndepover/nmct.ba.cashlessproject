using nmct.project.api.Helper;
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
    public class CustomerDA
    {
        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "dblogin").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "dbpass").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "dbname").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", @"JUSTIJN\SQLEXPRESS", Cryptography.Decrypt(dbname), Cryptography.Decrypt(dblogin), Cryptography.Decrypt(dbpass));
        }

        public static Customer GetCustomer(string rijksregisternummer, IEnumerable<Claim> claims)
        {
            Customer c;
            string sql = "SELECT * FROM Customer WHERE rijksID = @rijksID";
            DbParameter par1 = Database.AddParameter("ConnectionString", "@rijksID", rijksregisternummer);
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, par1);
            if (reader.HasRows == false)
            {
                return null;
            }
            reader.Read();
            c = CreateCustomer(reader);
            reader.Close();
            return c;
        }

        private static Customer CreateCustomer(IDataRecord record)
        {
            byte[] Picture;
            if (!DBNull.Value.Equals(record["Picture"]))
                Picture = (byte[])record["Picture"];
            else
                Picture = new byte[0];
            return new Customer()
            {
                ID = Int32.Parse(record["ID"].ToString()),
                CustomerName = record["CustomerName"].ToString(),
                Address = record["Address"].ToString(),
                Picture = Picture,
                Balance = double.Parse(record["Balance"].ToString()),
                RijksregisterNummer = long.Parse(record["RijksID"].ToString())
            };
        }

        public static void SaveCustomer(Customer c, IEnumerable<Claim> claims)
        {
            string sql = "INSERT INTO Customer VALUES (@CustomerName, @Address, @Picture, @Balance, @RijksID)";
            DbParameter par1 = Database.AddParameter("ConnectionString", "@CustomerName", c.CustomerName);
            DbParameter par2 = Database.AddParameter("ConnectionString", "@Address", c.Address);
            DbParameter par3 = Database.AddParameter("ConnectionString", "@Picture", c.Picture);
            DbParameter par4 = Database.AddParameter("ConnectionString", "@Balance", c.Balance);
            DbParameter par5 = Database.AddParameter("ConnectionString", "@RijksID", c.RijksregisterNummer);

            Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4, par5);
        }

        public static List<Customer> GetCustomers(IEnumerable<Claim> claims)
        {
            List<Customer> list = new List<Customer>();
            string sql = "SELECT * FROM Customer";
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);
            while (reader.Read())
            {
                list.Add(CreateCustomer(reader));
            }
            reader.Close();
            return list;
        }

        public static int LowerBalance(double d, long rijksid, IEnumerable<Claim> claims)
        {
            int rowsAffected = 0;
            string sql = "UPDATE Customer SET Balance=@balance WHERE rijksID=@rijksid";
            DbParameter par1 = Database.AddParameter("ConnectionString", "@balance", d);
            DbParameter par2 = Database.AddParameter("ConnectionString", "@rijksid", rijksid);
            rowsAffected = Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2);
            return rowsAffected;
        }

    }
}
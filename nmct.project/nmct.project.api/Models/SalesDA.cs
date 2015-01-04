using nmct.project.api.Helper;
using nmct.project.model.dbKlant;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace nmct.project.api.Models
{
    public class SalesDA
    {
        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "dblogin").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "dbpass").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "dbname").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", @"JUSTIJN\SQLEXPRESS", Cryptography.Decrypt(dbname), Cryptography.Decrypt(dblogin), Cryptography.Decrypt(dbpass));
        }

        public static int SaveSales(ProductList pl, IEnumerable<Claim> claims)
        {
            int rowsAffected = 0;
            string sql = "INSERT INTO Sale VALUES (@timestamp, @customerid, @registerid, @productid, 1, 1)";
            DbParameter par1 = Database.AddParameter("ConnectionString", "@timestamp", pl.TimeStamp);
            DbParameter par2 = Database.AddParameter("ConnectionString", "@customerid", pl.CustomerID);
            DbParameter par3 = Database.AddParameter("ConnectionString", "@registerid", pl.RegisterID);
            foreach (Products p in pl.Products)
            {
                DbParameter par4 = Database.AddParameter("ConnectionString", "productid", p.ID);
                rowsAffected = Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4);
            }
            return rowsAffected;
        }
    }
}
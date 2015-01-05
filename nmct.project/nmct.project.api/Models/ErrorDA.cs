using nmct.project.api.Helper;
using nmct.project.model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace nmct.project.api.Models
{
    public class ErrorDA
    {
        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "dblogin").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "dbpass").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "dbname").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", @"JUSTIJN\SQLEXPRESS", Cryptography.Decrypt(dbname), Cryptography.Decrypt(dblogin), Cryptography.Decrypt(dbpass));
        }

        public static int CreateErrorlog(Errorlog e, IEnumerable<Claim> claims)
        {
            string sql = "INSERT INTO Errorlog (RegisterID, Timestamp, Message, Stacktrace) VALUES (@RegisterID, @Timestamp, @Message, @Stacktrace)";
            DbParameter par1 = Database.AddParameter("ConnectionString", "@RegisterID", e.RegisterID);
            DbParameter par2 = Database.AddParameter("ConnectionString", "@Timestamp", e.Timestamp);
            DbParameter par3 = Database.AddParameter("ConnectionString", "@Message", e.Message);
            DbParameter par4 = Database.AddParameter("ConnectionString", "@Stacktrace", e.Stacktrace);
            int rows = Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4);
            return rows;
        }
    }
}
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
    public class RegisterDA
    {
        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "dblogin").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "dbpass").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "dbname").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", @"JUSTIJN\SQLEXPRESS", Cryptography.Decrypt(dbname), Cryptography.Decrypt(dblogin), Cryptography.Decrypt(dbpass));
        }

        public static List<Registers> GetRegisters(IEnumerable<Claim> claims)
        {
            List<Registers> list = new List<Registers>();
            string sql = "SELECT ID, RegisterName, Device FROM Register";
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);
            while (reader.Read())
            {
                list.Add(CreateRegister(reader));
            }
            reader.Close();
            return list;
        }

        private static Registers CreateRegister(IDataRecord record)
        {
            return new Registers()
            {
                ID = Int32.Parse(record["ID"].ToString()),
                RegisterName = record["RegisterName"].ToString(),
                Device = record["Device"].ToString()
            };
        }

        public static List<RegisterEmployee> GetEmployeesOnRegister(int registerID, IEnumerable<Claim> claims)
        {
            List<RegisterEmployee> list = new List<RegisterEmployee>();
            string sql = "SELECT EmployeeName, FromTime, UntilTime FROM Register_Employee INNER JOIN Employee ON Register_Employee.EmployeeID = Employee.ID WHERE RegisterID=@registerid";
            DbParameter par1 = Database.AddParameter("ConnectionString", "@registerid", registerID);
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, par1);
            while (reader.Read())
            {
                list.Add(CreateRegisterEmployee(reader));
            }
            reader.Close();
            return list;
        }

        private static RegisterEmployee CreateRegisterEmployee(IDataRecord record)
        {
            return new RegisterEmployee()
            {
                EmployeeName = record["EmployeeName"].ToString(),
                From = DateTime.Parse(record["FromTime"].ToString()),
                Until = DateTime.Parse(record["UntilTime"].ToString())
            };
        }
    }
}
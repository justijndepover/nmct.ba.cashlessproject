using nmct.project.api.Helper;
using nmct.project.model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace nmct.project.api.Models
{
    public class OrganisationsDA
    {
        public static Organisations CheckCredentials(string username, string password)
        {
            string sql = "SELECT * FROM Organisations WHERE Login=@Login AND Password=@Password";
            DbParameter par1 = Database.AddParameter("ConnectionString", "@Login", /*Cryptography.Encrypt(*/username);
            DbParameter par2 = Database.AddParameter("ConnectionString", "@Password", /*Cryptography.Encrypt(*/password);
            try
            {
                DbDataReader reader = Database.GetData(Database.GetConnection("ConnectionString"), sql, par1, par2);
                reader.Read();
                return new Organisations()
                {
                    ID = Int32.Parse(reader["ID"].ToString()),
                    Login = reader["Login"].ToString(),
                    Password = reader["Password"].ToString(),
                    DbName = reader["DbName"].ToString(),
                    DbLogin = reader["DbLogin"].ToString(),
                    DbPassword = reader["DbPassword"].ToString(),
                    OrganisationName = reader["OrganisationName"].ToString(),
                    Address = reader["Address"].ToString(),
                    Email = reader["Email"].ToString(),
                    Phone = reader["Phone"].ToString()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }
    }
}
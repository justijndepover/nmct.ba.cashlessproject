using nmct.project.model;
using nmct.project.web.itbedrijf.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace nmct.project.web.itbedrijf.Models
{
    public class OrganisationDA
    {
        public static List<Organisations> GetOrganisations()
        {
            List<Organisations> list = new List<Organisations>();
            string sql = "SELECT ID, Login, Password, DbName, DbLogin, DbPassword, OrganisationName, Address, Email, Phone FROM Organisations";
            DbDataReader reader = Database.GetData(Database.GetConnection("ConnectionString"), sql);
            while (reader.Read())
            {
                list.Add(Create(reader));
            }
            reader.Close();
            return list;
        }

        public static Organisations GetOrganisation(int id)
        {
            Organisations organisation = new Organisations();
            string sql = "SELECT ID, Login, Password, DbName, DbLogin, DbPassword, OrganisationName, Address, Email, Phone FROM Organisations WHERE ID = @ID";
            DbParameter par1 = Database.AddParameter("ConnectionString", "@ID", id);

            DbDataReader reader = Database.GetData(Database.GetConnection("ConnectionString"), sql, par1);
            while (reader.Read())
            {
                organisation = Create(reader);
            }
            reader.Close();
            return organisation;
        }
        public static int InsertOrganisations(Organisations organisation)
        {
            string sql = "INSERT INTO Organisations (Login, Password, DbName, DbLogin, DbPassword, OrganisationName, Address, Email, Phone) VALUES(@Login, @Password, @DbName, @DbLogin, @DbPassword, @OrganisationName, @Address, @Email, @Phone)";

            DbParameter par1 = Database.AddParameter("ConnectionString", "@Login", Cryptography.Encrypt(organisation.Login));
            DbParameter par2 = Database.AddParameter("ConnectionString", "@Password", Cryptography.Encrypt(organisation.Password));
            DbParameter par3 = Database.AddParameter("ConnectionString", "@DbName", Cryptography.Encrypt(organisation.DbName));
            DbParameter par4 = Database.AddParameter("ConnectionString", "@DbLogin", Cryptography.Encrypt(organisation.DbLogin));
            DbParameter par5 = Database.AddParameter("ConnectionString", "@DbPassword", Cryptography.Encrypt(organisation.DbPassword));
            DbParameter par6 = Database.AddParameter("ConnectionString", "@OrganisationName", organisation.OrganisationName);
            DbParameter par7 = Database.AddParameter("ConnectionString", "@Address", organisation.Address);
            DbParameter par8 = Database.AddParameter("ConnectionString", "@Email", organisation.Email);
            DbParameter par9 = Database.AddParameter("ConnectionString", "@Phone", organisation.Phone);
            int id = Database.InsertData(Database.GetConnection("ConnectionString"), sql, par1, par2, par3, par4, par5, par6, par7, par8, par9);

            CreateDatabase(organisation);
            return id;

        }
        public static int UpdateOrganisations(Organisations organisation)
        {
            string sql = "UPDATE Organisations SET Login = @Login, Password = @Password, DbName = @DbName, DbLogin = @DbLogin, DbPassword = @DbPassword, OrganisationName = @OrganisationName, Address = @Address, Email = @Email, Phone = @Phone WHERE ID=@ID";

            DbParameter par1 = Database.AddParameter("ConnectionString", "@Login", Cryptography.Encrypt(organisation.Login));
            DbParameter par2 = Database.AddParameter("ConnectionString", "@Password", Cryptography.Encrypt(organisation.Password));
            DbParameter par3 = Database.AddParameter("ConnectionString", "@DbName", Cryptography.Encrypt(organisation.DbName));
            DbParameter par4 = Database.AddParameter("ConnectionString", "@DbLogin", Cryptography.Encrypt(organisation.DbLogin));
            DbParameter par5 = Database.AddParameter("ConnectionString", "@DbPassword", Cryptography.Encrypt(organisation.DbPassword));
            DbParameter par6 = Database.AddParameter("ConnectionString", "@OrganisationName", organisation.Address);
            DbParameter par7 = Database.AddParameter("ConnectionString", "@Address", organisation.Address);
            DbParameter par8 = Database.AddParameter("ConnectionString", "@Email", organisation.Email);
            DbParameter par9 = Database.AddParameter("ConnectionString", "@Phone", organisation.Phone);
            DbParameter par10 = Database.AddParameter("ConnectionString", "@ID", organisation.ID);

            return Database.ModifyData(Database.GetConnection("ConnectionString"), sql, par1, par2, par3, par4, par5, par6, par7, par8, par9, par10);
        }

        private static Organisations Create(IDataRecord record)
        {
            return new Organisations()
            {
                ID = Int32.Parse(record["ID"].ToString()),
                Login = Cryptography.Decrypt(record["Login"].ToString()),
                Password = Cryptography.Decrypt(record["Password"].ToString()),
                DbName = Cryptography.Decrypt(record["DbName"].ToString()),
                DbLogin = Cryptography.Decrypt(record["DbLogin"].ToString()),
                DbPassword = Cryptography.Decrypt(record["DbPassword"].ToString()),
                OrganisationName = record["OrganisationName"].ToString(),
                Address = record["Address"].ToString(),
                Email = record["Email"].ToString(),
                Phone = record["Phone"].ToString()
            };
        }

        private static void CreateDatabase(Organisations o)
        {
            // create the actual database
            string create = File.ReadAllText(HostingEnvironment.MapPath(@"~/App_Data/create.txt")); //only for the web
            //string create = File.ReadAllText(@"..\..\Data\create.txt"); // only for desktop
            string sql = create.Replace("@@DbName", o.DbName).Replace("@@DbLogin", o.DbLogin).Replace("@@DbPassword", o.DbPassword);
            foreach (string commandText in RemoveGo(sql))
            {
                Database.ModifyData(Database.GetConnection("ConnectionString"), commandText);
            }

            // create login, user and tables
            DbTransaction trans = null;
            try
            {
                trans = Database.BeginTransaction("ConnectionString");

                string fill = File.ReadAllText(HostingEnvironment.MapPath(@"~/App_Data/fill.txt")); // only for the web
                //string fill = File.ReadAllText(@"..\..\Data\fill.txt"); // only for desktop
                string sql2 = fill.Replace("@@DbName", o.DbName).Replace("@@DbLogin", o.DbLogin).Replace("@@DbPassword", o.DbPassword);

                foreach (string commandText in RemoveGo(sql2))
                {
                    Database.ModifyData(trans, commandText);
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                //als er hier een fout is moet je het pad van SQL checken in de create file
                trans.Rollback();
                Console.WriteLine(ex.Message);
            }
        }

        private static string[] RemoveGo(string input)
        {
            //split the script on "GO" commands
            string[] splitter = new string[] { "\r\nGO\r\n" };
            string[] commandTexts = input.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
            return commandTexts;
        }
    }
}
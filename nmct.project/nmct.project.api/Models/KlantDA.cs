using nmct.project.api.Helper;
using nmct.project.model.dbKlant;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web;

namespace nmct.project.api.Models
{
    public class KlantDA
    {
        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "dblogin").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "dbpass").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "dbname").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", @"JUSTIJN\SQLEXPRESS", /*Cryptography.Decrypt(*/dbname, /*Cryptography.Decrypt(*/dblogin, /*Cryptography.Decrypt(*/dbpass);
        }

        #region Products
        public static List<Products> GetProducts(IEnumerable<Claim> claims)
        {
            List<Products> list = new List<Products>();
            string sql = "SELECT ID, ProductName, Price FROM Product";
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);
            while (reader.Read())
            {
                list.Add(CreateProduct(reader));
            }
            reader.Close();
            return list;
        }

        private static Products CreateProduct(IDataRecord record)
        {
            return new Products(){
                ID = Int32.Parse(record["ID"].ToString()),
                ProductName = record["ProductName"].ToString(),
                Price = Double.Parse(record["Price"].ToString())
            };
        }

        public static void DeleteProduct(int id, IEnumerable<Claim> claims)
        {
            int rowsAffected = 0;
            string sql = "DELETE FROM Product WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("ConnectionString", "@ID", id);
            rowsAffected += Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1);
        }

        public static void EditProduct(Products p, IEnumerable<Claim> claims)
        {
            string sql = "UPDATE Product SET ProductName = @productname, Price = @price WHERE ID=@id";
            DbParameter par1 = Database.AddParameter("ConnectionString", "@id", p.ID);
            DbParameter par2 = Database.AddParameter("ConnectionString", "@productname", p.ProductName);
            DbParameter par3 = Database.AddParameter("ConnectionString", "@price", p.Price);
            Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3);
        }
        #endregion

        #region customers

        #endregion
    }
}
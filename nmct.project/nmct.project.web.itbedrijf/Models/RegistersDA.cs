using nmct.project.model;
using nmct.project.web.itbedrijf.Helper;
using nmct.project.web.itbedrijf.Presentation_Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace nmct.project.web.itbedrijf.Models
{
    public class RegistersDA
    {
        public const string CON = "ConnectionString";
        public static List<Registers> GetRegisters()
        {
            List<Registers> list = new List<Registers>();
            string sql = "SELECT ID, RegisterName, Device, PurchaseDate, ExpiresDate FROM Registers";
            DbDataReader reader = Database.GetData(Database.GetConnection(CON), sql);
            while (reader.Read())
            {
                list.Add(Create(reader));
            }
            reader.Close();
            return list;
        }

        public static List<RegisterPM> GetAllRegisters()
        {
            List<RegisterPM> list = new List<RegisterPM>();
            string sql = "SELECT ID, RegisterName, Device, PurchaseDate, ExpiresDate, OrganisationID FROM Registers left join Organisation_Register on ID=RegisterID";
            DbDataReader reader = Database.GetData(Database.GetConnection(CON), sql);
            while (reader.Read())
            {
                list.Add(CreatePM(reader));
            }
            reader.Close();
            return list;
        }

        public static RegisterPM CreatePM(IDataRecord record)
        {
            int organisationID;
            Int32.TryParse(record["OrganisationID"].ToString(), out organisationID);

            Registers r = new Registers()
            {
                ID = Int32.Parse(record["ID"].ToString()),
                RegisterName = record["RegisterName"].ToString(),
                Device = record["Device"].ToString(),
                PurchaseDate = DateTime.Parse(record["PurchaseDate"].ToString()),
                ExpiresDate = DateTime.Parse(record["ExpiresDate"].ToString())
            };
            Organisations o;
            if (organisationID > 0)
            {
                o = OrganisationDA.GetOrganisation(organisationID);
            }
            else
            {
                o = null;
            }
            
            return new RegisterPM()
            {
                Kassa = r,
                Vereniging = o
            };
        }

        public static Registers Create(IDataRecord record)
        {
            return new Registers()
            {
                ID = Int32.Parse(record["ID"].ToString()),
                RegisterName = record["RegisterName"].ToString(),
                Device = record["Device"].ToString(),
                PurchaseDate = DateTime.Parse(record["PurchaseDate"].ToString()),
                ExpiresDate = DateTime.Parse(record["ExpiresDate"].ToString())
            };
        }

        public static Registers GetRegister(int id)
        {
            Registers r = new Registers();
            string sql = "SELECT ID, RegisterName, Device, PurchaseDate, ExpiresDate FROM Registers WHERE ID = @ID";
            DbParameter par1 = Database.AddParameter(CON, "@ID", id);

            DbDataReader reader = Database.GetData(Database.GetConnection(CON), sql, par1);
            while (reader.Read())
            {
                r = Create(reader);
            }
            reader.Close();
            return r;
        }

        public static int InsertRegisters(Registers r)
        {
            string sql = "INSERT INTO Registers (RegisterName, Device, PurchaseDate, ExpiresDate) VALUES(@RegisterName, @Device, @PurchaseDate, @ExpiresDate)";

            DbParameter par1 = Database.AddParameter(CON, "@RegisterName", r.RegisterName);
            DbParameter par2 = Database.AddParameter(CON, "@Device", r.Device);
            DbParameter par3 = Database.AddParameter(CON, "@PurchaseDate", r.PurchaseDate);
            DbParameter par4 = Database.AddParameter(CON, "@ExpiresDate", r.ExpiresDate);
            return Database.InsertData(Database.GetConnection(CON), sql, par1, par2, par3, par4);
        }

        public static int UpdateRegisters(Registers r)
        {
            string sql = "UPDATE Registers SET RegisterName = @RegisterName, Device = @Device, PurchaseDate = @PurchaseDate, ExpiresDate = @ExpiresDate WHERE ID=@ID";

            DbParameter par1 = Database.AddParameter(CON, "@RegisterName", r.RegisterName);
            DbParameter par2 = Database.AddParameter(CON, "@Device", r.Device);
            DbParameter par3 = Database.AddParameter(CON, "@PurchaseDate", r.PurchaseDate);
            DbParameter par4 = Database.AddParameter(CON, "@ExpiresDate", r.ExpiresDate);
            DbParameter par5 = Database.AddParameter(CON, "@ID", r.ID);

            return Database.ModifyData(Database.GetConnection(CON), sql, par1, par2, par3, par4, par5);
        }
    }
}
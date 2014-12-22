using nmct.project.model;
using nmct.project.web.itbedrijf.Helper;
using nmct.project.web.itbedrijf.Presentation_Models;
using System;
using System.Collections.Generic;
using System.Configuration;
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

        public static List<RegisterPM> GetRegisterPM()
        {
            List<RegisterPM> list = new List<RegisterPM>();
            string sql = "SELECT Registers.ID, RegisterName, Device, PurchaseDate, ExpiresDate, OrganisationID FROM Registers left join Organisation_Register on Registers.ID=RegisterID";
            DbDataReader reader = Database.GetData(Database.GetConnection(CON), sql);
            while (reader.Read())
            {
                list.Add(CreatePM(reader));
            }
            reader.Close();
            return list;
        }

        public static List<RegisterPM> GetRegistersPM(int organisation)
        {
            List<RegisterPM> list = new List<RegisterPM>();
            string sql = "SELECT Registers.ID, RegisterName, Device, PurchaseDate, ExpiresDate, OrganisationID FROM Registers left join Organisation_Register on Registers.ID=RegisterID WHERE OrganisationID = @organisationID";
            DbParameter par1 = Database.AddParameter(CON, "@OrganisationID", organisation);
            DbDataReader reader = Database.GetData(Database.GetConnection(CON), sql, par1);
            while (reader.Read())
            {
                list.Add(CreatePM(reader));
            }
            reader.Close();
            return list;
        }

        public static RegisterPM GetRegisterPM(int id)
        {
            RegisterPM r = new RegisterPM();
            string sql = "SELECT Registers.ID, RegisterName, Device, PurchaseDate, ExpiresDate, OrganisationID FROM Registers left join Organisation_Register on Registers.ID=RegisterID WHERE Registers.ID=@id";
            DbParameter par1 = Database.AddParameter(CON, "@id", id);
            DbDataReader reader = Database.GetData(Database.GetConnection(CON), sql,par1);
            reader.Read();
            r = CreatePM(reader);
            reader.Close();
            return r;
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

        public static int InsertRegisterPM(int register, int organisation)
        {
            string sql;
            RegisterPM r = GetRegisterPM(register);
            DbParameter par1 = Database.AddParameter(CON, "@RegisterID", register);
            DbParameter par2 = Database.AddParameter(CON, "@OrganisationID", organisation);
            DbParameter par3 = Database.AddParameter(CON, "@FromDate", DateTime.Now);
            DbParameter par4 = Database.AddParameter(CON, "@UntilDate", r.Kassa.ExpiresDate);
            int ValidUpdate;

            if (r.Vereniging == null)
            {
                sql = "INSERT INTO Organisation_Register (OrganisationID, RegisterID, FromDate, UntilDate) VALUES (@OrganisationID, @RegisterID, @FromDate, @UntilDate)";
                ValidUpdate = Database.InsertData(Database.GetConnection(CON), sql, par1, par2, par3, par4);
            }
            else
            {
                sql = "UPDATE Organisation_Register SET OrganisationID=@OrganisationID, FromDate=@FromDate, UntilDate=@UntilDate WHERE RegisterID=@RegisterID";
                ValidUpdate = Database.ModifyData(Database.GetConnection(CON), sql, par1, par2, par3, par4);
            }

            return ValidUpdate;
        }

        public static int UpdateOrganisationDatabase(int rID, int oID, int old_oID)
        {
            Registers r = GetRegister(rID);
            Organisations o = OrganisationDA.GetOrganisation(oID);
            Organisations o2 = OrganisationDA.GetOrganisation(old_oID);
            ConnectionStringSettings connectionstring = Database.CreateConnectionString("System.Data.SqlClient", "JUSTIJN\\SQLEXPRESS", o.DbName, o.DbLogin, o.DbPassword);
            string sql = "INSERT INTO Register VALUES (@RegisterName, @Device)";
            DbParameter par1 = Database.AddParameter(CON, "@RegisterName", r.RegisterName);
            DbParameter par2 = Database.AddParameter(CON, "@Device", r.Device);
            int ValidUpdate = Database.InsertData(Database.GetConnection(connectionstring), sql, par1, par2);
            int ValidUpdate2;
            if (old_oID > 0)
            {
                ConnectionStringSettings connectionstring2 = Database.CreateConnectionString("System.Data.SqlClient", "JUSTIJN\\SQLEXPRESS", o2.DbName, o2.DbLogin, o2.DbPassword);
                string sql2 = "DELETE FROM Register WHERE RegisterName=@RegisterName";
                DbParameter par3 = Database.AddParameter(CON, "@RegisterName", r.RegisterName);
                ValidUpdate2 = Database.ModifyData(Database.GetConnection(connectionstring2), sql2, par3);
            }
            else
            {
                ValidUpdate2 = 1;
            }
            

            if (ValidUpdate > 0 & ValidUpdate2 > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
           
        }
    }
}
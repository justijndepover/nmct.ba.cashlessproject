using nmct.project.model;
using nmct.project.web.itbedrijf.Helper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace nmct.project.web.itbedrijf.Models
{
    public class ErrorDA
    {
        public static List<Errorlog> GetErrorLogs()
        {
            List<Errorlog> list = new List<Errorlog>();
            string sql = "SELECT RegisterID, Timestamp, Message, Stacktrace FROM Errorlog";
            DbDataReader reader = Database.GetData(Database.GetConnection("ConnectionString"), sql);

            while (reader.Read())
            {
                Errorlog error = new Errorlog()
                {
                    RegisterID = Int32.Parse(reader["RegisterID"].ToString()),
                    Timestamp = DateTime.Parse(reader["Timestamp"].ToString()),
                    Message = reader["Message"].ToString(),
                    Stacktrace = reader["Stacktrace"].ToString()
                };
                list.Add(error);
            }
            reader.Close();
            return list;
        }
    }
}
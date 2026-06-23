using CreditCard_DataAccessLayer.Database;
using CreditCard_DataAccessLayer.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_DataAccessLayer.Repository
{
    public static class LogsRepository
    {

        public  static List<md_Logs> GetAllLogs()
        {
            List<md_Logs> logs = new List<md_Logs>();
            string sql = "SELECT * from FUN_Log_GetAllLogs()";
            try
            {
                using (SqlConnection con = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                          con.Open();
                        using (SqlDataReader reader =   cmd.ExecuteReader())
                        {
                            while (  reader.Read())
                            {
                                md_Logs log = new md_Logs(
                                    LogID: reader.GetInt32(reader.GetOrdinal("LogID")),
                                    ViewerID: reader.GetInt32(reader.GetOrdinal("ViewerID")),
                                    RecordID: reader.GetString(reader.GetOrdinal("RecordID")),
                                    SeverityLevel: reader.GetString(reader.GetOrdinal("SeverityLevel")),
                                    EventType: reader.GetString(reader.GetOrdinal("EventType")),
                                    TableName: reader.GetString(reader.GetOrdinal("TableName")),
                                    Description: reader.GetString(reader.GetOrdinal("Description")),
                                    logDate: reader.GetDateTime(reader.GetOrdinal("logDate"))
                                );
                                logs.Add(log);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return logs;
        }

       

    }
}

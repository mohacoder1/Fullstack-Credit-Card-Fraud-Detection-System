using CreditCard_DataAccessLayer.Database;
using CreditCard_DataAccessLayer.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_DataAccessLayer.Repository
{
    public static  class ReportsRepository
    {
        
       
        public static md_Report? GetSummeryReport()
        {
           
           string sql = "SELECT * FROM FUN_Report_GetFraudDetectionReport()";
               

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                   var report =new md_Report
                                    (
                                    TotalTransactions: reader.GetInt32(reader.GetOrdinal("TotalTransactions")),
                                    TodaysTransactions: reader.GetInt32(reader.GetOrdinal("TodaysTransactions")),
                                    Legitimate: reader.GetInt32(reader.GetOrdinal("Legitimate")),
                                    FraudDetected: reader.GetInt32(reader.GetOrdinal("FraudDetected")),
                                    FraudRate: reader.GetDouble(reader.GetOrdinal("FraudRate")),
                                    HighRiskAlerts: reader.GetInt32(reader.GetOrdinal("HighRiskAlerts")),
                                    PendingReviews: reader.GetInt32(reader.GetOrdinal("PendingReviews")),
                                    CrossBorderSuspicious: reader.GetInt32(reader.GetOrdinal("CrossBorderSuspicious"))
                                    );
                              return report;
                            }
                        }
                        //connection.Close();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return null;
        }
        public static md_Report? GetSummeryReportByID(int ID)
        {
           
           string sql = "SELECT * FROM FUN_Report_GetFraudDetectionReportByID(@ID)";
               

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                   var report =new md_Report
                                    (
                                    TotalTransactions: reader.GetInt32(reader.GetOrdinal("TotalTransactions")),
                                    TodaysTransactions: reader.GetInt32(reader.GetOrdinal("TodaysTransactions")),
                                    Legitimate: reader.GetInt32(reader.GetOrdinal("Legitimate")),
                                    FraudDetected: reader.GetInt32(reader.GetOrdinal("FraudDetected")),
                                    FraudRate: reader.GetDouble(reader.GetOrdinal("FraudRate")),
                                    HighRiskAlerts: reader.GetInt32(reader.GetOrdinal("HighRiskAlerts")),
                                    PendingReviews: reader.GetInt32(reader.GetOrdinal("PendingReviews")),
                                    CrossBorderSuspicious: reader.GetInt32(reader.GetOrdinal("CrossBorderSuspicious"))
                                    );
                              return report;
                            }
                        }
                        //connection.Close();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return null;
        }
    }
}

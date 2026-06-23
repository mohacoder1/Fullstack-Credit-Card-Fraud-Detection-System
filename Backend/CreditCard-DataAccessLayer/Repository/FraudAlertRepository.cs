using CreditCard_DataAccessLayer.Database;
using CreditCard_DataAccessLayer.DTO;
using CreditCard_DataAccessLayer.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_DataAccessLayer.Repository
{
    public  static class FraudAlertRepository
    {
    
        public static  bool setAlertAcknowledge(int AlertID ,int ViewerID )
        {
            string sql = "[dbo].[SP_FraudAlerts_Acknowledge]";
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@AlertID", AlertID);
                        cmd.Parameters.AddWithValue("@ViewerID", ViewerID);

                        connection.Open();
                        
                        var rowEffected =  cmd.ExecuteScalar();
                        if (rowEffected != null && int.TryParse(rowEffected.ToString(), out int result))
                            return result > 0;

                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
                return false;
            // Return a random ID for demonstration
        }
        public static  bool ResolveFraudAlert(DTO_ResolveAlert alert)
        {
            string sql = "[dbo].[SP_FraudAlerts_Resolve]";
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", alert.Action);
                        cmd.Parameters.AddWithValue("@AlertID", alert.AlertID);
                        cmd.Parameters.AddWithValue("@ResolveStatus", alert.ResolveStatus);
                        cmd.Parameters.AddWithValue("@Comment", alert.Comment);
                        cmd.Parameters.AddWithValue("@viewerID", alert.viewerID);

                        connection.Open();
                        
                        var rowEffected =  cmd.ExecuteNonQuery();
                          connection.Close();
                        return rowEffected>0;

                    }

                }
            }
            catch (Exception)
            {

                return false;
            }
            // Return a random ID for demonstration
        }
        public static  DTO_DetailedAlert GetDetailedAlert(int AlertID)
        {
            string sql = "select * from FUN_FraudAlerts_GetDetailedView(@AlertID)";
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@AlertID", AlertID);
                          connection.Open();
                        using (SqlDataReader reader =   cmd.ExecuteReader())
                        {
                            if (  reader.Read())
                            {
                                var alert = new DTO_DetailedAlert
                                {
                                    //Alert
                                    AlertID = reader.GetInt32(reader.GetOrdinal("AlertID")),
                                    AlertLevel = reader.GetString(reader.GetOrdinal("AlertLevel")),
                                    AlertStatus = reader.GetString(reader.GetOrdinal("AlertStatus")),
                                    AlertTime = reader.GetDateTime(reader.GetOrdinal("AlertTime")),
                                    RiskScore = Convert.ToDouble(reader.GetDecimal(reader.GetOrdinal("RiskScore"))),

                                    TX_Number = reader.GetInt32(reader.GetOrdinal("TX_Number")),
                                    Currency = reader.GetString(reader.GetOrdinal("Currency")),
                                    MerchantCategory = reader.GetString(reader.GetOrdinal("MerchantCategory")),
                                    TransactionReason = reader.GetString(reader.GetOrdinal("TransactionReason")),
                                    TransactionType = reader.GetString(reader.GetOrdinal("TransactionType")),
                                    Amount = Convert.ToDouble(reader.GetDecimal(reader.GetOrdinal("Amount"))),

                                    FullName = reader.GetString(reader.GetOrdinal("FullName")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                    ResidentCountry = reader.GetString(reader.GetOrdinal("ResidentCountry")),
                                    CardNumber = reader.GetString(reader.GetOrdinal("CardNumber")),
                                    CardStatus = reader.GetString(reader.GetOrdinal("CardStatus")),
                                    CurrentBalance = Convert.ToDouble(reader.GetDecimal(reader.GetOrdinal("CurrentBalance"))),

                                    SystemRecommendation = reader.GetString(reader.GetOrdinal("SystemRecommendation"))
                                };






                                return alert;
                            }
                        }
                          connection.Close();
                        }
                    }
                
            }
            catch (Exception ex)
            {
                throw(new Exception(ex.Message, ex));
            }
            return null;
        }
        public static  List<md_FraudAlert> GetAllFraudAlerts()
        {
            string sql = "select * from FUN_FraudAlerts_GetAll() order by TX_Time desc";
            List<md_FraudAlert> fraudAlerts = new List<md_FraudAlert>();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        
                          connection.Open();
                        using (SqlDataReader reader =   cmd.ExecuteReader())
                        {
                            while (  reader.Read())
                            {
                                var alert = new md_FraudAlert
                                (
                                    AlertID:reader.GetInt32(reader.GetOrdinal("AlertID")),
                                    AlertLevel: reader.GetString(reader.GetOrdinal("AlertLevel")), 
                                    AlertStatus: reader.GetString(reader.GetOrdinal("AlertStatus")),
                                    Title: reader.GetString(reader.GetOrdinal("Title")),
                                    SubTitle: reader.GetString(reader.GetOrdinal("SubTitle")),
                                    Currency: reader.GetString(reader.GetOrdinal("Currency")),
                                    TX_Number: reader.GetInt32(reader.GetOrdinal("TX_Number")),
                                    TX_Time: reader.GetDateTime(reader.GetOrdinal("TX_Time")),                     
                                    Amount:Convert.ToDouble(reader.GetDecimal(reader.GetOrdinal("Amount"))),
                                    RiskScore:Convert.ToDouble(reader.GetDecimal(reader.GetOrdinal("RiskScore"))) );
                                    fraudAlerts.Add(alert);
                            }
                        }
                          connection.Close();
                    }
                }
            }
            catch (Exception)
            {
                return fraudAlerts;
            }
            return fraudAlerts;
        }
        public static  List<md_FraudAlert> GetAlertsByCardholderID(int CardholderID)
        {
            string sql = "SELECT * FROM FUN_FraudAlerts_GetAlertsByCardholderID(@CardholderID)";
            List<md_FraudAlert> fraudAlerts = new List<md_FraudAlert>();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@CardholderID", CardholderID);

                        connection.Open();
                        using (SqlDataReader reader =   cmd.ExecuteReader())
                        {
                            while (  reader.Read())
                            {
                                var alert = new md_FraudAlert
                                (
                                    AlertID:reader.GetInt32(reader.GetOrdinal("AlertID")),
                                    AlertLevel: reader.GetString(reader.GetOrdinal("AlertLevel")), 
                                    AlertStatus: reader.GetString(reader.GetOrdinal("AlertStatus")),
                                    Title: reader.GetString(reader.GetOrdinal("Title")),
                                    SubTitle: reader.GetString(reader.GetOrdinal("SubTitle")),
                                    Currency: reader.GetString(reader.GetOrdinal("Currency")),
                                    TX_Number: reader.GetInt32(reader.GetOrdinal("TX_Number")),
                                    TX_Time: reader.GetDateTime(reader.GetOrdinal("TX_Time")),                     
                                    Amount:Convert.ToDouble(reader.GetDecimal(reader.GetOrdinal("Amount"))),
                                    RiskScore:Convert.ToDouble(reader.GetDecimal(reader.GetOrdinal("RiskScore"))) );
                                    fraudAlerts.Add(alert);
                            }
                        }
                          connection.Close();
                    }
                }
            }
            catch (Exception)
            {
                return fraudAlerts;
            }
            return fraudAlerts;
        }
        
    }
}

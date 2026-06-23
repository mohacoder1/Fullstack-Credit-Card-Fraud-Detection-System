using CreditCard_DataAccessLayer.Database;
using CreditCard_DataAccessLayer.DTO.Transactions;
using CreditCard_DataAccessLayer.Models.Transactions;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CreditCard_DataAccessLayer.Repository
{
    public class TransactionsRepository
    {
      
        public async Task<DTO_ExecuteResult> ExecuteTransaction(DTO_AddNewTransaction transaction)

        {

            string sql = "[dbo].[SP_Transactions_Execute]";
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@CardID", transaction.CardID);
                        //cmd.Parameters.AddWithValue("@Amount", transaction.Amount);
                        //cmd.Parameters.AddWithValue("@ExternalFraudScore", transaction.ExternalFraudScore);
                        cmd.Parameters.AddWithValue("@Currency", transaction.Currency);
                        cmd.Parameters.AddWithValue("@Country", transaction.Country);
                        cmd.Parameters.AddWithValue("@DeviceType", transaction.DeviceType);
                        cmd.Parameters.AddWithValue("@MerchantCategory", transaction.MerchantCategory);
                        cmd.Parameters.AddWithValue("@TransactionType", transaction.TransactionType);
                        cmd.Parameters.AddWithValue("@Location_City", transaction.Location_City);
                        cmd.Parameters.AddWithValue("@IP_Address",transaction.IP_Address);
                        //cmd.Parameters.AddWithValue("@LatencyMS", transaction.LatencyMS);


                        var scoreParam = new SqlParameter("@ExternalFraudScore", SqlDbType.Decimal);
                        scoreParam.Value = (decimal) Math.Round(transaction.ExternalFraudScore,5);
                        scoreParam.Precision = 18; // إجمالي الأرقام في الجدول
                        scoreParam.Scale = 5;     // الأرقام بعد الفاصلة في الجدول
                        cmd.Parameters.Add(scoreParam);


                        var AmountParam = new SqlParameter("@Amount", SqlDbType.Decimal);
                        AmountParam.Value = (decimal)Math.Round(transaction.Amount,2);
                        AmountParam.Precision = 18; // إجمالي الأرقام في الجدول
                        AmountParam.Scale = 2;     // الأرقام بعد الفاصلة في الجدول
                        cmd.Parameters.Add(AmountParam);

                        cmd.CommandType = CommandType.StoredProcedure;
                        await connection.OpenAsync();
                        SqlDataReader reader= await cmd.ExecuteReaderAsync();
                        if (await reader.ReadAsync())
                        {
                            return new DTO_ExecuteResult
                            {
                                TransactionID = reader.GetInt32(reader.GetOrdinal("TransactionID")),
                                AlertID = reader.GetInt32(reader.GetOrdinal("AlertID")),
                                Result = reader.GetInt32(reader.GetOrdinal("Result")),
                                Score = Convert.ToDouble(reader.GetDecimal(reader.GetOrdinal("Score"))),
                                Status = reader.GetString(reader.GetOrdinal("Status")),
                                Reason = reader.GetString(reader.GetOrdinal("Reason"))

                            };
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("An error occurred: " + ex.Message);
            }
            return new DTO_ExecuteResult { };

        }
        public List<md_Transactions> GetAll ()
        {
            List<md_Transactions> transactionsList = new List<md_Transactions>();
            string sql = "select * from [dbo].[FUN_Transactions_GetAll]() order by Date Desc";
       
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                          connection.Open ();
                        using (SqlDataReader reader =   cmd.ExecuteReader ())
                        {
                            while (  reader.Read ())
                            {
                                var transaction = new md_Transactions(
                                    Status: reader.GetString(reader.GetOrdinal("TransactionStatus")),
                                    TransactionID: reader.GetInt32(reader.GetOrdinal("TransactionID")),
                                    CardholderID: reader.GetInt32(reader.GetOrdinal("CardholderID")),
                                    Amount: reader.GetString(reader.GetOrdinal("Amount")),
                                    Name: reader.GetString(reader.GetOrdinal("Name")),
                                    TransactionType: reader.GetString(reader.GetOrdinal("TransactionType")),
                                    MaskedCardNumber: reader.GetString(reader.GetOrdinal("CardNumber")),
                                    RiskLevel: reader.GetString(reader.GetOrdinal("RiskLevel")),
                                    Date: reader.GetDateTime(reader.GetOrdinal("Date"))
                                );
                                transactionsList.Add(transaction);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            return transactionsList;
        }
        public md_DetailedTransaction GetTransactionById(int ID)
        {
            md_DetailedTransaction? transaction = null;
            string sql = "select * from [FUN_Transactions_GetDetailTransactionByID](@ID)";
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@ID", ID);
                        cmd.CommandType = CommandType.Text;
                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                transaction = new md_DetailedTransaction(
                                    TransactionID: reader.GetInt32(reader.GetOrdinal("TransactionID")),
                                    Name: reader.GetString(reader.GetOrdinal("Name")),
                                    MaskedCardNumber: reader.GetString(reader.GetOrdinal("CardNumber")),
                                    Amount: reader.GetString(reader.GetOrdinal("Amount")),
                                    TransactionStatus: reader.GetString(reader.GetOrdinal("TransactionStatus")),
                                    CardID: reader.GetInt32(reader.GetOrdinal("CardID")),
                                    CardType: reader.GetString(reader.GetOrdinal("CardType")),
                                    IssueCountry: reader.GetString(reader.GetOrdinal("IssueCountry")),
                                    MerchantCategory: reader.GetString(reader.GetOrdinal("MerchantCategory")),
                                    Location: reader.GetString(reader.GetOrdinal("Location")),
                                    DeviceType: reader.GetString(reader.GetOrdinal("DeviceType")),
                                    IP_Address: reader.GetString(reader.GetOrdinal("IP_Address")),
                                    RiskLevel: reader.GetString(reader.GetOrdinal("RiskLevel")),
                                    Reason: reader.GetString(reader.GetOrdinal("Reason")),
                                    ExpiryDate: reader.GetDateTime(reader.GetOrdinal("ExpiryDate")),
                                    TransactionDate: reader.GetDateTime(reader.GetOrdinal("TransactionDate")),
                                    CardholderID: reader.GetInt32(reader.GetOrdinal("CardholderID"))
                            );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            return transaction!;
        }
        public List<DTO_LatestTransaction> GetLatestTransactionByByCardholderID(int CardholderID)
        {

            List<DTO_LatestTransaction> LatestTransaction=new List<DTO_LatestTransaction>();
            string sql = "select * from FUN_Transaction_GetLastTransaction(@cardholderID)";
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@cardholderID", CardholderID);
                        cmd.CommandType = CommandType.Text;
                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                var dlt= new DTO_LatestTransaction
                                {
                                    TransactionID = reader.GetInt32(reader.GetOrdinal("TransactionID")),
                                    cardID = reader.GetInt32(reader.GetOrdinal("CardID")),
                                    Amount = reader.GetString(reader.GetOrdinal("Amount")),
                                    Merchant = reader.GetString(reader.GetOrdinal("Merchant")),
                                    Message = reader.GetString(reader.GetOrdinal("Message")),
                                    Time = reader.GetDateTime(reader.GetOrdinal("Time")),
                                    Status = reader.GetString(reader.GetOrdinal("Status"))
                                };
                                LatestTransaction.Add(dlt);
                            }
                        }
                    }
                }
                return LatestTransaction;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            return null!;
        }

        public async Task<bool> FinalizeAfterOtp(DTO_FinalizeAfterOtp dTO)
        {
            string sql = "SP_Transactions_FinalizeAfterOtp";
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // التأكد من مطابقة الأسماء للبروسيجر بالضبط
                        cmd.Parameters.AddWithValue("@transactionID", dTO.transactionId);
                        cmd.Parameters.AddWithValue("@CardID", dTO.CardID);
                        cmd.Parameters.AddWithValue("@Amount", dTO.amount);

                        SqlParameter returnParam = new SqlParameter
                        {
                            Direction = ParameterDirection.ReturnValue
                        };
                        cmd.Parameters.Add(returnParam);

                        await connection.OpenAsync(); // استخدام الفتح غير المتزامن
                        await cmd.ExecuteNonQueryAsync();

                        int result = (int)returnParam.Value;
                        return result == 1;
                    }
                }
            }
            catch (Exception ex)
            {
                // اطبع الخطأ لتعرف إذا كان هناك Permission أو Connection issue
                Console.WriteLine($"Database Error: {ex.Message}");
                return false;
            }
        }
    }
}
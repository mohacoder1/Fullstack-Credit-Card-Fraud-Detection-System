using CreditCard_DataAccessLayer.Database;
using CreditCard_DataAccessLayer.DTO;
using CreditCard_DataAccessLayer.Models;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_DataAccessLayer.Repository
{
    public static class CardsRepository
    {



        public static async Task<int> AddNewCard(DTO_AddNewCard Card)
        {
            string sql = "[SP_Cards_CreateCard]";
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CardholderID", Card.CardholderID);
                        cmd.Parameters.AddWithValue("@CardNumber", Card.CardNumber);
                        cmd.Parameters.AddWithValue("@CardType", Card.CardType);
                        cmd.Parameters.AddWithValue("@Balance", Card.Balance);
                        cmd.Parameters.AddWithValue("@CVV", Card.CVV);

                        SqlParameter outputIdParam = new SqlParameter("@CardID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        await connection.OpenAsync();
                        cmd.Parameters.Add(outputIdParam);
                        await cmd.ExecuteNonQueryAsync();
                        await connection.CloseAsync();
                        return (int)outputIdParam.Value;

                    }

                }
            }
            catch (Exception)
            {

                return -1;
            }

        }
        public static async Task<bool> UpdateCardBalanceAfterTransaction(int CardID, double Amount)
        {
            string sql = "[dbo].[SP_Cards_UpdateBalanceAfterTransaction]";
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CardID", CardID);
                        cmd.Parameters.AddWithValue("@Amount", Amount);

                        await connection.OpenAsync();
                        object Result = await cmd.ExecuteScalarAsync();
                        if (Result != null && int.TryParse(Result.ToString(), out int ProcessStatus))
                            return ProcessStatus > 0;
                        await connection.CloseAsync();
                    }

                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
        public static async Task<bool> UpdateCardStatus(DTO_UpdateCardStatus DTO)
        {
            string sql = "[SP_Cards_UpdateStatus]";
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CardID", DTO.CardID);
                        cmd.Parameters.AddWithValue("@Status", DTO.Status);
                        cmd.Parameters.AddWithValue("@viewerID", DTO.viewerID);

                        await connection.OpenAsync();
                        object Result = await cmd.ExecuteScalarAsync();
                        if (Result != null && int.TryParse(Result.ToString(), out int ProcessStatus))
                            return ProcessStatus > 0;
                        await connection.CloseAsync();
                    }

                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
        public static async Task<md_Cards> GetCardInfoBycardholderID(int CardholderID)
        {
            string sql = "select * from [dbo].[FUN_Cards_GetCardsSafeDetailByCardholderID](@CardholderID)";
            try
            {
              
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                 
                        cmd.Parameters.AddWithValue("@CardholderID", CardholderID);


                        await connection.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                            var Card=new md_Cards(
                                cardID: reader.GetInt32(reader.GetOrdinal("CardID")),
                                cardholderID:reader.GetInt32(reader.GetOrdinal("CardholderID")),
                                CardholderName:reader.GetString(reader.GetOrdinal("CardholderName")),
                                displayNumber:reader.GetString(reader.GetOrdinal("CardNumber")),
                                 BaseCurrency:reader.GetString(reader.GetOrdinal("BaseCurrency")),
                                 IssueCountry:reader.GetString(reader.GetOrdinal("IssueCountry")),
                                cardStatus:reader.GetString(reader.GetOrdinal("CardStatus")),
                                cardType:reader.GetString(reader.GetOrdinal("CardType")),
                                balance:Convert.ToDouble(reader.GetDecimal(reader.GetOrdinal("Balance"))),
                              
                                 expiryDate:  reader.GetDateTime(reader.GetOrdinal("ExpiryDate"))
                                );
                               
                                 return Card;
                            }
                        }
                    }

                }
            }
            catch (Exception ex )
            {
                Console.WriteLine(ex.Message);
            }
              return null!;
        }
        public static async Task<md_Cards> GetCardInfoByID(int CardID)
        {
            string sql = "select * from [dbo].[FUN_Cards_GetCardSafeDetailsByID](@CardID)";
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                 
                        cmd.Parameters.AddWithValue("@CardID", CardID);


                        await connection.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var Card = new md_Cards(
                                cardID: reader.GetInt32(reader.GetOrdinal("CardID")),
                                cardholderID: reader.GetInt32(reader.GetOrdinal("CardholderID")),
                                CardholderName: reader.GetString(reader.GetOrdinal("CardholderName")),
                                displayNumber: reader.GetString(reader.GetOrdinal("CardNumber")),
                                 BaseCurrency: reader.GetString(reader.GetOrdinal("BaseCurrency")),
                                 IssueCountry: reader.GetString(reader.GetOrdinal("IssueCountry")),
                                cardStatus: reader.GetString(reader.GetOrdinal("CardStatus")),
                                cardType: reader.GetString(reader.GetOrdinal("CardType")),
                                balance: Convert.ToDouble(reader.GetDecimal(reader.GetOrdinal("Balance"))),
                            
                                 expiryDate: reader.GetDateTime(reader.GetOrdinal("ExpiryDate"))
                                );

                              

                                return Card;
                            }
                        }
                    }

                }
            }
            catch (Exception)
            {
                return null!;
            }
            return null!;
        }
        public static async Task<List<md_Cards>> GetCardsInfo()
        {
            List<md_Cards> Cards = new List<md_Cards>();
            string sql = "select * from [dbo].[FUN_Cards_GetCardsSafeDetails]()";
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {

                        await connection.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var Card = new md_Cards(
                                cardID: reader.GetInt32(reader.GetOrdinal("CardID")),
                                cardholderID: reader.GetInt32(reader.GetOrdinal("CardholderID")),
                                CardholderName: reader.GetString(reader.GetOrdinal("CardholderName")),
                                displayNumber: reader.GetString(reader.GetOrdinal("CardNumber")),
                                 BaseCurrency: reader.GetString(reader.GetOrdinal("BaseCurrency")),
                                 IssueCountry: reader.GetString(reader.GetOrdinal("IssueCountry")),
                                cardStatus: reader.GetString(reader.GetOrdinal("CardStatus")),
                                cardType: reader.GetString(reader.GetOrdinal("CardType")),
                                balance: Convert.ToDouble(reader.GetDecimal(reader.GetOrdinal("Balance"))),
                            
                                 expiryDate: reader.GetDateTime(reader.GetOrdinal("ExpiryDate"))
                                );

                               
                                Cards.Add(Card);
                            }
                        }

                    }
                    return Cards;
                }
            }
            catch (Exception)
            {
                return null!;
            }
        }
        public static async Task<bool> CheckCardBalance(int CardID, double WithdrawAmount)
        {
            string sql = "[SP_Cards_CheckBalance]";
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CardID", CardID);
                        cmd.Parameters.AddWithValue("@WithdrawAmount", WithdrawAmount);

                        await connection.OpenAsync();
                        object Result = await cmd.ExecuteScalarAsync();
                        if (Result != null && int.TryParse(Result.ToString(), out int ProcessStatus))
                            return ProcessStatus > 0;
                        await connection.CloseAsync();
                    }

                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
    }
}

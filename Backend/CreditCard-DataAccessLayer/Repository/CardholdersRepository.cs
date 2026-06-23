using CreditCard_DataAccessLayer.Database;
using CreditCard_DataAccessLayer.DTO;
using CreditCard_DataAccessLayer.Models;
using CreditCard_DataAccessLayer.Models.Transactions;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CreditCard_DataAccessLayer.Repository
{
    public static class CardholdersRepository
    {

      
        public static async Task<int> CreateCardholder(DTO_AddNewCardholder dto)
        {
            string sql = "[dbo].[SP_Cardholders_CreateCardholders]";
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // إضافة المعاملات (Input)
                        cmd.Parameters.AddWithValue("@FirstName", dto.FirstName);
                        cmd.Parameters.AddWithValue("@SecondName", dto.SecondName);
                        cmd.Parameters.AddWithValue("@ThirdName", dto.ThirdName);
                        cmd.Parameters.AddWithValue("@LastName", dto.LastName);
                        cmd.Parameters.AddWithValue("@Email", dto.Email);
                        cmd.Parameters.AddWithValue("@DateOfBirth", dto.DateOfBirth);
                        cmd.Parameters.AddWithValue("@PhoneNumber", dto.PhoneNumber);
                        cmd.Parameters.AddWithValue("@NationalID", dto.NationalID);
                        cmd.Parameters.AddWithValue("@PasswordHash", dto.PasswordHash);
                        cmd.Parameters.AddWithValue("@Country", dto.Country);

                        // بيانات البطاقه
                        cmd.Parameters.AddWithValue("@CardNumber", dto.CardNumber);
                        cmd.Parameters.AddWithValue("@CardType", dto.CardType);
                        cmd.Parameters.AddWithValue("@Balance", dto.Balance);
                        cmd.Parameters.AddWithValue("@CVV", dto.CVV);
                        


                        // إعداد معامل المخرجات (Output)
                        SqlParameter outputIdParam = new SqlParameter("@NewID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputIdParam);

                        await connection.OpenAsync();

                        // نستخدم ExecuteNonQuery لأننا ننتظر القيمة في الـ Parameter وليس كـ Result Set
                        await cmd.ExecuteNonQueryAsync();

                        // استرجاع القيمة من المعلمة بعد التنفيذ
                        if (outputIdParam.Value != null && outputIdParam.Value != DBNull.Value)
                        {
                            return (int)outputIdParam.Value;
                        }
                        else
                        {
                            throw new InvalidOperationException("Insert succeeded but NewID was not returned.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // في نظام حساس مثل كشف الاحتيال، يفضل تسجيل الخطأ (Logging) بدلاً من الطباعة فقط
                Console.WriteLine("An error occurred: " + ex.Message);
                return -1;
            }
        }
        public static async Task<md_Cardholders?> GetCardholderByEmail(string Email)
        {
            md_Cardholders? Cardholder = null!;
            string sql = "select * from [dbo].[FUN_People_GetProfileByEmail](@Email)";
            
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Email", Email);
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                    Cardholder = new md_Cardholders(
                                    CardholderID: reader.GetInt32(reader.GetOrdinal("CardholderID")),
                                    Name: reader.GetString(reader.GetOrdinal("Name")),
                                    Email: reader.GetString(reader.GetOrdinal("Email")),
                                    PhoneNumber: reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                    NationalID: reader.GetString(reader.GetOrdinal("NationalID")),
                                    isActive: reader.GetBoolean(reader.GetOrdinal("isActive"))?"Active ":"inActive",
                                    Country: reader.GetString(reader.GetOrdinal("Country")),
                                    JoinDate: reader.GetDateTime(reader.GetOrdinal("JoinDate")),
                                    CardsCount: reader.GetInt32(reader.GetOrdinal("CardsCount")),
                                    MembershipLevel: reader.GetString(reader.GetOrdinal("MembershipLevel"))
                          
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
            return Cardholder;
        }
        public static async Task<DTO_DetailedCardholder?> GetCardholderInDetails(int CardholderID)
        {
            DTO_DetailedCardholder? Cardholder = null!;
            string sql = "select * from [dbo].[FUN_Cardholders_GetCardholderDetails](@CardholderID)";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CardholderID", CardholderID);
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                Cardholder = new DTO_DetailedCardholder
                                {
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    Id = reader.GetInt32(reader.GetOrdinal("CardholderID")),
                                    Name = reader.GetString(reader.GetOrdinal("FullName")),
                                    DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                    JoinDate = reader.GetDateTime(reader.GetOrdinal("JoinDate")),
                                    MembershipLevel = reader.GetString(reader.GetOrdinal("MembershipLevel")),
                                    Country = reader.GetString(reader.GetOrdinal("Country")),
                                    NationalID = reader.GetString(reader.GetOrdinal("NationalID")),
                                    Role = reader.GetString(reader.GetOrdinal("Role")),
                                    Status = reader.GetString(reader.GetOrdinal("Status"))
                                };
                              

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            return Cardholder;
        }
        public static async Task<md_Cardholders?> GetCardholderByID(int CardholderID)
        {
            md_Cardholders? Cardholder = null!;
            string sql = "select * from [dbo].[FUN_People_GetFullProfile](@CardholderID)";
            
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CardholderID", CardholderID);
                        await connection.OpenAsync();
                        using (SqlDataReader reader =await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                    Cardholder = new md_Cardholders(
                                    CardholderID: reader.GetInt32(reader.GetOrdinal("CardholderID")),
                                    Name: reader.GetString(reader.GetOrdinal("Name")),
                                    Email: reader.GetString(reader.GetOrdinal("Email")),
                                    PhoneNumber: reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                    NationalID: reader.GetString(reader.GetOrdinal("NationalID")),
                                    isActive: reader.GetBoolean(reader.GetOrdinal("isActive")) ? "Active " : "inActive",
                                    Country: reader.GetString(reader.GetOrdinal("Country")),
                                    JoinDate: reader.GetDateTime(reader.GetOrdinal("JoinDate")),
                                    CardsCount: reader.GetInt32(reader.GetOrdinal("CardsCount")),
                                    MembershipLevel: reader.GetString(reader.GetOrdinal("MembershipLevel"))
                          
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
            return Cardholder;
        }
        public static async Task<List<md_Cardholders?>> GetAllCardholders()
        {
            List<md_Cardholders?> Cardholders = new List<md_Cardholders?>();
            string sql = "select * from [dbo].[FUN_People_GetFullProfiles]()";
            
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var Cardholder=new md_Cardholders(
                                CardholderID: reader.GetInt32(reader.GetOrdinal("CardholderID")),
                                Name: reader.GetString(reader.GetOrdinal("Name")),
                                Email: reader.GetString(reader.GetOrdinal("Email")),
                                PhoneNumber: reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                NationalID: reader.GetString(reader.GetOrdinal("NationalID")),
                                isActive: reader.GetBoolean(reader.GetOrdinal("isActive"))?"Active":"inActive",
                                Country: reader.GetString(reader.GetOrdinal("Country")),
                                JoinDate: reader.GetDateTime(reader.GetOrdinal("JoinDate")),
                                CardsCount: reader.GetInt32(reader.GetOrdinal("CardsCount")),
                                MembershipLevel: reader.GetString(reader.GetOrdinal("MembershipLevel"))

                            );
                               Cardholders.Add(Cardholder);
                            }
                        }
                    }
                }
            return Cardholders;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            return null!;
        }
        
    }
}
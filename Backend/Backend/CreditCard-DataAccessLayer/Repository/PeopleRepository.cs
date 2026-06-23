using CreditCard_DataAccessLayer.Database;
using CreditCard_DataAccessLayer.DTO;
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
    public class PeopleRepository
    {
        public static async Task<bool> isPersonActive(int ID , string Role)
        {

            string sql = "[dbo].[SP_People_IsActive]";
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@ID", ID);
                        cmd.Parameters.AddWithValue("@Role", Role);
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlParameter returnParameter = new SqlParameter();
                        returnParameter.ParameterName = "@ReturnValue"; // الاسم لا يهم هنا برمجياً لكنه للتعريف
                        returnParameter.SqlDbType = SqlDbType.Int;       // الـ Return دائماً يكون Int في SQL
                        returnParameter.Direction = ParameterDirection.ReturnValue;
                        cmd.Parameters.Add(returnParameter);

                        await connection.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        return ((int)returnParameter.Value) > 0;

                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("An error occurred: " + ex.Message);
            }
            return false;
        }
        public static async Task<int> ResetPassword(int ID, string newPassword)
        {
            string sql = "[dbo].[SP_People_ResetPassword]";
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@ID", ID);
          
                        cmd.Parameters.AddWithValue("@newPassword", newPassword);
                        cmd.CommandType = CommandType.StoredProcedure;



                        SqlParameter returnParameter = new SqlParameter();
                        returnParameter.ParameterName = "@ReturnValue"; // الاسم لا يهم هنا برمجياً لكنه للتعريف
                        returnParameter.SqlDbType = SqlDbType.Int;       // الـ Return دائماً يكون Int في SQL
                        returnParameter.Direction = ParameterDirection.ReturnValue;
                        cmd.Parameters.Add(returnParameter);

                        await connection.OpenAsync();
                        await cmd.ExecuteNonQueryAsync(); 
               
                        return ((int)returnParameter.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return -1;
            }
        }
        public static async Task<int> ChangePassword(int ID, string OldPasswordHash, string NewPasswordHash, string Role)
        {

            string sql = "[dbo].[SP_People_ChangePassword]";
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@ID", ID);
                        cmd.Parameters.AddWithValue("@Role", Role);
                        cmd.Parameters.AddWithValue("@OldPasswordHash", OldPasswordHash);
                        cmd.Parameters.AddWithValue("@NewPasswordHash", NewPasswordHash);

                        cmd.CommandType = CommandType.StoredProcedure;
                        await connection.OpenAsync();
                        SqlParameter returnParameter = new SqlParameter();
                        returnParameter.ParameterName = "@ReturnValue"; // الاسم لا يهم هنا برمجياً لكنه للتعريف
                        returnParameter.SqlDbType = SqlDbType.Int;       // الـ Return دائماً يكون Int في SQL
                        returnParameter.Direction = ParameterDirection.ReturnValue;
                        cmd.Parameters.Add(returnParameter);

                        await connection.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        return ((int)returnParameter.Value);

                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("An error occurred: " + ex.Message);
                return -1;
            }
        }
        public static async Task<bool> SetAccoutStatus(int ID, bool Status, string Role)
        {

            string sql = "[dbo].[SP_People_SetAccountStatus]";
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@ID", ID);
                        cmd.Parameters.AddWithValue("@Role", Role);
                        cmd.Parameters.AddWithValue("@Status", Status);

                        cmd.CommandType = CommandType.StoredProcedure;
                        await connection.OpenAsync();
                        SqlParameter returnParameter = new SqlParameter();
                        returnParameter.ParameterName = "@ReturnValue"; // الاسم لا يهم هنا برمجياً لكنه للتعريف
                        returnParameter.SqlDbType = SqlDbType.Int;       // الـ Return دائماً يكون Int في SQL
                        returnParameter.Direction = ParameterDirection.ReturnValue;
                        cmd.Parameters.Add(returnParameter);

                        await cmd.ExecuteNonQueryAsync();
                        return ((int)returnParameter.Value) >0;

                       
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("An error occurred: " + ex.Message);
                return false;
            }
        }
        public async static Task<md_AuthPerson?>Login(string Identifier, string PasswordHash)
        {
			try
			{
                const string query = "[dbo].[SP_People_Login]";
                using (SqlConnection Con=new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd=new SqlCommand(query,Con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Identifier", Identifier);
                        cmd.Parameters.AddWithValue("@PasswordHash", PasswordHash);
                        await Con.OpenAsync();
                        using (SqlDataReader reader=await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var Person = new md_AuthPerson
                                    (
                                    ID:reader.GetInt32(reader.GetOrdinal("ID")),
                                    FullName:reader.GetString(reader.GetOrdinal("FullName")),                           
                                    Email: reader.GetString(reader.GetOrdinal("Email")),
                                    PhoneNumber: reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                    PersonRole: reader.GetString(reader.GetOrdinal("PersonRole")),
                                    AccessLevel: reader.GetInt32(reader.GetOrdinal("AccessLevel")),
                                    isActive:reader.GetBoolean(reader.GetOrdinal("isActive"))

                                    );
                                

                                return Person;
                            }
                        }
                    }
                }

            }
			catch (Exception)
			{
                return null;
                throw;
			}
                return null;
        }
        public async static Task<md_AuthPerson?> GetProfileByIdentifier(string Identifier)
        {
            try
            {
                const string query = "select * from [dbo].[FUN_People_GetProfileByIdentifier](@Identifier)";
                using (SqlConnection Con = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, Con))
                    {

                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Identifier", Identifier);
                        await Con.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var Person = new md_AuthPerson
                                                                   (
                                                                   ID: reader.GetInt32(reader.GetOrdinal("ID")),
                                                                   FullName: reader.GetString(reader.GetOrdinal("FullName")),
                                                                   Email: reader.GetString(reader.GetOrdinal("Email")),
                                                                   PhoneNumber: reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                                                   PersonRole: reader.GetString(reader.GetOrdinal("PersonRole")),
                                                                   AccessLevel: reader.GetInt32(reader.GetOrdinal("AccessLevel")),
                                                                   isActive: reader.GetBoolean(reader.GetOrdinal("isActive"))

                                                                   );


                                return Person;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               throw new Exception("An error occurred while retrieving the profile: " + ex.Message);
            }
            return null!;
        }
    }
}

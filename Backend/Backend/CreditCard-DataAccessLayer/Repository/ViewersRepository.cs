using CreditCard_DataAccessLayer.Database;
using CreditCard_DataAccessLayer.DTO;
using CreditCard_DataAccessLayer.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;

namespace CreditCard_DataAccessLayer.Repository
{
    public class ViewersRepository
    {

        public static async Task<DTO_DetailedView?> GetViewerInDetails(int ViewerID)
        {
            DTO_DetailedView? Viewer = null!;
            string sql = "select * from [dbo].[FUN_Viewers_GetViewerDetails](@ViewerID)";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@ViewerID", ViewerID);
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                Viewer = new DTO_DetailedView
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("ViewerID")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    Name = reader.GetString(reader.GetOrdinal("FullName")),
                                    DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
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
            return Viewer;
        }
        public static async Task<int> CreateViewer(DTO_AddNewViewer dto)
        {

            string sql = "[SP_Viewers_CreateViewer]";
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", dto.FirstName);
                        cmd.Parameters.AddWithValue("@SecondName", dto.SecondName);
                        cmd.Parameters.AddWithValue("@ThirdName", dto.ThirdName);
                        cmd.Parameters.AddWithValue("@LastName", dto.LastName);
                        cmd.Parameters.AddWithValue("@Email", dto.Email);
                        cmd.Parameters.AddWithValue("@AccessLevel", dto.AccessLevel);
                        cmd.Parameters.AddWithValue("@DateOfBirth", dto.DateOfBirth);
                        cmd.Parameters.AddWithValue("@PhoneNumber", dto.PhoneNumber);
                        cmd.Parameters.AddWithValue("@NationalID", dto.NationalID);
                        cmd.Parameters.AddWithValue("@PasswordHash", dto.PasswordHash);
                        cmd.Parameters.AddWithValue("@Country", dto.Country);


                        SqlParameter outputIdParam = new SqlParameter("@NewID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputIdParam);
                        cmd.CommandType = CommandType.StoredProcedure;
                        await connection.OpenAsync();
                        var idObj = await cmd.ExecuteScalarAsync();
                        if (idObj is null || idObj == DBNull.Value)
                            throw new InvalidOperationException("Insert succeeded but ViewerID was not returned.");

                        return Convert.ToInt32(idObj);


                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("An error occurred: " + ex.Message);
                return -1;
            }
        }
        public static async Task<md_Viewers?> GetViewerByID(int ID)
        {
            md_Viewers? viewer = null!;
            string sql = "select * from [FUN_Viewers_GetViewerByID](@ID)";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("ID", ID);
                        await connection.OpenAsync();
                        using (SqlDataReader reader =await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                viewer = new md_Viewers(
                                ViewerID: reader.GetInt32(reader.GetOrdinal("ViewerID")),
                                Name: reader.GetString(reader.GetOrdinal("Name")),
                                Email: reader.GetString(reader.GetOrdinal("Email")),
                                PhoneNumber: reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                NationalID: reader.GetString(reader.GetOrdinal("NationalID")),
                                isActive: reader.GetString(reader.GetOrdinal("isActive")),
                                Country: reader.GetString(reader.GetOrdinal("Country")),
                                DateOfBirth: reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                                                AccessLevel: reader.GetInt32(reader.GetOrdinal("AccessLevel"))

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
            return viewer;
        }
        public static async Task<List<md_Viewers>> GetAll()
        {
            List<md_Viewers> viewers = new List<md_Viewers>();
            string sql = "select * from [dbo].[FUN_Viewers_GetAll]()";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var Viewer = new md_Viewers(
                                
                                ViewerID: reader.GetInt32(reader.GetOrdinal("ViewerID")),
                                Name: reader.GetString(reader.GetOrdinal("Name")),
                                Email: reader.GetString(reader.GetOrdinal("Email")),
                                PhoneNumber: reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                NationalID: reader.GetString(reader.GetOrdinal("NationalID")),
                                isActive: reader.GetString(reader.GetOrdinal("isActive")),
                                Country: reader.GetString(reader.GetOrdinal("Country")),
                                DateOfBirth: reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                AccessLevel: reader.GetInt32(reader.GetOrdinal("AccessLevel"))

                                );
                                viewers.Add(Viewer);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            return viewers;
        }
        public static async Task<int> CheckUserPermission (int userId)
        {
            string procedureName = "[sp_CheckUserPermission]";
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand(procedureName, sqlConnection))
                    {
                        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@UserId", userId);
                          await sqlConnection.OpenAsync ();
                        object result =   await sqlCommand.ExecuteScalarAsync ();
                          
                        if (result != null && int.TryParse(result.ToString(), out int permissionLevel))
                        {
                            return permissionLevel;
                        }
                        else
                        {
                            throw new Exception("Failed to retrieve user permission level.");
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);

            }
            return -1;
        }
    }
}
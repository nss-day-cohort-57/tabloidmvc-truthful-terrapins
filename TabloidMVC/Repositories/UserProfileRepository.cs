using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TabloidMVC.Models;
using TabloidMVC.Utils;
using System.Data.SqlClient;
using System.Data;

namespace TabloidMVC.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration config) : base(config) { }

        public UserProfile GetByEmail(string email)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    
                    cmd.CommandText = @"
                       SELECT u.id, u.FirstName, u.LastName, u.DisplayName, u.Email,
                              u.CreateDateTime, u.ImageLocation, u.UserTypeId,
                              ut.[Name] AS UserTypeName
                         FROM UserProfile u
                              LEFT JOIN UserType ut ON u.UserTypeId = ut.id
                        WHERE email = @email";
                    cmd.Parameters.AddWithValue("@email", email);

                    UserProfile userProfile = null;
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        userProfile = new UserProfile()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                            UserType = new UserType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                            },
                        };
                    }

                    reader.Close();

                    return userProfile;
                }
            }
        }

        public List <UserProfile> GetAllUsers()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT DisplayName, FirstName, LastName, UserTypeId, UserType.Name as UserTypeName 
                        FROM UserProfile  
                        JOIN UserType 
                        ON UserType.Id = UserProfile.UserTypeId
                        ORDER BY DisplayName ASC
                    ";

                    

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List <UserProfile> users = new List <UserProfile>();
                        
                        while (reader.Read())
                        {
                            UserProfile user = new UserProfile
                            {
                                DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                UserType = new UserType
                                {
                                    Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                                }
                                
                            };
                            

                            

                            users.Add(user);
                        }
                        return users;
                    }
                }
            }
        }

        public UserProfile GetUserById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT FirstName, LastName, DisplayName, Email, CreateDateTime, UserTypeId, UserType.Name UserTypeName 
                        FROM UserProfile  
                        JOIN UserType 
                        ON UserType.Id = UserProfile.UserTypeId
                        
                    ";

                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            UserProfile user = new UserProfile
                            {
                                
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                //ImageLocation = reader.GetString(reader.GetOrdinal("ImageLocation")),
                                DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                                UserType = new UserType
                                {
                                    Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                                }

                                
                            };

                            return user;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
    }
}

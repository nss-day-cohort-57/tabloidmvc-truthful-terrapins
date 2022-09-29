using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        public  CommentRepository(IConfiguration config) : base(config) { }

        public List<Comment> GetAllCommentsByPostId(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Subject, c.Content,c.PostId,p.Title,u.FirstName,u.LastName,
                                        u.DisplayName,c.UserProfileId, u.DisplayName as AuthorName, c.CreateDateTime as CreatedOn 
                                        FROM Comment c
                                        LEFT JOIN Post p ON p.Id = c.PostId
                                        LEFT JOIN UserProfile u ON u.Id = c.UserProfileId
                                        WHERE p.Id = @id
                                        order by c.CreateDateTime";

                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();

                    var comments = new List<Comment>();

                    while (reader.Read())
                    {
                        comments.Add(new Comment()
                        {
                            Subject = reader.GetString(reader.GetOrdinal("Subject")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                            UserProfile = new UserProfile()
                            {
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                         
                            },
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                            Post = new Post()
                            {
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                            },
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreatedOn")),
                           
                        });
                    }

                    reader.Close();

                    return comments;
                }
            }
        }

        public void AddComment(Comment comment)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Comment (
                           PostId, UserProfileId, Subject, Content, CreateDateTime  )
                        OUTPUT INSERTED.ID
                        VALUES (
                            @PostId, @UserProfileId, @Subject, @Content, @CreateDateTime)";
                    cmd.Parameters.AddWithValue("@Subject", comment.Subject);
                    cmd.Parameters.AddWithValue("@Content", comment.Content);
                    cmd.Parameters.AddWithValue("@PostId", comment.PostId);
                    cmd.Parameters.AddWithValue("@UserProfileId", comment.UserProfileId);
                    cmd.Parameters.AddWithValue("@CreateDateTime", comment.CreateDateTime);

                    int id = (int)cmd.ExecuteScalar();
                    DateTime createDateTime = (DateTime)cmd.ExecuteScalar();

                    comment.Id = id;
                }
            }
        }
    }
}

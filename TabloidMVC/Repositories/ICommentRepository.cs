using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ICommentRepository
    {
        public List<Comment> GetAllCommentsByPostId(int id);
        public void AddComment(Comment comment);
    }
}

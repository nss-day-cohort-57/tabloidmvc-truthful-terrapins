using System.Collections.Generic;

namespace TabloidMVC.Models.ViewModels
{
    public class CommentCreateViewModel
    {
        public Comment Comment { get; set; }
        public List<Comment> CommentOptions { get; set; }
    }
}

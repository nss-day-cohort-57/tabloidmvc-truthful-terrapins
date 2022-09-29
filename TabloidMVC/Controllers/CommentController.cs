using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Security.Claims;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class CommentController : Controller
    {

        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ICommentRepository _commentRepository;

        public CommentController(
            IPostRepository postRepository,
            ICategoryRepository categoryRepository,
            ITagRepository tagRepository,
            ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _commentRepository = commentRepository;
            _tagRepository = tagRepository;
        }

        public IActionResult Index(int id)
        {

            var comments = _commentRepository.GetAllCommentsByPostId(id);
            var post = _postRepository.GetPublishedPostById(id);
            ViewData["PostTitle"] = post.Title;
            ViewData["PostId"] = post.Id;
            return View(comments);
        }

        public IActionResult Create(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);
            ViewData["PostTitle"] = post.Title;
            ViewData["PostId"] = post.Id;
            var vm = new CommentCreateViewModel();
            vm.CommentOptions = _commentRepository.GetAllCommentsByPostId(id);
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(int id, CommentCreateViewModel vm)
        {
            int UserId = GetCurrentUserProfileId();
            try
            {
                vm.Comment.CreateDateTime = DateTime.Now;
                vm.Comment.UserProfileId = UserId;
                vm.Comment.PostId = id;
                _commentRepository.AddComment(vm.Comment);
                return RedirectToAction("Index", new { id = vm.Comment.Id });
            }
            catch
            {
                vm.CommentOptions = _commentRepository.GetAllCommentsByPostId(id);
                return View(vm);
            }
        }
        private int GetCurrentUserProfileId()
        {
            string Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(Id);
        }
    }
}

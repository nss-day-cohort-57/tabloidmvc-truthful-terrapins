﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Security.Claims;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;
using TabloidMVC.Models;
using System.Collections.Generic;
using System;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;

        public PostController(
            IPostRepository postRepository,
            ICategoryRepository categoryRepository,
            ITagRepository tagRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
        }

        public IActionResult Index()
        {
            var posts = _postRepository.GetAllPublishedPosts();
            return View(posts);
        }

        public IActionResult Details(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);
            if (post == null)
            {
                int userId = GetCurrentUserProfileId();
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }
            return View(post);
        }

        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            //vm.Post.PublishDateTime = DateTime.Now;
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            } 
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }

        // GET: Post/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            List<Category> categories = _categoryRepository.GetAll();

            PostCreateViewModel vm = new PostCreateViewModel()
            {
                Post = _postRepository.GetPublishedPostById(id),
                CategoryOptions = categories
        };
            if (vm == null)
            {
                return NotFound();
            }
            return View(vm);
        }

        // POST: Post/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, PostCreateViewModel vm)
        {
            List<Category> categories = _categoryRepository.GetAll();
            vm.CategoryOptions = categories;
            try
            {
                _postRepository.UpdatePost(vm.Post);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(vm);
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}

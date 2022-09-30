using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserProfileRepository _userRepo;

        public UserController(IUserProfileRepository userProfileRepository)
        {
            _userRepo = userProfileRepository;
        }

        public IActionResult Index()
        {
            List<UserProfile> users = _userRepo.GetAllUsers();

            return View(users);
        }

        public ActionResult Details(int id)
        {
            UserProfile user = _userRepo.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public ActionResult Create()
        {
            var pro = new UserProfile();
            var UserProfiles = _userRepo.GetAllUsers();
            return View(pro);
        }

        [HttpPost]

        public ActionResult Create(UserProfile userProfile)
        {
            try
            {
                userProfile.CreateDateTime = DateTime.Now;
                userProfile.ImageLocation = "https://upload.wikimedia.org/wikipedia/commons/a/ac/No_image_available.svg";
                int id = _userRepo.CreateUser(userProfile);

                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                return View(userProfile);
            }
        }
        
    }
}
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            List<UserProfile> users = _userRepo.GetAllUsers();

            return View(users);
        }

        private readonly IUserProfileRepository _userRepo;

        public UserController(IUserProfileRepository userProfileRepository)
        {
            _userRepo = userProfileRepository;
        }
    }
}

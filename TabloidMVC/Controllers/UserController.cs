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

        public ActionResult Details(int id)
        {
            UserProfile user = _userRepo.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
    }
}

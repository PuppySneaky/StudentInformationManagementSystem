using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentInformationManagementSystem.Attributes;
using StudentInformationManagementSystem.Interfaces;
using StudentInformationManagementSystem.Models;
using System.Threading.Tasks;

namespace StudentInformationManagementSystem.Controllers
{
    [AuthorizeRoles("Admin")]
    public class AdminController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AdminController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: Admin/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            // Get current admin user
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (userId == 0)
            {
                return RedirectToAction("Login", "Auth");
            }

            var user = await _userRepository.GetByIdAsync(userId);
            ViewBag.UserName = user.Username;

            // You can add statistics or data for the dashboard here
            ViewBag.TotalUsers = (await _userRepository.GetAllAsync()).Count();

            return View();
        }

        // Admin functionality can be added here
        // For example:

        // GET: Admin/ManageUsers
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _userRepository.GetAllAsync();
            return View(users);
        }

        // GET: Admin/ManageCourses
        public IActionResult ManageCourses()
        {
            // This would require a CourseRepository
            return View();
        }
    }
}
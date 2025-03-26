using Microsoft.AspNetCore.Mvc;
using StudentInformationManagementSystem.Models;
using StudentInformationManagementSystem.Services;
using System.Diagnostics;

namespace StudentInformationManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // If the user is not logged in, redirect to login page
            // Uncomment this to force login before accessing home page
            /*
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            */

            return View();
        }
        // Add this method to your HomeController class
        public async Task<IActionResult> RecreateAdmin()
        {
            await DbSeeder.ForceCreateAdminUser(HttpContext.RequestServices.GetRequiredService<IApplicationBuilder>());
            return Content("Admin recreated");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
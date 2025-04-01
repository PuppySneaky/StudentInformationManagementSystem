using Microsoft.AspNetCore.Mvc;
using StudentInformationManagementSystem.Interfaces;
using StudentInformationManagementSystem.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;

namespace StudentInformationManagementSystem.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // GET: Auth/Login
        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _authService.AuthenticateAsync(model.Username, model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt");
                return View(model);
            }

            // Create session for the logged-in user
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("UserRole", user.Role.Name);

            // Redirect based on user role
            if (user.Role.Name == "Admin")
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            else if (user.Role.Name == "Faculty")
            {
                // Redirect to faculty dashboard when it's implemented
                return RedirectToAction("Index", "Home");
            }
            else if (user.Role.Name == "Student")
            {
                // Redirect to student dashboard when it's implemented
                return RedirectToAction("Index", "Home");
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Auth/Logout
        public IActionResult Logout()
        {
            // Clear the session
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        // GET: Auth/AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
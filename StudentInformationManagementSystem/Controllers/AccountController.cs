using Microsoft.AspNetCore.Mvc;
using StudentInformationManagementSystem.Interfaces;
using StudentInformationManagementSystem.Models;
using System;
using System.Threading.Tasks;

namespace StudentInformationManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserFactory _userFactory;

        public AccountController(IUserFactory userFactory)
        {
            _userFactory = userFactory;
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Create a new student user
                var user = await _userFactory.CreateStudentUserAsync(
                    model.Username,
                    model.Email,
                    model.Password,
                    model.FirstName,
                    model.LastName
                );

                // Redirect to login page with success message
                TempData["SuccessMessage"] = "Registration successful! You can now log in.";
                return RedirectToAction("Login", "Auth");
            }
            catch (InvalidOperationException ex)
            {
                // This catches username/email already exists exceptions
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
            catch (Exception)
            {
                // Generic error handling
                ModelState.AddModelError("", "An error occurred during registration. Please try again.");
                return View(model);
            }
        }
    }
}
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
        private readonly ILogger<AccountController> _logger;

        public AccountController(IUserFactory userFactory, ILogger<AccountController> logger)
        {
            _userFactory = userFactory;
            _logger = logger;
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
                // Create a new student user with all the information
                var user = await _userFactory.CreateStudentUserAsync(
                    model.Username,
                    model.Email,
                    model.Password,
                    model.FirstName,
                    model.LastName,
                    model.Address ?? "",
                    model.PhoneNumber,
                    model.DateOfBirth,
                    model.StudentNumber ?? ""
                );

                // Redirect to login page with success message
                TempData["SuccessMessage"] = "Registration successful! You can now log in.";
                return RedirectToAction("Login", "Auth");
            }
            catch (InvalidOperationException ex)
            {
                // This catches username/email already exists exceptions
                _logger.LogWarning(ex, "Registration failed due to duplicate username/email");
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
            catch (Exception ex)
            {
                // Log the actual exception for debugging
                _logger.LogError(ex, "Error during registration");

                // Display the specific error message for debugging purposes
#if DEBUG
                ModelState.AddModelError("", $"Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    ModelState.AddModelError("", $"Inner Error: {ex.InnerException.Message}");
                }
#else
                ModelState.AddModelError("", "An error occurred during registration. Please try again.");
#endif

                return View(model);
            }
        }
    }
}
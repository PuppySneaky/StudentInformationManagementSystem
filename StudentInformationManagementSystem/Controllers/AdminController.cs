using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentInformationManagementSystem.Attributes;
using StudentInformationManagementSystem.Interfaces;
using StudentInformationManagementSystem.Models;
using System;
using System.Threading.Tasks;

namespace StudentInformationManagementSystem.Controllers
{
    [AuthorizeRoles("Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            IAdminService adminService,
            IUserRepository userRepository,
            ILogger<AdminController> logger)
        {
            _adminService = adminService;
            _userRepository = userRepository;
            _logger = logger;
        }

        // GET: Admin/Dashboard
        public IActionResult Dashboard()
        {
            return View();
        }

        // GET: Admin/Users
        public async Task<IActionResult> Users()
        {
            var users = await _userRepository.GetAllAsync();
            return View(users);
        }

        // GET: Admin/Setup (without authorization to allow initial setup)
        [AllowAnonymous]
        public async Task<IActionResult> Setup()
        {
            // Only allow setup if no users exist in the system
            if (!await _adminService.IsFirstRunAsync())
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new AdminSetupViewModel());
        }

        // POST: Admin/Setup
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Setup(AdminSetupViewModel model)
        {
            // Only allow setup if no users exist in the system
            if (!await _adminService.IsFirstRunAsync())
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var admin = await _adminService.CreateAdminAccountAsync(
                        model.Username,
                        model.Email,
                        model.Password);

                    // Set success message
                    TempData["SuccessMessage"] = "Admin account created successfully. You can now log in.";

                    // Redirect to login page
                    return RedirectToAction("Login", "Auth");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    _logger.LogError(ex, "Error creating admin account");
                }
            }

            return View(model);
        }

        // GET: Admin/CreateAdmin
        public IActionResult CreateAdmin()
        {
            return View(new CreateAdminViewModel());
        }

        // POST: Admin/CreateAdmin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdmin(CreateAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var admin = await _adminService.CreateAdminAccountAsync(
                        model.Username,
                        model.Email,
                        model.Password);

                    TempData["SuccessMessage"] = "Admin account created successfully.";
                    return RedirectToAction(nameof(Users));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    _logger.LogError(ex, "Error creating admin account");
                }
            }

            return View(model);
        }
    }
}
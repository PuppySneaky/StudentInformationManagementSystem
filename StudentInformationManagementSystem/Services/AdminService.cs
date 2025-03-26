using Microsoft.EntityFrameworkCore;
using StudentInformationManagementSystem.Data;
using StudentInformationManagementSystem.Interfaces;
using StudentInformationManagementSystem.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StudentInformationManagementSystem.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserFactory _userFactory;

        public AdminService(ApplicationDbContext context, IUserFactory userFactory)
        {
            _context = context;
            _userFactory = userFactory;
        }

        public async Task<User> CreateAdminAccountAsync(string username, string email, string password)
        {
            // Check if admin account already exists
            var adminExists = await _context.Users
                .AnyAsync(u => u.Role.Name == "Admin");

            if (adminExists)
            {
                throw new InvalidOperationException("An admin account already exists.");
            }

            // Create admin account using the UserFactory
            var adminUser = await _userFactory.CreateAdminUserAsync(username, email, password);

            return adminUser;
        }

        public async Task<bool> IsFirstRunAsync()
        {
            // Check if any users exist in the system
            return !await _context.Users.AnyAsync();
        }
    }
}
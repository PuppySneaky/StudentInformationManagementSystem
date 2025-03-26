using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StudentInformationManagementSystem.Data;
using StudentInformationManagementSystem.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StudentInformationManagementSystem.Services
{
    public static class DbSeeder
    {
        public static async Task ForceCreateAdminUser(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var userFactory = services.GetRequiredService<IUserFactory>();
                    var logger = services.GetRequiredService<ILogger<Program>>();

                    logger.LogInformation("Force creating admin account.");

                    var adminUser = await userFactory.CreateAdminUserAsync(
                        username: "admin",
                        email: "admin@university.edu",
                        password: "Admin@123456"
                    );

                    logger.LogInformation($"Admin account created. UserId: {adminUser.UserId}");
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while creating admin user.");
                }
            }
        }
    }
}
      
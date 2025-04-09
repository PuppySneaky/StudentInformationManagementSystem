using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentInformationManagementSystem.Controllers;
using StudentInformationManagementSystem.Data;
using StudentInformationManagementSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;
using StudentInformationManagementSystem.Tests.Helpers;

namespace StudentInformationManagementSystem.Tests.Controllers
{
    public class CourseControllerTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private readonly CourseController _controller;

        public CourseControllerTests()
        {
            // Setup in-memory database with a unique name for test isolation
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestCoursesDb_{Guid.NewGuid()}")
                .Options;

            // Clear any existing data to prevent PK conflicts
            using (var setupContext = new ApplicationDbContext(_options))
            {
                setupContext.Database.EnsureDeleted();
                setupContext.Database.EnsureCreated();
            }

            // Seed fresh test data
            SeedDatabase();

            // Create a new context for the controller
            var context = new ApplicationDbContext(_options);

            // Create controller with real context
            _controller = new CourseController(context);

            // Setup HttpContext for session
            var httpContext = new DefaultHttpContext();
            var mockSession = new MockHttpSession();
            mockSession.SetInt32("UserId", 1);
            mockSession.SetString("UserRole", "Admin");
            httpContext.Session = mockSession;

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
        }

        private void SeedDatabase()
        {
            using (var seedContext = new ApplicationDbContext(_options))
            {
                // Add test roles
                seedContext.Roles.Add(new Role { RoleId = 1, Name = "Admin", Description = "Administrator" });

                // Add test users
                seedContext.Users.Add(new User
                {
                    UserId = 1,
                    Username = "admin",
                    Email = "admin@example.com",
                    RoleId = 1,
                    IsActive = true,
                    PasswordHash = "hash",
                    Salt = "salt"
                });

                // Add test courses with Description
                seedContext.Courses.Add(new Course
                {
                    // Let the database assign IDs
                    CourseCode = "CS101",
                    CourseName = "Introduction to Programming",
                    Description = "Basic programming concepts",
                    CreditHours = 3,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                });

                seedContext.Courses.Add(new Course
                {
                    // Let the database assign IDs
                    CourseCode = "CS201",
                    CourseName = "Data Structures",
                    Description = "Advanced data structures",
                    CreditHours = 4,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                });

                seedContext.SaveChanges();
            }
        }

        [Fact]
        public async Task Index_ReturnsViewWithCourses()
        {
            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Course>>(viewResult.Model);
            Assert.Equal(2, model.Count());
            Assert.Contains(model, c => c.CourseCode == "CS101");
            Assert.Contains(model, c => c.CourseCode == "CS201");
        }

        [Fact]
        public void Create_Get_ReturnsView()
        {
            // Act
            var result = _controller.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Create_Post_ValidCourse_RedirectsToIndex()
        {
            // Arrange
            var newCourse = new Course
            {
                CourseCode = "CS301", // Use a different code to avoid conflicts
                CourseName = "Software Engineering",
                Description = "Software development methodologies",
                CreditHours = 3,
                IsActive = true
            };

            // Act
            var result = await _controller.Create(newCourse);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            // Verify course was added to database
            using (var verifyContext = new ApplicationDbContext(_options))
            {
                var savedCourse = await verifyContext.Courses.FirstOrDefaultAsync(c => c.CourseCode == "CS301");
                Assert.NotNull(savedCourse);
                Assert.Equal("Software Engineering", savedCourse.CourseName);
                Assert.Equal(3, savedCourse.CreditHours);
                Assert.True(savedCourse.IsActive);
                Assert.NotEqual(default(DateTime), savedCourse.CreatedDate);
            }
        }
    }
}
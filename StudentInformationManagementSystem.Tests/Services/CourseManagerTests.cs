using Xunit;
using Microsoft.EntityFrameworkCore;
using StudentInformationManagementSystem.Data;
using StudentInformationManagementSystem.Models;
using StudentInformationManagementSystem.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StudentInformationManagementSystem.Tests.Services
{
    public class CourseManagerTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public CourseManagerTests()
        {
            // Create unique database name for test isolation
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestCourseManager_{Guid.NewGuid()}")
                .Options;

            // Ensure database is clean for each test
            using (var context = new ApplicationDbContext(_options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }

        [Fact]
        public async Task AddCourseAsync_ValidCourse_ReturnsCourseId()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var courseManager = CourseManager.GetInstance(context);
                var newCourse = new Course
                {
                    CourseCode = "CS301",
                    CourseName = "Software Engineering",
                    Description = "Software development methodologies", // Make sure Description is set
                    CreditHours = 3,
                    IsActive = true
                };

                // Act
                var courseId = await courseManager.AddCourseAsync(newCourse);

                // Assert
                Assert.True(courseId > 0);

                // Verify course was added to database
                var savedCourse = await context.Courses.FindAsync(courseId);
                Assert.NotNull(savedCourse);
                Assert.Equal("CS301", savedCourse.CourseCode);
                Assert.Equal("Software Engineering", savedCourse.CourseName);
                Assert.Equal("Software development methodologies", savedCourse.Description);
                Assert.NotEqual(default(DateTime), savedCourse.CreatedDate);
            }
        }

        [Fact]
        public async Task GetAllCoursesAsync_ReturnsCoursesList()
        {
            // Arrange - Seed the database in this test method
            using (var contextForSeed = new ApplicationDbContext(_options))
            {
                contextForSeed.Courses.Add(new Course
                {
                    CourseCode = "CS101",
                    CourseName = "Introduction to Programming",
                    Description = "Basic programming concepts", // Make sure Description is set
                    CreditHours = 3,
                    IsActive = true,
                    CreatedDate = DateTime.Now.AddDays(-1)
                });

                contextForSeed.Courses.Add(new Course
                {
                    CourseCode = "CS201",
                    CourseName = "Data Structures",
                    Description = "Advanced data structures", // Make sure Description is set
                    CreditHours = 4,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                });

                await contextForSeed.SaveChangesAsync();
            }

            // Act - Use a fresh context for the test
            using (var contextForTest = new ApplicationDbContext(_options))
            {
                var courseManager = CourseManager.GetInstance(contextForTest);
                var courses = await courseManager.GetAllCoursesAsync();

                // Assert
                Assert.Equal(2, courses.Count);
                Assert.Contains(courses, c => c.CourseCode == "CS101");
                Assert.Contains(courses, c => c.CourseCode == "CS201");
            }
        }
    }
}
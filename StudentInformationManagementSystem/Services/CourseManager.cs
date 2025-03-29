using Microsoft.EntityFrameworkCore;
using StudentInformationManagementSystem.Data;
using StudentInformationManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentInformationManagementSystem.Services
{
    // This class implements the Singleton pattern to provide a single instance
    // of the CourseManager throughout the application
    public class CourseManager
    {
        // Singleton instance
        private static CourseManager _instance;

        // Lock object for thread safety
        private static readonly object _lock = new object();

        // Database context
        private readonly ApplicationDbContext _context;

        // Private constructor to prevent direct instantiation
        private CourseManager(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get the singleton instance
        public static CourseManager GetInstance(ApplicationDbContext context)
        {
            // Double-check locking pattern for thread safety
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new CourseManager(context);
                    }
                }
            }

            return _instance;
        }

        // Get all courses
        public async Task<List<Course>> GetAllCoursesAsync()
        {
            return await _context.Courses.ToListAsync();
        }

        // Get course by ID
        public async Task<Course> GetCourseByIdAsync(int id)
        {
            return await _context.Courses.FindAsync(id);
        }

        // Add a new course
        public async Task<int> AddCourseAsync(Course course)
        {
            // Set creation date
            course.CreatedDate = DateTime.UtcNow;

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return course.CourseId;
        }

        // Update an existing course
        public async Task UpdateCourseAsync(Course course)
        {
            _context.Entry(course).State = EntityState.Modified;

            // Do not modify the creation date
            _context.Entry(course).Property(x => x.CreatedDate).IsModified = false;

            await _context.SaveChangesAsync();
        }

        // Delete a course (set as inactive)
        public async Task DeleteCourseAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                course.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }

        // Hard delete a course (completely remove from database)
        public async Task HardDeleteCourseAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
        }

        // Check if a course code already exists
        public async Task<bool> CourseCodeExistsAsync(string courseCode, int? excludeCourseId = null)
        {
            if (excludeCourseId.HasValue)
            {
                return await _context.Courses.AnyAsync(c =>
                    c.CourseCode == courseCode && c.CourseId != excludeCourseId.Value);
            }

            return await _context.Courses.AnyAsync(c => c.CourseCode == courseCode);
        }

        // Get active courses
        public async Task<List<Course>> GetActiveCoursesAsync()
        {
            return await _context.Courses
                .Where(c => c.IsActive)
                .ToListAsync();
        }
    }
}
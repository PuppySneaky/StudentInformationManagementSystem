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
    // of the CourseAssignmentManager throughout the application
    public class CourseAssignmentManager
    {
        // Singleton instance
        private static CourseAssignmentManager _instance;

        // Lock object for thread safety
        private static readonly object _lock = new object();

        // Database context
        private readonly ApplicationDbContext _context;

        // Private constructor to prevent direct instantiation
        private CourseAssignmentManager(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get the singleton instance
        public static CourseAssignmentManager GetInstance(ApplicationDbContext context)
        {
            // Double-check locking pattern for thread safety
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new CourseAssignmentManager(context);
                    }
                }
            }

            return _instance;
        }

        // Get all student-course assignments
        public async Task<List<StudentCourse>> GetAllStudentCoursesAsync()
        {
            return await _context.StudentCourses
                .Include(sc => sc.Student)
                .Include(sc => sc.Course)
                .ToListAsync();
        }

        // Get assignments for a specific student
        public async Task<List<StudentCourse>> GetStudentCoursesAsync(int studentId)
        {
            return await _context.StudentCourses
                .Include(sc => sc.Course)
                .Where(sc => sc.StudentId == studentId)
                .ToListAsync();
        }

        // Get students enrolled in a specific course
        public async Task<List<StudentCourse>> GetCourseStudentsAsync(int courseId)
        {
            return await _context.StudentCourses
                .Include(sc => sc.Student)
                .Where(sc => sc.CourseId == courseId)
                .ToListAsync();
        }

        // Assign a course to a student
        public async Task<int> AssignCourseToStudentAsync(int studentId, int courseId)
        {
            // Check if the assignment already exists
            var existingAssignment = await _context.StudentCourses
                .FirstOrDefaultAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId);

            if (existingAssignment != null)
            {
                // If it exists but is inactive, activate it
                if (!existingAssignment.IsActive)
                {
                    existingAssignment.IsActive = true;
                    await _context.SaveChangesAsync();
                    return existingAssignment.StudentCourseId;
                }

                // Already exists and active
                return -1;
            }

            // Create a new assignment
            var studentCourse = new StudentCourse
            {
                StudentId = studentId,
                CourseId = courseId,
                EnrollmentDate = DateTime.Now,
                IsActive = true
            };

            _context.StudentCourses.Add(studentCourse);
            await _context.SaveChangesAsync();

            return studentCourse.StudentCourseId;
        }

        // Remove a course assignment (set as inactive)
        public async Task<bool> RemoveCourseAssignmentAsync(int studentCourseId)
        {
            var assignment = await _context.StudentCourses.FindAsync(studentCourseId);
            if (assignment == null)
            {
                return false;
            }

            assignment.IsActive = false;
            await _context.SaveChangesAsync();

            return true;
        }

        // Hard delete a course assignment
        public async Task<bool> DeleteCourseAssignmentAsync(int studentCourseId)
        {
            var assignment = await _context.StudentCourses.FindAsync(studentCourseId);
            if (assignment == null)
            {
                return false;
            }

            _context.StudentCourses.Remove(assignment);
            await _context.SaveChangesAsync();

            return true;
        }

        // Update a student's grade for a course
        public async Task<bool> UpdateStudentGradeAsync(int studentCourseId, string grade)
        {
            var assignment = await _context.StudentCourses.FindAsync(studentCourseId);
            if (assignment == null)
            {
                return false;
            }

            assignment.Grade = grade;
            await _context.SaveChangesAsync();

            return true;
        }

        // Get all students for assignment selection
        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _context.Students
                .Include(s => s.User)
                .Where(s => s.User.IsActive)
                .ToListAsync();
        }

        // Get all active courses for assignment selection
        public async Task<List<Course>> GetActiveCoursesAsync()
        {
            return await _context.Courses
                .Where(c => c.IsActive)
                .ToListAsync();
        }

        // Check if a student is enrolled in a course
        public async Task<bool> IsStudentEnrolledInCourseAsync(int studentId, int courseId)
        {
            return await _context.StudentCourses
                .AnyAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId && sc.IsActive);
        }
    }
}
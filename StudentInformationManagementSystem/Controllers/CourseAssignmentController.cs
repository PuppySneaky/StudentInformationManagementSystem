using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentInformationManagementSystem.Attributes;
using StudentInformationManagementSystem.Data;
using StudentInformationManagementSystem.Models;
using StudentInformationManagementSystem.Services;
using StudentInformationManagementSystem.ViewModels;
using System.Threading.Tasks;

namespace StudentInformationManagementSystem.Controllers
{
    [AuthorizeRoles("Admin")]
    public class CourseAssignmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CourseAssignmentManager _assignmentManager;

        public CourseAssignmentController(ApplicationDbContext context)
        {
            _context = context;
            // Get the singleton instance of CourseAssignmentManager
            _assignmentManager = CourseAssignmentManager.GetInstance(context);
        }

        // GET: CourseAssignment/Index
        public async Task<IActionResult> Index()
        {
            var assignments = await _assignmentManager.GetAllStudentCoursesAsync();
            return View(assignments);
        }

        // GET: CourseAssignment/AssignCourse
        public async Task<IActionResult> AssignCourse()
        {
            await PopulateDropdownsAsync();
            return View(new CourseAssignmentViewModel());
        }

        // POST: CourseAssignment/AssignCourse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignCourse(CourseAssignmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if student is already enrolled in this course
                if (await _assignmentManager.IsStudentEnrolledInCourseAsync(model.StudentId, model.CourseId))
                {
                    ModelState.AddModelError("", "This student is already enrolled in this course.");
                    await PopulateDropdownsAsync();
                    return View(model);
                }

                // Assign the course to the student
                var result = await _assignmentManager.AssignCourseToStudentAsync(model.StudentId, model.CourseId);

                if (result == -1)
                {
                    ModelState.AddModelError("", "This student is already enrolled in this course.");
                    await PopulateDropdownsAsync();
                    return View(model);
                }

                TempData["SuccessMessage"] = "Course assigned successfully!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdownsAsync();
            return View(model);
        }

        // GET: CourseAssignment/RemoveAssignment/5
        public async Task<IActionResult> RemoveAssignment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _context.StudentCourses
                .Include(sc => sc.Student)
                .Include(sc => sc.Course)
                .FirstOrDefaultAsync(sc => sc.StudentCourseId == id);

            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment);
        }

        // POST: CourseAssignment/RemoveAssignment/5
        [HttpPost, ActionName("RemoveAssignment")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveAssignmentConfirmed(int id)
        {
            await _assignmentManager.RemoveCourseAssignmentAsync(id);
            TempData["SuccessMessage"] = "Course assignment removed successfully!";
            return RedirectToAction(nameof(Index));
        }

        // Helper method to populate dropdowns
        private async Task PopulateDropdownsAsync()
        {
            // Get all active students
            var students = await _assignmentManager.GetAllStudentsAsync();
            ViewBag.Students = new SelectList(students, "StudentId", "FullName");

            // Get all active courses
            var courses = await _assignmentManager.GetActiveCoursesAsync();
            ViewBag.Courses = new SelectList(courses, "CourseId", "CourseName");
        }
    }
}
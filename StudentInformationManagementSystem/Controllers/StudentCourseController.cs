using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentInformationManagementSystem.Attributes;
using StudentInformationManagementSystem.Data;
using StudentInformationManagementSystem.Models;
using System.Threading.Tasks;

namespace StudentInformationManagementSystem.Controllers
{
    [AuthorizeRoles("Student")]
    public class StudentCourseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentCourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StudentCourse
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
                return RedirectToAction("Login", "Auth");

            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.UserId == userId.Value);

            if (student == null)
                return NotFound();

            var enrollments = await _context.StudentCourses
                .Include(sc => sc.Course)
                .Where(sc => sc.StudentId == student.StudentId && sc.IsActive)
                .ToListAsync();

            return View(enrollments);
        }

        // GET: StudentCourse/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
                return RedirectToAction("Login", "Auth");

            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.UserId == userId.Value);

            if (student == null)
                return NotFound();

            var studentCourse = await _context.StudentCourses
                .Include(sc => sc.Course)
                .FirstOrDefaultAsync(sc => sc.StudentCourseId == id && sc.StudentId == student.StudentId);

            if (studentCourse == null)
                return NotFound();

            var viewModel = new CourseViewModel
            {
                CourseId = studentCourse.CourseId,
                CourseCode = studentCourse.Course.CourseCode,
                CourseName = studentCourse.Course.CourseName,
                Description = studentCourse.Course.Description,
                CreditHours = studentCourse.Course.CreditHours,
                Grade = studentCourse.Grade ?? "Not Graded",
                EnrollmentDate = studentCourse.EnrollmentDate
            };

            return View(viewModel);
        }
    }
}

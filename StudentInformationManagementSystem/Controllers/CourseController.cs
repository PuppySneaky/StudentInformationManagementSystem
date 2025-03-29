using Microsoft.AspNetCore.Mvc;
using StudentInformationManagementSystem.Attributes;
using StudentInformationManagementSystem.Data;
using StudentInformationManagementSystem.Models;
using StudentInformationManagementSystem.Services;
using System.Threading.Tasks;

namespace StudentInformationManagementSystem.Controllers
{
    [AuthorizeRoles("Admin")]
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CourseManager _courseManager;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
            // Get the singleton instance of CourseManager
            _courseManager = CourseManager.GetInstance(context);
        }

        // GET: Course/Index
        public async Task<IActionResult> Index()
        {
            var courses = await _courseManager.GetAllCoursesAsync();
            return View(courses);
        }

        // GET: Course/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _courseManager.GetCourseByIdAsync(id.Value);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Course/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Course/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseCode,CourseName,Description,CreditHours,IsActive")] Course course)
        {
            if (ModelState.IsValid)
            {
                // Check if course code already exists
                if (await _courseManager.CourseCodeExistsAsync(course.CourseCode))
                {
                    ModelState.AddModelError("CourseCode", "This Course Code already exists.");
                    return View(course);
                }

                await _courseManager.AddCourseAsync(course);
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Course/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _courseManager.GetCourseByIdAsync(id.Value);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Course/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,CourseCode,CourseName,Description,CreditHours,CreatedDate,IsActive")] Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Check if course code already exists (excluding this course)
                if (await _courseManager.CourseCodeExistsAsync(course.CourseCode, course.CourseId))
                {
                    ModelState.AddModelError("CourseCode", "This Course Code already exists.");
                    return View(course);
                }

                await _courseManager.UpdateCourseAsync(course);
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Course/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _courseManager.GetCourseByIdAsync(id.Value);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _courseManager.DeleteCourseAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
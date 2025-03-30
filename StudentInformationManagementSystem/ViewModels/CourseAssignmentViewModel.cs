using System.ComponentModel.DataAnnotations;

namespace StudentInformationManagementSystem.ViewModels
{
    public class CourseAssignmentViewModel
    {
        [Required]
        [Display(Name = "Student")]
        public int StudentId { get; set; }

        [Required]
        [Display(Name = "Course")]
        public int CourseId { get; set; }
    }
}
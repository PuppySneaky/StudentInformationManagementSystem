using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentInformationManagementSystem.Models
{
    public class Student
    {
        public Student()
        {
            // Initialize collections
            StudentCourses = new HashSet<StudentCourse>();
        }

        [Key]
        public int StudentId { get; set; }

        // Foreign key to User
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(200)]
        public string Address { get; set; } = ""; // Default to empty string instead of null

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        // Student ID number assigned by the university
        [StringLength(20)]
        public string StudentNumber { get; set; } = ""; // Default to empty string instead of null

        // Enrollment date at the university
        public DateTime EnrollmentDate { get; set; }

        // Navigation property for course enrollments
        public virtual ICollection<StudentCourse> StudentCourses { get; set; }

        // Full name property for display purposes
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}
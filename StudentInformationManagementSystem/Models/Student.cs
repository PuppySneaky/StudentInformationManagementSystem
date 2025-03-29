using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentInformationManagementSystem.Models
{
    public class Student
    {
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

        [Required]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; } // Made nullable but required via validation

        [StringLength(200)]
        public string Address { get; set; } = ""; // Default to empty string instead of null

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        // Student ID number assigned by the university
        [StringLength(20)]
        public string StudentNumber { get; set; } = ""; // Default to empty string instead of null

        // Additional student-specific fields can be added here
        public DateTime EnrollmentDate { get; set; }
    }
}
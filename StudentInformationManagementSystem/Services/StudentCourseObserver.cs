using StudentInformationManagementSystem.Interfaces;
using System;

namespace StudentInformationManagementSystem.Services
{
    public class StudentCourseObserver : ICourseObserver
    {
        private readonly string _studentId;
        private readonly string _studentName;

        public StudentCourseObserver(string studentId, string studentName)
        {
            _studentId = studentId;
            _studentName = studentName;
        }

        public void UpdateCourseContent(string courseId, string contentType, string content)
        {
            Console.WriteLine($"Student {_studentName} (ID: {_studentId}) received update for course {courseId}");
            Console.WriteLine($"Content Type: {contentType}");
            Console.WriteLine($"Content: {content}");
        }
    }
}

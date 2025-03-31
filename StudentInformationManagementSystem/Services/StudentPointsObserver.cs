using StudentInformationManagementSystem.Interfaces;
using System;

namespace StudentInformationManagementSystem.Services
{
    public class StudentPointsObserver : IPointsObserver
    {
        private readonly string _studentId;
        private readonly string _studentName;

        public StudentPointsObserver(string studentId, string studentName)
        {
            _studentId = studentId;
            _studentName = studentName;
        }

        public void UpdatePoints(string studentId, string courseId, int points, string reason)
        {
            if (_studentId == studentId)
            {
                Console.WriteLine($"Student {_studentName} (ID: {studentId}) received {points} points for course {courseId}");
                Console.WriteLine($"Reason: {reason}");
            }
        }
    }
}

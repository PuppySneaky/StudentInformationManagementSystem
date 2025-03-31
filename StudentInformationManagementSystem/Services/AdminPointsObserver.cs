using StudentInformationManagementSystem.Interfaces;
using System;

namespace StudentInformationManagementSystem.Services
{
    public class AdminPointsObserver : IPointsObserver
    {
        public void UpdatePoints(string studentId, string courseId, int points, string reason)
        {
            Console.WriteLine($"Admin notified: Student {studentId} received {points} points for course {courseId}");
            Console.WriteLine($"Reason: {reason}");
        }
    }
}

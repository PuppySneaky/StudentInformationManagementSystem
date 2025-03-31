using StudentInformationManagementSystem.Interfaces;
using System.Collections.Generic;

namespace StudentInformationManagementSystem.Services
{
    public class PointsUpdateService
    {
        private List<IPointsObserver> _observers = new List<IPointsObserver>();

        public void Subscribe(IPointsObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IPointsObserver observer)
        {
            _observers.Remove(observer);
        }

        public void AwardPoints(string studentId, string courseId, int points, string reason)
        {
            foreach (var observer in _observers)
            {
                observer.UpdatePoints(studentId, courseId, points, reason);
            }
        }
    }
}

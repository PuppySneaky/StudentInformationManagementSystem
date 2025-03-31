using StudentInformationManagementSystem.Interfaces;
using System.Collections.Generic;

namespace StudentInformationManagementSystem.Services
{
    public class CourseUpdateService
    {
        private Dictionary<string, List<ICourseObserver>> _observers = new Dictionary<string, List<ICourseObserver>>();

        public void Subscribe(string courseId, ICourseObserver observer)
        {
            if (!_observers.ContainsKey(courseId))
            {
                _observers[courseId] = new List<ICourseObserver>();
            }
            _observers[courseId].Add(observer);
        }

        public void Unsubscribe(string courseId, ICourseObserver observer)
        {
            if (_observers.ContainsKey(courseId))
            {
                _observers[courseId].Remove(observer);
            }
        }

        public void NotifyCourseUpdate(string courseId, string contentType, string content)
        {
            if (_observers.ContainsKey(courseId))
            {
                foreach (var observer in _observers[courseId])
                {
                    observer.UpdateCourseContent(courseId, contentType, content);
                }
            }
        }
    }
}

namespace StudentInformationManagementSystem.Interfaces
{
    public interface ICourseObserver
    {
        void UpdateCourseContent(string courseId, string contentType, string content);
    }
}

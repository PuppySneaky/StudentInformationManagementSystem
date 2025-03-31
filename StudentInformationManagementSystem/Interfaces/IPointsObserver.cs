namespace StudentInformationManagementSystem.Interfaces
{
    public interface IPointsObserver
    {
        void UpdatePoints(string studentId, string courseId, int points, string reason);
    }
}

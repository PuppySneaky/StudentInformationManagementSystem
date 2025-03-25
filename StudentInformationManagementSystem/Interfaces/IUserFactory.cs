using StudentInformationManagementSystem.Models;

namespace StudentInformationManagementSystem.Interfaces
{
    public interface IUserFactory
    {
        Task<User> CreateUserAsync(string username, string email, string password, string roleName);

        Task<User> CreateStudentUserAsync(
            string username,
            string email,
            string password,
            string firstName,
            string lastName,
            string address = "",
            string phoneNumber = "",
            DateTime? dateOfBirth = null,
            string studentNumber = "");

        Task<User> CreateFacultyUserAsync(string username, string email, string password);

        Task<User> CreateAdminUserAsync(string username, string email, string password);
    }
}
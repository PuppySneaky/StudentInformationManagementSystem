using StudentInformationManagementSystem.Models;
using System.Threading.Tasks;

namespace StudentInformationManagementSystem.Interfaces
{
    public interface IUserFactory
    {
        Task<User> CreateUserAsync(string username, string email, string password, string roleName);
        Task<User> CreateStudentUserAsync(string username, string email, string password, string firstName, string lastName);
        Task<User> CreateFacultyUserAsync(string username, string email, string password);
        Task<User> CreateAdminUserAsync(string username, string email, string password);
    }
}
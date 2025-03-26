using StudentInformationManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentInformationManagementSystem.Interfaces
{
    public interface IAdminService
    {
        Task<User> CreateAdminAccountAsync(string username, string email, string password);
        Task<bool> IsFirstRunAsync();
    }
}
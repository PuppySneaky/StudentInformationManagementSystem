// Interfaces/ICourseRepository.cs
using StudentInformationManagementSystem.Models;

namespace StudentInformationManagementSystem.Interfaces
{
    public interface ICourseRepository
    {
        Task<Course> GetByIdAsync(int id);
        Task<IEnumerable<Course>> GetAllAsync();
    }
}

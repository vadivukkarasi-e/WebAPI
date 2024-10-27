using Company.API.Models.Domain;

namespace Company.API.Repositories.Interface
{
    public interface IEmployeeRepository
    {
        Task<Employee> CreateAsync(Employee employee);

        Task<IEnumerable<Employee>> GetAllAsync();

        Task<Employee?> GetByIdAsync(int id);

        Task<Employee?> UpdateAsync(Employee employee);

        Task<Employee?> DeleteAsync(int id);
    }
}

using Company.API.Data;
using Company.API.Models.Domain;
using Company.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Company.API.Repositories.Implementation
{
    public class EmployeeRepository : IEmployeeRepository
    {
        
        private readonly IApplicationDbContext dbContext;

        public EmployeeRepository(IApplicationDbContext context)
        {
            dbContext = context;
        }

        public async Task<Employee> CreateAsync(Employee employee)
        {
            await dbContext.Employees.AddAsync(employee);
            await dbContext.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee?> DeleteAsync(int id)
        {
            var existingEmployee = await dbContext.Employees.FirstOrDefaultAsync(x => x.EmployeeID == id);

            if (existingEmployee == null)
            {
                return null;
            }
            
            dbContext.Employees.Remove(existingEmployee);
            await dbContext.SaveChangesAsync();
            return existingEmployee;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await dbContext.Employees.ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await dbContext.Employees.FirstOrDefaultAsync(x => x.EmployeeID == id);
        }

        public async Task<Employee?> UpdateAsync(Employee employee)
        {
            var existingEmployee = await dbContext.Employees.FindAsync(employee.EmployeeID);

            if (existingEmployee == null)
            {
                return null;
            }

            // Update existing employee's values
            existingEmployee.Name = employee.Name;
            existingEmployee.Department = employee.Department;
            existingEmployee.Salary = employee.Salary;

            await dbContext.SaveChangesAsync();
            return existingEmployee;
        }
    }
}
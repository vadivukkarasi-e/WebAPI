using Company.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Company.API.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Employee> Employees { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

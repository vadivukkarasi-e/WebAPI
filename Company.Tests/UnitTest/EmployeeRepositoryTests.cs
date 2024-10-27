using Company.API.Data;
using Company.API.Models.Domain;
using Company.API.Repositories.Implementation;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Company.Tests.UnitTest
{
    [TestFixture]
    public class EmployeeRepositoryTests
    {
        private ApplicationDbContext _context;
        private EmployeeRepository _employeeRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique name for each test
           .Options;

            _context = new ApplicationDbContext(options);

            _employeeRepository = new EmployeeRepository(_context);

            // Seed the in-memory database with initial data
            SeedDatabase();
        }
        private void SeedDatabase()
        {
            // Add sample employees to the in-memory database
            _context.Employees.AddRange(new List<Employee>
                {
                    new Employee { EmployeeID = 1, Name = "Aadhav", Department = "HR", Salary = 50000 },
                    new Employee { EmployeeID = 2, Name = "Dhana", Department = "IT", Salary = 60000 }
                });

            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            // Clear the database after each test to ensure isolation
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetEmployeeByIdAsync_ShouldReturnEmployee_WhenEmployeeExists()
        {
            var employee = await _employeeRepository.GetByIdAsync(1);

            Assert.IsNotNull(employee);
            Assert.AreEqual("Aadhav", employee.Name);
        }

        [Test]
        public async Task CreateAsync_ShouldAddEmployee()
        {
            // Arrange
            var newEmployee = new Employee { Name = "Pranav", Department = "Finance", Salary = 70000 }; // No EmployeeID here

            // Act
            var result = await _employeeRepository.CreateAsync(newEmployee); // Call the service method to add

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo("Pranav"));
            Assert.That(result.Department, Is.EqualTo("Finance"));
            Assert.That(result.Salary, Is.EqualTo(70000));
            Assert.That(await _context.Employees.CountAsync(), Is.EqualTo(3)); // Check if one employee is in the context
        }

        [Test]
        public async Task DeleteAsync_ShouldReturnEmployee_WhenEmployeeExists()
        {
            // Arrange
            var employeeToDelete = new Employee { EmployeeID = 3, Name = "Pranav", Department = "Finance", Salary = 70000 };

            // First, add the employee to the context so it can be deleted
            await _employeeRepository.CreateAsync(employeeToDelete);

            // Act
            var result = await _employeeRepository.DeleteAsync(employeeToDelete.EmployeeID);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.EmployeeID, Is.EqualTo(employeeToDelete.EmployeeID));
            Assert.That(await _context.Employees.CountAsync(), Is.EqualTo(2)); // Expecting the count to be 0 after deletion
        }
        [Test]
        public async Task GetAllAsync_ShouldReturnAllEmployees()
        {
            var result = await _employeeRepository.GetAllAsync();

            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedEmployee_WhenEmployeeExists()
        {
            //var existingEmployee = new Employee { EmployeeID = 1, Name = "Aadhav", Department = "HR", Salary = 50000 };
            //await _employeeRepository.CreateAsync(existingEmployee); // Ensure the employee is created

            // Act: Create a new instance with updated values
            var updatedEmployee = new Employee { EmployeeID = 1, Name = "Aadhav Kumar", Department = "HR", Salary = 55000 };

            var result = await _employeeRepository.UpdateAsync(updatedEmployee); // Call the update method

            // Assert: Verify the result
            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo(updatedEmployee.Name));
            Assert.That(result.Salary, Is.EqualTo(updatedEmployee.Salary));

            // Verify the employee count remains the same
            Assert.That(await _context.Employees.CountAsync(), Is.EqualTo(2));
        }
    }
}

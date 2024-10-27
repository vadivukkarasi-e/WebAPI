using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Company.API.Data;
using Company.API.Models.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Company.API.Models.DTO;
using Company.API.Repositories.Implementation;
using Company.API.Repositories.Interface;
using System.Net;

namespace Company.Tests.IntegrationTest
{

    [TestFixture]
    public class EmployeesControllerTests
    {
        private WebApplicationFactory<Program> _factory; // Using Program for ASP.NET Core 6+
        private HttpClient _client;

        [SetUp]
        public void SetUp()
        {
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // Remove existing ApplicationDbContext registration
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                        if (descriptor != null) services.Remove(descriptor);

                        // Register ApplicationDbContext with InMemory database
                        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestDb");
                        });

                        // Register your repository
                        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
                    });
                });

            _client = _factory.CreateClient();
        }
        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        [Test]
        public async Task GetAllEmployees_ShouldReturnOk_WhenEmployeesExist()
        {
            // Arrange
            var employee1 = new Employee
            {
                EmployeeID = 1,
                Name = "Aadhav",
                Department = "IT",
                Salary = 60000
            };
            var employee2 = new Employee
            {
                EmployeeID = 2,
                Name = "Dana",
                Department = "HR",
                Salary = 55000
            };

            // Use the in-memory database context to seed data
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Employees.Add(employee1);
                context.Employees.Add(employee2);
                context.SaveChanges();
            }

            // Act
            var response = await _client.GetAsync("/api/employees");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var responseData = await response.Content.ReadAsStringAsync();
            var employees = JsonConvert.DeserializeObject<List<EmployeeDto>>(responseData);
            Assert.AreEqual(2, employees.Count);
            Assert.AreEqual("Aadhav", employees[0].Name);
            Assert.AreEqual("Dana", employees[1].Name);
        }

        //[Test]
        //public async Task GetAllEmployees_ShouldReturnOk_WhenNoEmployeesExist()
        //{
        //    // Act
        //    var response = await _client.GetAsync("/api/employees");

        //    // Assert
        //    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        //    var responseData = await response.Content.ReadAsStringAsync();
        //    var employees = JsonConvert.DeserializeObject<List<EmployeeDto>>(responseData);
        //    Assert.AreEqual(0, employees.Count);
        //}
    }
}
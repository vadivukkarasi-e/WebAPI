using Company.API.Models.Domain;
using Company.API.Models.DTO;
using Company.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Company.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository employeeRepository;

        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        // GET: {apibaseurl}/api/Employee
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await employeeRepository.GetAllAsync();

            // Convert Domain model to DTO
            var response = new List<EmployeeDto>();
            foreach (var employee in employees)
            {
                response.Add(new EmployeeDto
                {
                     EmployeeID = employee.EmployeeID,
                     Department = employee.Department,
                     Name = employee.Name,
                     Salary = employee.Salary   
                });
            }

            return Ok(response);
        }

        // POST: {apibaseurl}/api/employees
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Convert DTO to DOmain
            var employee = new Employee
            {
                Department = request.Department,
                Name = request.Name,
                Salary = request.Salary
            };

            employee = await employeeRepository.CreateAsync(employee);

            // Convert Domain Model back to DTO
            var response = new EmployeeDto
            {
                EmployeeID = employee.EmployeeID,
                Department = employee.Department,
                Name = employee.Name,
                Salary = employee.Salary
            };

            return Ok(response);
        }

        // PUT: {apibaseurl}/api/employees/{id}
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateemployeeById([FromRoute] int id, UpdateEmployeeRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Convert DTO to Domain Model
            var employee = new Employee
            {
                EmployeeID = id,
                Department = request.Department,
                Name = request.Name,
                Salary = request.Salary
            };

            // Call Repository To Update customer Domain Model
            var updatedemployee = await employeeRepository.UpdateAsync(employee);

            if (updatedemployee == null)
            {
                return NotFound();
            }

            // Convert Domain model back to DTO
            var response = new EmployeeDto
            {
                EmployeeID = employee.EmployeeID,
                Department = employee.Department,
                Name = employee.Name,
                Salary = employee.Salary
            };

            return Ok(response);
        }

        // DELETE: {apibaseurl}/api/employees/{id}
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Deleteemployee([FromRoute] int id)
        {
            var deletedemployee = await employeeRepository.DeleteAsync(id);

            if (deletedemployee == null)
            {
                return NotFound();
            }

            // Convert Domain model to DTO
            var response = new EmployeeDto
            {
                EmployeeID = deletedemployee.EmployeeID,
                Department = deletedemployee.Department,
                Name = deletedemployee.Name,
                Salary = deletedemployee.Salary
            };

            return Ok(response);
        }
    }
}

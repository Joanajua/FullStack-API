using FullStack.API.Data;
using FullStack.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FullStack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : Controller
    {
        private readonly FullStackDbContext _fullStackDbContext;

        public EmployeesController(FullStackDbContext fullStackDbContext)
        {
            _fullStackDbContext = fullStackDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _fullStackDbContext.Employees.ToListAsync();

            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employeeRequest)
        {
            employeeRequest.Id = Guid.NewGuid();
            await _fullStackDbContext.Employees.AddAsync(employeeRequest);
            await _fullStackDbContext.SaveChangesAsync();

            return Ok(employeeRequest);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetEmployeeById([FromRoute]Guid id)
        {
            var employee = await _fullStackDbContext.Employees.FirstOrDefaultAsync(e => e.Id == id);

            if (employee is null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] Guid id, [FromBody] Employee employeeRequest)
        {
            var employee = await _fullStackDbContext.Employees.FindAsync(id);

            if (employee is null)
            {
                return NotFound();
            }

            employee.Name = employeeRequest.Name;
            employee.Email = employeeRequest.Email;
            employee.Phone = employeeRequest.Phone;
            employee.Salary = employeeRequest.Salary;
            employee.Department = employeeRequest.Department;

            _fullStackDbContext.Employees.Update(employee);

            await _fullStackDbContext.SaveChangesAsync();

            return Ok(employee);
        }
    }
}

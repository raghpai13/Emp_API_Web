using EmpCrudOps.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmpCrudOps.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmpDbContext _dbContext;
        public EmployeeController(EmpDbContext dbContext) {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var employees = await _dbContext.Employees.ToListAsync();
            if (_dbContext.Employees == null)
            {
                return NotFound();
            }
            return employees;

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            if (_dbContext.Employees == null)
            {
                return NotFound();
            }
            var employee= await _dbContext.Employees.FindAsync(id);
            if (employee == null) {
                return NotFound();    
            }

            return employee;

        }
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee),new {id=employee.Id}, employee);
        }

        [HttpPut]
        public async Task<ActionResult<Employee>> PutEmployee(int id,Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }
            _dbContext.Entry(employee).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch(DbUpdateException)
            {
                throw;
            }
            return Ok();

        }
        [HttpDelete]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            if (_dbContext.Employees == null)
            {
                return NotFound();
            }
            var employee = await _dbContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("InvokeStoredProcedure")]
        public async Task<ActionResult<IEnumerable<Employee>>> InvokeStoredProcedure()
        {
            await _dbContext.Database.ExecuteSqlRawAsync("EXEC GetAllEmployees");

            return await _dbContext.Employees.ToListAsync();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController : Controller
    {
        private readonly EmployeeCrudContext _context;

        public EmployeesController(EmployeeCrudContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees.ToListAsync();
            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Employee empReq)
        {
            empReq.Id = Guid.NewGuid();
            await _context.Employees.AddAsync(empReq);
            await _context.SaveChangesAsync();
            return Ok(empReq);

        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var emp = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (emp == null)
                return NotFound();
            else
                return Ok(emp);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] Employee updatedEmp)
        {
            var emp = await _context.Employees.FindAsync(id);


            if (emp == null)
                return NotFound();
            else
            {
                emp.Name = updatedEmp.Name;
                emp.Email = updatedEmp.Email;
                emp.Phone = updatedEmp.Phone;
                emp.Salary = updatedEmp.Salary;
                emp.Depertment = updatedEmp.Depertment;
                await _context.SaveChangesAsync();

                return Ok(updatedEmp);
            }
        }
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var emp = await _context.Employees.FindAsync(id);

            if (emp == null)
                return NotFound();
            else
            {
                _context.Remove(emp);
                await _context.SaveChangesAsync();

                return Ok(emp);
            }
        }
    }
}

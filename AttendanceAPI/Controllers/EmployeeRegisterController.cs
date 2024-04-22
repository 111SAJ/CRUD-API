using AttendanceAPI.Data;
using AttendanceAPI.Model;
using AttendanceAPI.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AttendanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeRegisterController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ErrorResponseService _errorResponseService;

        public EmployeeRegisterController(ApplicationDbContext context, ErrorResponseService errorResponseService)
        {
            _context = context;
            _errorResponseService = errorResponseService;
        }

        //GET : List Employee
        [HttpGet("index")]
        public async Task<ActionResult<IEnumerable<EmployeeRegister>>> GetEmployee()
        {
            try
            {
                var EmployeeList = await _context.EmployeeRegister.ToListAsync();

                if (EmployeeList == null || EmployeeList.Count == 0)
                {
                    var errorResponse = _errorResponseService.CreateErrorResponse(400, "No Employee found");
                    return BadRequest(errorResponse);
                }

                var response = new
                {
                    Status = 200,
                    Message = "Action Performed Successfully",
                    Date = EmployeeList
                };

                return Created("", response);
            }
            catch (Exception)
            {
                var errorResponse = _errorResponseService.CreateErrorResponse(500, "Internal Server Error");
                return StatusCode(500, errorResponse);
            }
        }

        //POST : Register Employee
        [HttpPost("create")]
        public async Task<ActionResult<EmployeeRegister>> EmployeeRegister(EmployeeRegister employeeRegister)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Check if email already exist
                    var existEmployee = await _context.EmployeeRegister.FirstOrDefaultAsync(e => e.EmployeeEmail == employeeRegister.EmployeeEmail);
                    if (existEmployee != null)
                    {
                        var errorResponse = _errorResponseService.CreateErrorResponse(400, "Email already exist");
                        return BadRequest(errorResponse);
                    }

                    _context.EmployeeRegister.Add(employeeRegister);
                    await _context.SaveChangesAsync();

                    var response = new
                    {
                        Status = 200,
                        Message = "Action Performed Successfully",
                        Data = employeeRegister
                    };

                    return Created("", response);
                }
                return BadRequest(ModelState);
            }
            catch (Exception)
            {
                var errorResponse = _errorResponseService.CreateErrorResponse(500, "Internal Server Error");
                return StatusCode(500, errorResponse);
            }
        }

        //POST : Update Employee
        [HttpPost("update")]
        public async Task<ActionResult> UpdateEmployee (int employeeId, [FromBody] EmployeeRegister updateEmployee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existEmployee = await _context.EmployeeRegister.FindAsync(employeeId);

                    if (existEmployee == null)
                    {
                        var errorResponse = _errorResponseService.CreateErrorResponse(404, "No Employee found");
                        return BadRequest(errorResponse);
                    }

                    existEmployee.EmployeeName = updateEmployee.EmployeeName;
                    existEmployee.EmployeeEmail = updateEmployee.EmployeeEmail;
                    existEmployee.Password = updateEmployee.Password;
                    existEmployee.Address = updateEmployee.Address;

                    await _context.SaveChangesAsync();

                    var response = new
                    {
                        Status = 200,
                        Message = "Action Performed Successfully",
                        Date = existEmployee
                    };

                    return Created("", response);
                }
                return BadRequest(ModelState);
            }
            catch (Exception)
            {
                var errorResponse = _errorResponseService.CreateErrorResponse(500, "Internal Server Error");
                return StatusCode(500, errorResponse);
            }
        }

        //POST : Delete Employee
        [HttpPost("delete")]
        public async Task<ActionResult> DeleteEmployee(int employeeId)
        {
            try
            {
                var existEmployee = await _context.EmployeeRegister.FindAsync(employeeId);

                if (existEmployee == null)
                {
                    var errorResponse = _errorResponseService.CreateErrorResponse(404, "Employee not found");
                    return BadRequest(errorResponse);
                }

                _context.EmployeeRegister.Remove(existEmployee);
                await _context.SaveChangesAsync();

                var response = new
                {
                    Status = 200,
                    Message = "Action Performed Successfully",
                    Data = existEmployee
                };

                return Created("", response);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

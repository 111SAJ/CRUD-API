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
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ErrorResponseService _errorResponseService;

        public LoginController(ApplicationDbContext context, ErrorResponseService errorResponseService)
        {
            _context = context;
            _errorResponseService = errorResponseService;
        }

        //POST : Login
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    login.isLoggedIn = false;
                    var loggedIn = await _context.EmployeeRegister.FirstOrDefaultAsync(x => x.EmployeeEmail == login.EmployeeEmail && x.Password == login.Password);
                    if (loggedIn == null)
                    {
                        login.isLoggedIn = false;
                        var errorResponse = _errorResponseService.CreateErrorResponse(400, "Username or Password is invalid");
                        return BadRequest(errorResponse);
                    }

                    login.isLoggedIn = true;
                    var response = new
                    {
                        Status = 200,
                        Message = "Action Performed Successfully",
                        Data = new
                        {
                            login.EmployeeEmail,
                            login.isLoggedIn
                        }
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
    }
}

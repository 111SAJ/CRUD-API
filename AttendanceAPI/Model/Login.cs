namespace AttendanceAPI.Model
{
    public class Login
    {
        public string EmployeeEmail { get; set; }
        public string Password { get; set; }
        public bool isLoggedIn { get; set; }
    }
}

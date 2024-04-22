using AttendanceAPI.Model;
using AttendanceAPI.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace AttendanceAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        //Model
        public DbSet<EmployeeRegister> EmployeeRegister { get; set; }

        //ViewModel
        public DbSet<CustomeErrorResponseViewModel> CustomeErrorResponseViewModel { get; set; }
        public ApplicationDbContext(DbContextOptions options) : base(options) 
        { 

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomeErrorResponseViewModel>().HasNoKey();
        }
    }
}

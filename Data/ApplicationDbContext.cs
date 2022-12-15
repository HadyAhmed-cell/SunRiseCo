using Microsoft.EntityFrameworkCore;
using SunRiseCo.Models;

namespace SunRiseCo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }

        public DbSet<Branch> Branches { get; set; }
    }
}
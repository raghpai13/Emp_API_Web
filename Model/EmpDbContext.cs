using Microsoft.EntityFrameworkCore;

namespace EmpCrudOps.Model
{
    public class EmpDbContext: DbContext
    {

        public EmpDbContext(DbContextOptions<EmpDbContext> options):base(options) 
        {

        }
        public DbSet<Employee> Employees { get; set; } 

    }
}

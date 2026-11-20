using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Models
{
    public class StudentsDbContext : DbContext
    {
        public StudentsDbContext(DbContextOptions<StudentsDbContext>options) :base(options)
        {

        }
        public DbSet<Students> Students { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using TugceErciyesProject.Models;

namespace TugceErciyesProject
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> opts) : base(opts) { }
        public DbSet<CourseModel> Courses { get; set; }
    }
}
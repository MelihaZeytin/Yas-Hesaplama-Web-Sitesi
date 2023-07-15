using Microsoft.EntityFrameworkCore;
using windows_ödev.Models.Domain;

namespace windows_ödev.Data
{
    public class RazorPagesDemoDbContext : DbContext
    {
        public RazorPagesDemoDbContext()
        {
        }

        public RazorPagesDemoDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
    }
}

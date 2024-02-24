using Mango_api.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Mango> Mangoes { get; set; }

        
    }
}

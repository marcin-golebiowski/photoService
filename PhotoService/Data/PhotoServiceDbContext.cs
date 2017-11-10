using Microsoft.EntityFrameworkCore;
using PhotoService.Models;

namespace PhotoService.Data
{
    public class PhotoServiceDbContext : DbContext
    {
        public PhotoServiceDbContext(DbContextOptions<PhotoServiceDbContext> options) : base(options)
        {
        }

        public DbSet<Photo> Photos { get; set; }
    }
}

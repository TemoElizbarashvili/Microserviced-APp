using Microsoft.EntityFrameworkCore;
using platformService.Models;

namespace platformService.DataContext;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Platform> Platforms { get; set; }
}

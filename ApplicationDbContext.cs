using Microsoft.EntityFrameworkCore;
using Source.Models;
  
namespace Source
{  
    public class ApplicationDbContext : DbContext  
    {  
        public DbSet<Category> Category { get; set; }  
        public DbSet<Product> Product { get; set; }  
  
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)  
        {   
        }  
  
        protected override void OnModelCreating(ModelBuilder builder)  
        {
            builder.Entity<Category>().HasIndex(x => x.Name);
            builder.Entity<Product>().HasIndex(x => x.Name);
            builder.Entity<Product>().HasIndex(x => x.CategoryId);
        }
    }
}
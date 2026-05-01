using Microsoft.EntityFrameworkCore;
using SecureNode.Models;

namespace SecureNode.Data
{
    public class SecureDbContext : DbContext
    {
        public DbSet<CustomerPii> Customers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=secure_site.db");
        }
    }
}
using Microsoft.EntityFrameworkCore;
using PublicNode.Models;

namespace PublicNode.Data
{
    public class PublicDbContext : DbContext
    {
        public DbSet<PurchaseRecord> Purchases { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=public_site.db");
        }
    }
}
using KnowledgeHubPortal.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeHubPortal.Web.Models.Data
{
    public class KnowledgeHubPortalDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\mssqllocaldb;Initial Catalog=KnowledgeHubPortalDb2022;Integrated Security=True");
        }

        public DbSet<Catagory> Catagories { get; set; }
        public DbSet<Article> Articles { get; set; }
    }
}

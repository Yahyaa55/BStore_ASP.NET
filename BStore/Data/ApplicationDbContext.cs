using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BStore.Models;

namespace BStore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<BStore.Models.Product> Product { get; set; }
        public DbSet<BStore.Models.CartRow> CartRow { get; set; }
        public DbSet<BStore.Models.Cart> Cart { get; set; }
    }
}
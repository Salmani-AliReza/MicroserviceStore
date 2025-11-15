using Microsoft.EntityFrameworkCore;
using ProductService.Model.Entities;

namespace ProductService.Infrastructure.Contexts
{
    public class ProductDataBaseContext : DbContext
    {
        public ProductDataBaseContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}

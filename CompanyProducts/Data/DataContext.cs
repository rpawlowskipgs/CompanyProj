using Microsoft.EntityFrameworkCore;
using Models.Products.Models;

namespace CompanyProducts.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}

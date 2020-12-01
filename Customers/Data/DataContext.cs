using Microsoft.EntityFrameworkCore;
using Models.Customers.Models;

namespace Customers.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
    }
}

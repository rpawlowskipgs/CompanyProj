using Customers.Models;
using Microsoft.EntityFrameworkCore;

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

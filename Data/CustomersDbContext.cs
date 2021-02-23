using Microsoft.EntityFrameworkCore;
using SecuringWebApiUsingJwtAuthentication.Models;

namespace SecuringWebApiUsingJwtAuthentication.Data
{
    public class CustomersDbContext : DbContext
    {
        public CustomersDbContext(DbContextOptions<CustomersDbContext> options) : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }

    }
}
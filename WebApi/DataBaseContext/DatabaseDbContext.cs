using Microsoft.EntityFrameworkCore;
using WebApi.Entities;
using WebApi.Models;

namespace WebApi.DataBaseContext
{
    public class DatabaseDbContext : DbContext
    {

        public DatabaseDbContext(DbContextOptions<DatabaseDbContext> options)
        : base(options)
        {
        }

        public DbSet<Loan> Loan { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<LoanDocument> LoanDocument { get; set; }
        public DbSet<LoanHistory> LoanHistory { get; set; }
        public DbSet<User> Users { get; set; }
    }
}

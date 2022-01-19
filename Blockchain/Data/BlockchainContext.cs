using Blockchain.Models;
using Microsoft.EntityFrameworkCore;

namespace Blockchain.Data
{
    public class BlockchainContext : DbContext
    {
        public BlockchainContext(DbContextOptions<BlockchainContext> options) : base(options)
        { }

        public DbSet<BCUser> Users { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<Authority> Authoritys { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<BCApplication> BCApplications { get; set; }
        public DbSet<PermitApplication> Permits { get; set; }
        public DbSet<LoanApplication> Loans { get; set; }
        public DbSet<LoanDecision> LoanDecisions { get; set; }
        public DbSet<PermitDecision> PermitDecisions { get; set;}
        public DbSet<Property> Properties { get; set; } 
        public DbSet<BCFile> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
     
        }
    }
}

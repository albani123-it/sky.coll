using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using sky.coll.Entities;
using sky.coll.General;
using System.IO;
using System.Runtime;


namespace sky.coll.Insfrastructures
{
    public class dbConfig:DbContext
    {
        public DbSet<master_loan> master_loan{ get; set; }
        public DbSet<master_customer> master_customer { get; set; }
        private DbContextSettings _appsetting { get; set; }
        public dbConfig(IOptions<DbContextSettings> appsetting)
        {
            _appsetting = appsetting.Value;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //var builder = new ConfigurationBuilder()
            //     .SetBasePath(Directory.GetCurrentDirectory())
            //     .AddJsonFile("appsettings.json");
            //var config = builder.Build();
            //var Connstring= config.GetSection("DbContextSettings:ConnectionString_coll").Value.ToString();
            var SQLCons = "Host=103.53.197.67;Database=sky.coll;Username=postgres;Password=User123!";

            optionsBuilder.UseNpgsql(_appsetting.postgresql.ConnectionString_coll);

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<master_loan>()
              .HasKey(e => e.Id);
            modelBuilder.Entity<master_customer>()
            .HasKey(e => e.Id);

        }

    }
}

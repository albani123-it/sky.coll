using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using sky.coll.Entities;
using System.IO;


namespace sky.coll.Insfrastructures
{
    public class dbConfig:DbContext
    {
        public DbSet<master_loan> master_loan{ get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json");
            var config = builder.Build();
            var Connstring= config.GetSection("DbContextSettings:ConnectionString_coll").Value.ToString();
            //var SQLCons = "Host=103.53.197.67;Database=sky.coll;Username=postgres;Password=User123!";

            optionsBuilder.UseNpgsql(Connstring);

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<master_loan>()
              .HasKey(e => e.Id);


        }

    }
}

using Microsoft.EntityFrameworkCore;
using sky.coll.Entities;


namespace sky.coll.Insfrastructures
{
    public class dbConfig:DbContext
    {
        public DbSet<master_loan> master_loan{ get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var SQLCons = "Host=103.53.197.67;Database=sky.coll;Username=postgres;Password=User123!";

            optionsBuilder.UseNpgsql(SQLCons);

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<master_loan>()
              .HasKey(e => e.Id);


        }

    }
}

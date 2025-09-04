using Microsoft.EntityFrameworkCore;
using TicketsGeneratorServices.Db.Entities;


namespace TicketsGeneratorServices.Db.Context
{
    public class TicketsGeneratorDbContext : DbContext
    {
        public TicketsGeneratorDbContext(DbContextOptions<TicketsGeneratorDbContext> options)
            : base(options)
        { }


        public virtual DbSet<MyAwesomeProduct> MyAwesomeProducts { get; set; }



        public virtual DbSet<LogMyAwesomeProduct> LogMyAwesomeProducts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Configurations.MyAwesomeProductConfiguration());

            modelBuilder.ApplyConfiguration(new Configurations.LogMyAwesomeProductConfiguration());
        }
    }
}

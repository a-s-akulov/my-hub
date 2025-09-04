using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TicketsGeneratorServices.Db.Context;


namespace TicketsGeneratorServices.Db
{
    /// <summary>
    /// Фабрика для создания DbContext при создании миграций из Package Manager Console в DesignTime
    /// </summary>
    public class DesignTimeContextFactory : IDesignTimeDbContextFactory<TicketsGeneratorDbContext>
    {
        public TicketsGeneratorDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TicketsGeneratorDbContext>();
            optionsBuilder.UseNpgsql();

            return new TicketsGeneratorDbContext(optionsBuilder.Options);
        }
    }
}
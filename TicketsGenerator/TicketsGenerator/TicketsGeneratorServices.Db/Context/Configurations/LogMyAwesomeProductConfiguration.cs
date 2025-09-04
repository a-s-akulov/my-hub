using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketsGeneratorServices.Db.Context.Configurations.Base;
using TicketsGeneratorServices.Db.Entities;


namespace TicketsGeneratorServices.Db.Context.Configurations
{
    public class LogMyAwesomeProductConfiguration : LogEntityConfiguration<LogMyAwesomeProduct>
    {
        protected override void ConfigurePartial(EntityTypeBuilder<LogMyAwesomeProduct> entity)
        {
            entity.ToTable("log_my_awesome_products");
            entity.HasIndex(e => e.Id);

            MyAwesomeProductConfiguration.ConfigreAtomic(entity);
        }
    }
}

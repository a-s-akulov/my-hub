using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketsGeneratorServices.Db.Entities;


namespace TicketsGeneratorServices.Db.Context.Configurations
{
    public class MyAwesomeProductConfiguration : IEntityTypeConfiguration<MyAwesomeProduct>
    {
        public void Configure(EntityTypeBuilder<MyAwesomeProduct> entity)
        {
            entity.ToTable("my_awesome_products");
            entity.HasKey(e => e.Id);

            ConfigreAtomic(entity);

            entity.HasMany(e => e.Logs)
                .WithOne(e => e.Entity)
                .HasForeignKey(e => e.Id)
                .OnDelete(DeleteBehavior.NoAction);
        }


        public static void ConfigreAtomic(EntityTypeBuilder entityBuilder)
        {
            entityBuilder.Property(nameof(MyAwesomeProduct.Id)).HasColumnName("id");
            entityBuilder.Property(nameof(MyAwesomeProduct.Name)).HasColumnName("name");
            entityBuilder.Property(nameof(MyAwesomeProduct.ProductType)).HasColumnName("product_type");
        }
    }
}

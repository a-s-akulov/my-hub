using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketsGeneratorServices.Db.Entities.Base;


namespace TicketsGeneratorServices.Db.Context.Configurations.Base
{
    public abstract class LogEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class, ILogEntity
    {
        public void Configure(EntityTypeBuilder<TEntity> entity)
        {
            entity.HasKey(e => e.LogId);

            entity.Property(e => e.LogId).HasColumnName("log_id");
            entity.Property(e => e.ChangedDate).HasColumnName("changed_date");
            entity.Property(e => e.ChangedOperation).HasColumnName("changed_operation");

            ConfigurePartial(entity);
        }

        protected virtual void ConfigurePartial(EntityTypeBuilder<TEntity> entity)
        { }
    }
}

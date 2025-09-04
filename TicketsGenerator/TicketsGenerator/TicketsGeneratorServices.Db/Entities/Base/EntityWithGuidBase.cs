namespace TicketsGeneratorServices.Db.Entities.Base
{

    public abstract class EntityWithGuidBase : IEntityWithGuid
    {
        /// <inheritdoc/>
        public Guid Id { get; set; }
    }
}
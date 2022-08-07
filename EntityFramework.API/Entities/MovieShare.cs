using EntityFramework.API.Entities.EntityBase;

namespace EntityFramework.API.Entities
{
    public class MovieShare : BaseEntity
    {
        public long MovieId { get; set; }
        public long UserId { get; set; }
    }
}

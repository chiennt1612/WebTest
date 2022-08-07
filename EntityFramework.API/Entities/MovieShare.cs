using EntityFramework.API.Entities.EntityBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework.API.Entities
{
    public class MovieShare : BaseEntity
    {
        public long MovieId { get; set; }
        public long UserId { get; set; }
    }
}

using EntityFramework.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework.API.DBContext.Interfaces
{
    public interface IAppDbContext
    {
        public DbSet<Movies> Movies { get; set; }
        public DbSet<MovieShare> MoviesShares { get; set; }
    }
}

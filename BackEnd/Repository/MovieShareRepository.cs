using BackEnd.Repository.Interfaces;
using EntityFramework.API.DBContext;
using EntityFramework.API.Entities;

namespace BackEnd.Repository
{
    public class MovieShareRepository : GenericRepository<MovieShare, long>, IMovieShareRepository
    {
        private readonly AppDbContext _context;
        public MovieShareRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor)
            : base(dbContext, contextAccessor)
        {
            _context = dbContext;
        }
    }
}

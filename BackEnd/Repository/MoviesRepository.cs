using BackEnd.Repository.Interfaces;
using EntityFramework.API.DBContext;
using EntityFramework.API.Entities;

namespace BackEnd.Repository
{
    public class MoviesRepository : GenericRepository<Movies, long>, IMoviesRepository
    {
        private readonly AppDbContext _context;
        public MoviesRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor)
            : base(dbContext, contextAccessor)
        {
            _context = dbContext;
        }
    }
}

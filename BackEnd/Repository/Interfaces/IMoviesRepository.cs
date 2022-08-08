using EntityFramework.API.Entities;

namespace BackEnd.Repository.Interfaces
{
    public interface IMoviesRepository : IGenericRepository<Movies, long>    
    {
    }
}

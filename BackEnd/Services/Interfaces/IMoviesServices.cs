using BackEnd.Models;
using EntityFramework.API.Entities;
using EntityFramework.API.Entities.EntityBase;
using System.Linq.Expressions;

namespace BackEnd.Services.Interfaces
{
    public interface IMoviesServices
    {
        Task<bool> AddAsync(MovieShareModel model, long UserId, string Email);
        Task<Movies?> GetByIdAsync(long Id, long UserId);
        Task<BaseEntityList<Movies>?> GetListAsync(
            Expression<Func<Movies, bool>> expression,
            Func<Movies, object> sort, bool desc,
            int page, int pageSize);
        Task<IEnumerable<MovieShare>?> GetManyAsync(Expression<Func<MovieShare, bool>> where);
        Task Update(Movies entity);
    }
}

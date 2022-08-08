using BackEnd.Models;
using BackEnd.Repository.Interfaces;
using BackEnd.Services.Interfaces;
using EntityFramework.API.Entities;
using EntityFramework.API.Entities.EntityBase;
using System.Linq.Expressions;

namespace BackEnd.Services
{
    public class MoviesServices : IMoviesServices
    {
        private IUnitOfWork unitOfWork;
        private ILogger<MoviesServices> ilogger;
        public MoviesServices(IUnitOfWork unitOfWork, ILogger<MoviesServices> ilogger)
        {
            this.unitOfWork = unitOfWork;
            this.ilogger = ilogger;
        }
        public async Task<bool> AddAsync(MovieShareModel model, long UserId, string Email)
        {
            DateTime dateCreate = DateTime.Now;
            // check share if model.UserIds.HasValue is share with userids. else is publish (share all)
            bool isPublish = model.IsPublish;
            if (model.UserIds == null)
                isPublish = true;
            else
                if (model.UserIds.Count < 1) isPublish = true;
            if (!String.IsNullOrEmpty(model.Title))
            {
                if (!String.IsNullOrEmpty(model.Link))
                {
                    Movies movies = new Movies()
                    {
                        Description = model.Description,
                        IsDeleted = false,
                        IsPublish = isPublish,
                        Like = 0,
                        Link = model.Link,
                        Title = model.Title,
                        UnLike = 0,
                        DateCreator = dateCreate,
                        UserCreator = UserId,
                        EmailCreator= Email
                    };
                    var a = await unitOfWork.moviesRepository.AddAsync(movies);
                    await unitOfWork.SaveAsync();
                    if (model.UserIds != null && a != null)
                    {
                        if (model.UserIds.Count > 0)
                        {
                            List<MovieShare> movieShares = new List<MovieShare>();
                            foreach (var b in model.UserIds)
                            {
                                movieShares.Add(new MovieShare()
                                {
                                    DateCreator = dateCreate,
                                    IsDeleted = false,
                                    MovieId = a.Id,
                                    UserId = b,
                                    UserCreator = UserId                                    
                                });
                            }
                            await unitOfWork.movieShareRepository.AddManyAsync(movieShares);
                            await unitOfWork.SaveAsync();
                        }
                    }
                    if(a != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<Movies?> GetByIdAsync(long Id, long UserId)
        {
            try
            {
                var a = await unitOfWork.moviesRepository.GetByIdAsync(Id);
                if(a != null) // Permisstion check
                {
                    if (a.IsDeleted) return default;
                    if(a.IsPublish) return a;
                    // Check if Is owner && not is delete
                    if (a.UserCreator == UserId) return a;
                    // Check if shared && not is delete
                    Expression<Func<MovieShare, bool>> expression = u => (u.MovieId == Id && !u.IsDeleted && u.UserId == UserId);
                    var b = await unitOfWork.movieShareRepository.GetManyAsync(expression);
                    if (b != null) return a;
                }
                return default;
            }
            catch (Exception ex)
            {
                ilogger.LogError($"Get by id {Id.ToString()} Is Fail {ex.Message}");
                return default;
            }
        }

        public async Task<BaseEntityList<Movies>?> GetListAsync(Expression<Func<Movies, bool>> expression, Func<Movies, object> sort, bool desc, int page, int pageSize)
        {
            try
            {
                var a = await unitOfWork.moviesRepository.GetListAsync(expression, sort, desc, page, pageSize);
                if(ilogger != null) ilogger.LogInformation($"GetListAsync expression, sort {desc} {page} {pageSize}");
                return a;
            }
            catch (Exception ex)
            {
                if (ilogger != null) ilogger.LogError($"GetListAsync expression, sort {desc} {page} {pageSize} Is Fail {ex.Message}");
                return default;
            }
        }

        public async Task<IEnumerable<MovieShare>?> GetManyAsync(Expression<Func<MovieShare, bool>> where)
        {
            try
            {
                var a = await unitOfWork.movieShareRepository.GetManyAsync(where);
                if (ilogger != null) ilogger.LogInformation($"GetManyAsync");
                return a;
            }
            catch (Exception ex)
            {
                if (ilogger != null) ilogger.LogError($"GetManyAsync");
                return default;
            }
        }

        public async Task Update(Movies movie)
        {
            unitOfWork.moviesRepository.Update(movie);
            await unitOfWork.SaveAsync();
        }
    }
}

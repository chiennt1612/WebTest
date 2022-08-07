using BackEnd.Repository.Interfaces;
using EntityFramework.API.DBContext;

namespace BackEnd.Repository
{
    /// <inheritdoc />
    /// <summary>
    /// Unit of work class
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        #region inject field variables
        private readonly AppDbContext _appDbContext;
        #endregion

        #region data members

        private readonly IHttpContextAccessor _httpContext;
        private IMoviesRepository _MoviesRepository;
        private IMovieShareRepository _MovieShareRepository;
        #endregion

        /// <summary>
        /// Unit of work constructor
        /// </summary>
        /// <param name="appDbContext"></param>
        /// <param name="contextAccessor"></param>
        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public UnitOfWork(AppDbContext appDbContext, IHttpContextAccessor contextAccessor)
        {
            _appDbContext = appDbContext;
            _httpContext = contextAccessor;
        }
        #region Properties
        /// <summary>
        /// Get AppDbContext
        /// </summary>
        public AppDbContext AppDbContext => _appDbContext;

        /// <summary>
        /// Get UserRepository
        /// </summary>
        public IMoviesRepository moviesRepository
        {
            get
            {
                return _MoviesRepository = _MoviesRepository ?? new MoviesRepository(_appDbContext, _httpContext);
            }
        }

        public IMovieShareRepository movieShareRepository
        {
            get
            {
                return _MovieShareRepository = _MovieShareRepository ?? new MovieShareRepository(_appDbContext, _httpContext);
            }
        }

        /// <summary>
        /// Save
        /// </summary>
        public void Save()
        {
            _appDbContext.SaveChanges();
        }
        /// <summary>
        /// Save Async
        /// </summary>
        public async Task SaveAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
        #endregion

        #region dispose
        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _appDbContext.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}

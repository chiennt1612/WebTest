namespace BackEnd.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IMoviesRepository moviesRepository { get; }
        IMovieShareRepository movieShareRepository { get; }

        void Save();
        Task SaveAsync();
    }
}

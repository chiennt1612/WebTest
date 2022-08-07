using EntityFramework.API.Constants;
using EntityFramework.API.DBContext.Interfaces;
using EntityFramework.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework.API.DBContext
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public DbSet<Movies> Movies { get; set; }
        public DbSet<MovieShare> MoviesShares { get; set; }

        public AppDbContext(string connectionString) : base(GetOptions(connectionString))
        {
        }

        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureLogContext(builder);
        }

        private void ConfigureLogContext(ModelBuilder builder)
        {
            builder.Entity<Movies>(log =>
            {
                log.ToTable(TableConsts.Movies);
                log.HasKey(x => x.Id);
            });
            builder.Entity<MovieShare>(log =>
            {
                log.ToTable(TableConsts.MoviesShare);
                log.HasKey(x => x.Id);
            });
        }
    }
}

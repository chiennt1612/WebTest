using EntityFramework.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework.API.DBContext.Interfaces
{
    public interface IAppDbContext
    {
        public DbSet<Movies> Movies { get; set; }
        public DbSet<MovieShare> MoviesShares { get; set; }
    }
}

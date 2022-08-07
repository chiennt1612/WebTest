using BackEnd.Repository.Interfaces;
using BackEnd.Services;
using BackEnd.Services.Interfaces;
using EntityFramework.API.DBContext;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Repository;
using System.Linq.Expressions;
using EntityFramework.API.Entities;

namespace UnitTests
{
    [TestFixture]
    public class MoviesService_IsMoviesShould
    {
        private IMoviesServices _movieService;
        private IUnitOfWork unitOfWork;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = new UnitOfWork(new AppDbContext("Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=Movies"));
            _movieService = new MoviesServices(unitOfWork, null);
        }

        #region add function
        [Test]
        public async Task AddAsync_ReturnTrue()
        {
            var result = await _movieService.AddAsync(new BackEnd.Models.MovieShareModel()
            {
                Description = "Test Description",
                IsPublish = true,
                Link = "https://www.youtube.com/watch?v=fcN0BXzK8bg",
                Title = "Testing",
                UserIds = null
            }, 1, "chien@yahoo.com");

            Assert.IsTrue(result, "1 should not be prime");
        }

        [Test]
        public async Task AddAsync_ReturnFalse()
        {
            var result = await _movieService.AddAsync(new BackEnd.Models.MovieShareModel()
            {
                Description = "Test Description",
                IsPublish = true,
                Link = "",
                Title = "",
                UserIds = null
            }, 1, "chien@yahoo.com");

            Assert.IsFalse(result, "1 should not be prime");
        }

        [Test]
        public async Task AddAsync_More_ReturnTrue()
        {
            var result = await _movieService.AddAsync(new BackEnd.Models.MovieShareModel()
            {
                Description = "Test Description",
                IsPublish = false,
                Link = "https://www.youtube.com/watch?v=fcN0BXzK8bg",
                Title = "Testing",
                UserIds = new List<long>() { 1,2,3}
            }, 1, "chien@yahoo.com");

            Assert.IsTrue(result, "1 should not be prime");
        }

        [Test]
        public async Task AddAsync_More_ReturnFalse()
        {
            var result = await _movieService.AddAsync(new BackEnd.Models.MovieShareModel()
            {
                Description = "Test Description",
                IsPublish = true,
                Link = "",
                Title = "",
                UserIds = new List<long>() { 1, 2, 3 }
            }, 1, "chien@yahoo.com");

            Assert.IsFalse(result, "1 should not be prime");
        }
        #endregion

        #region get by id
        [Test]
        public async Task GetByIdAsync_ReturnTrue()
        {
            var result = await _movieService.GetByIdAsync(1, 1);

            Assert.IsNotNull(result, "1 should not be prime");
        }

        [Test]
        public async Task GetByIdAsync_ReturnFalse()
        {
            var result = await _movieService.GetByIdAsync(-1, 1);

            Assert.IsNull(result, "1 should not be prime");
        }
        #endregion

        #region get many by where codition
        [Test]
        public async Task GetManyAsync_ReturnTrue()
        {
            Expression<Func<MovieShare, bool>> sqlWhere = u => (!u.IsDeleted && u.UserId == 1);
            var result = await _movieService.GetManyAsync(sqlWhere);

            Assert.IsNotNull(result, "1 should not be prime");
        }
        #endregion

        #region getlist funtion
        [Test]
        public async Task GetListAsync_ReturnTrue()
        {
            Expression<Func<Movies, bool>> expression = u => (!u.IsDeleted && u.UserCreator == 1);
            Func<Movies, object> sort = u => u.Id;
            bool desc = true;
            int page = 1;
            int pageSize = 10;
            var result = await _movieService.GetListAsync(expression, sort, desc, page, pageSize);

            Assert.IsNotNull(result, "1 should not be prime");
        }
        #endregion
    }
}

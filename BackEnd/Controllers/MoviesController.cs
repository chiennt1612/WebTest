using BackEnd.ExceptionHandling;
using BackEnd.Models;
using BackEnd.Services.Interfaces;
using EntityFramework.API.Entities;
using EntityFramework.API.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Security.Claims;

namespace BackEnd.Controllers
{
    [Route("api/v1.0/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ControllerExceptionFilterAttribute))]
    [Produces("application/json", "application/problem+json")]
    [Authorize]
    public class MoviesController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<MoviesController> _logger;
        private readonly IMoviesServices _iMoviesServices;

        public MoviesController(
            ILogger<MoviesController> logger,
            UserManager<AppUser> userManager,
            IMoviesServices iMoviesServices)
        {
            _userManager = userManager;
            _iMoviesServices = iMoviesServices;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]/{getListBy}")]
        public async Task<IActionResult> List(GetListBy getListBy, int? page, int? pageSize)
        {
            bool IsMyVideo = ((int)getListBy == 0);
            int _Page = (page.HasValue ? page.Value : 1); if (_Page < 1) _Page = 1;
            int _PageSize = pageSize.HasValue ? pageSize.Value : 10; if (_PageSize < 10) _PageSize = 10;
            Func<Movies, object> sqlOrder = s => s.Id;
            var user = await GetCurrentUserAsync(HttpContext.User);
            IEnumerable<long> videoIds = new List<long>();
            if (!IsMyVideo) // Share video
            {
                Expression<Func<MovieShare, bool>> sqlWhere1 = u => (!u.IsDeleted && u.UserId == user.Id);
                videoIds = from a in (await _iMoviesServices.GetManyAsync(sqlWhere1))
                           select a.MovieId;
            }
            Expression<Func<Movies, bool>> sqlWhere = u => (
                (!u.IsDeleted) &&
                (IsMyVideo && u.UserCreator == user.Id) &&
                (!IsMyVideo && videoIds.Contains(u.Id))
            );
            var r = await _iMoviesServices.GetListAsync(sqlWhere, sqlOrder, true, _Page, _PageSize);
            _logger.LogInformation($"GetListAsync {getListBy}/{page}/{pageSize}");
            if (r == null)
                return Ok(new ResponseOK()
                {
                    Code = 404,
                    InternalMessage = Language.EntityValidation.Fail,
                    MoreInfo = Language.EntityValidation.Fail,
                    Status = 0,
                    UserMessage = Language.EntityValidation.Fail,
                    data = null
                });
            else
                return Ok(new ResponseOK()
                {
                    Code = 200,
                    InternalMessage = Language.EntityValidation.Success,
                    MoreInfo = Language.EntityValidation.Success,
                    Status = 1,
                    UserMessage = Language.EntityValidation.Success,
                    data = r
                });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Index(int? page, int? pageSize)
        {
            int _Page = (page.HasValue ? page.Value : 1); if (_Page < 1) _Page = 1;
            int _PageSize = pageSize.HasValue ? pageSize.Value : 10; if (_PageSize < 10) _PageSize = 10;
            Func<Movies, object> sqlOrder = s => s.Id;

            Expression<Func<Movies, bool>> sqlWhere = u => (
                (!u.IsDeleted) &&
                (u.IsPublish)
            );
            var r = await _iMoviesServices.GetListAsync(sqlWhere, sqlOrder, true, _Page, _PageSize);
            _logger.LogInformation($"Index/{page}/{pageSize}");
            if (r == null)
                return Ok(new ResponseOK()
                {
                    Code = 404,
                    InternalMessage = Language.EntityValidation.Fail,
                    MoreInfo = Language.EntityValidation.Fail,
                    Status = 0,
                    UserMessage = Language.EntityValidation.Fail,
                    data = null
                });
            else
                return Ok(new ResponseOK()
                {
                    Code = 200,
                    InternalMessage = Language.EntityValidation.Success,
                    MoreInfo = Language.EntityValidation.Success,
                    Status = 1,
                    UserMessage = Language.EntityValidation.Success,
                    data = r
                });
        }

        [HttpGet]
        [Route("[action]/{Id}")]
        public async Task<IActionResult> Details(long Id)
        {
            var user = await GetCurrentUserAsync(HttpContext.User);
            var movie = await _iMoviesServices.GetByIdAsync(Id, user.Id);

            _logger.LogInformation($"Details {Id}", $"Details {Id}");
            if (movie == null)
                return Ok(
                    new ResponseOK()
                    {
                        Code = 404,
                        InternalMessage = Language.EntityValidation.Fail,
                        MoreInfo = Language.EntityValidation.Fail,
                        Status = 0,
                        UserMessage = Language.EntityValidation.Fail,
                        data = null
                    });
            else
                return Ok(
                    new ResponseOK()
                    {
                        Code = 200,
                        InternalMessage = Language.EntityValidation.Success,
                        MoreInfo = Language.EntityValidation.Success,
                        Status = 1,
                        UserMessage = Language.EntityValidation.Success,
                        data = movie
                    });
        }

        [HttpGet]
        [Route("[action]/{Id}")]
        public async Task<IActionResult> Like(long Id)
        {
            var user = await GetCurrentUserAsync(HttpContext.User);
            var movie = await _iMoviesServices.GetByIdAsync(Id, user.Id);

            _logger.LogInformation($"Details {Id}", $"Details {Id}");
            if (movie == null)
                return Ok(
                    new ResponseOK()
                    {
                        Code = 404,
                        InternalMessage = Language.EntityValidation.Fail,
                        MoreInfo = Language.EntityValidation.Fail,
                        Status = 0,
                        UserMessage = Language.EntityValidation.Fail,
                        data = null
                    });
            else
            {
                movie.Like = movie.Like + 1;
                await _iMoviesServices.Update(movie);
                return Ok(
                    new ResponseOK()
                    {
                        Code = 200,
                        InternalMessage = Language.EntityValidation.Success,
                        MoreInfo = Language.EntityValidation.Success,
                        Status = 1,
                        UserMessage = Language.EntityValidation.Success,
                        data = movie
                    });
            }
        }

        [HttpGet]
        [Route("[action]/{Id}")]
        public async Task<IActionResult> UnLike(long Id)
        {
            var user = await GetCurrentUserAsync(HttpContext.User);
            var movie = await _iMoviesServices.GetByIdAsync(Id, user.Id);

            _logger.LogInformation($"Details {Id}", $"Details {Id}");
            if (movie == null)
                return Ok(
                    new ResponseOK()
                    {
                        Code = 404,
                        InternalMessage = Language.EntityValidation.Fail,
                        MoreInfo = Language.EntityValidation.Fail,
                        Status = 0,
                        UserMessage = Language.EntityValidation.Fail,
                        data = null
                    });
            else
            {
                movie.UnLike = movie.UnLike + 1;
                await _iMoviesServices.Update(movie);
                return Ok(
                    new ResponseOK()
                    {
                        Code = 200,
                        InternalMessage = Language.EntityValidation.Success,
                        MoreInfo = Language.EntityValidation.Success,
                        Status = 1,
                        UserMessage = Language.EntityValidation.Success,
                        data = movie
                    });
            }
        }

        private async Task<AppUser> GetCurrentUserAsync(ClaimsPrincipal user)
        {
            return await _userManager.GetUserAsync(user);
        }
    }
}

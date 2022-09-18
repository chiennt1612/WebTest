using BackEnd.Models;
using EntityFramework.API.Entities;
using EntityFramework.API.Entities.EntityBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Net;
using WebTest.Helper;
using WebTest.Models;

namespace WebTest.Controllers
{
    //[SecurityHeaders]
    public class HomeController : Controller
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<HomeController> _logger;
        private IConfiguration _configuration;
        public HomeController(IConfiguration configuration, ILogger<HomeController> logger, IDistributedCache cache)
        {
            this._logger = logger;
            this._configuration = configuration;
            this._cache = cache;
        }

        public async Task<IActionResult> Index(int? page)
        {
            string UserName = HttpContext.Session.GetString("UserName");
            if(!string.IsNullOrEmpty(UserName)) ViewData["Logined"] = HttpContext.Session.GetString("UserName");
            int _page = page.HasValue? page.Value : 1;if (_page < 1) _page = 1;
            MoviesList a = await APIRequest<MoviesList, string>(_logger, _configuration["APIConfig:URL"] + "api/v1.0/Movies/Index?page=" + _page.ToString() + "&pageSize=10", "", "", "GET");
            ViewData["Page"] = "0";
            if (a.Status == 1)
                return View(a.data);
            else
                return View(null);
        }

        public async Task<IActionResult> MyVideo(int? page)
        {
            string UserName = HttpContext.Session.GetString("UserName");
            if (!string.IsNullOrEmpty(UserName)) ViewData["Logined"] = HttpContext.Session.GetString("UserName");
            int _page = page.HasValue ? page.Value : 1; if (_page < 1) _page = 1;
            string Token = HttpContext.Session.GetString("Token");
            MoviesList a = await APIRequest<MoviesList, string>(_logger, _configuration["APIConfig:URL"] + "api/v1.0/Movies/List/0?page=" + _page.ToString() + "&pageSize=10", Token, "", "GET");
            ViewData["Page"] = "1";
            if (a.Status == 1)
                return View("Index", a.data);
            else
                return View("Index", null);
        }

        public async Task<IActionResult> ShareVideo(int? page)
        {
            string UserName = HttpContext.Session.GetString("UserName");
            if (!string.IsNullOrEmpty(UserName)) ViewData["Logined"] = HttpContext.Session.GetString("UserName");
            string Token = HttpContext.Session.GetString("Token");
            int _page = page.HasValue ? page.Value : 1; if (_page < 1) _page = 1;
            MoviesList a = await APIRequest<MoviesList, string>(_logger, _configuration["APIConfig:URL"] + "api/v1.0/Movies/List/1?page=" + _page.ToString() + "&pageSize=10", Token, "", "GET");
            ViewData["Page"] = "2";
            if (a.Status == 1)
                return View("Index", a.data);
            else
                return View("Index", null);
        }

        [HttpGet]
        public IActionResult Share()
        {
            string UserName = HttpContext.Session.GetString("UserName");
            if (!string.IsNullOrEmpty(UserName)) ViewData["Logined"] = HttpContext.Session.GetString("UserName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Share(MovieShareInput model)
        {
            string UserName = HttpContext.Session.GetString("UserName");
            if (!string.IsNullOrEmpty(UserName)) ViewData["Logined"] = HttpContext.Session.GetString("UserName");
            string Token = HttpContext.Session.GetString("Token");
            var userIds = new List<long>();
            if(model.UserId > 0) userIds.Add(model.UserId);
            MovieShareModel a = new MovieShareModel ()
            {
                Title = model.Title,
                Link = model.Link,
                Description = model.Description,
                IsPublish = false,
                UserIds = userIds
            };
            MovieShareResponseOK a1 = await APIRequest<MovieShareResponseOK, MovieShareModel>(_logger, _configuration["APIConfig:URL"] + "api/v1.0/Movies/Share", Token, a, "POST");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            ViewData["Login"] = "";
            LoginOutputModel a = await APIRequest<LoginOutputModel, LoginModel>(_logger, _configuration["APIConfig:URL"] + "api/v1.0/Authenticate/Register", "", loginModel, "POST");
            if (a.Status == 1)
            {
                HttpContext.Session.SetString("Token", a.data.Token);
                HttpContext.Session.SetString("RefreshToken", a.data.RefreshToken);
                HttpContext.Session.SetString("UserId", a.data.UserId.Value.ToString());
                HttpContext.Session.SetString("UserName", a.data.UserName);
                ViewData["Logined"] = a.data.UserName;
                ViewData["Page"] = "1";
                MoviesList a1 = await APIRequest<MoviesList, string>(_logger, _configuration["APIConfig:URL"] + "api/v1.0/Movies/List/0?page=1&pageSize=10", a.data.Token, "", "GET"); 
                return View("Index", a1.data);
            }                
            else
            {
                ViewData["Page"] = "0";
                ViewData["Login"] = $"Login false {a.InternalMessage}";
                MoviesList a1 = await APIRequest<MoviesList, string>(_logger, _configuration["APIConfig:URL"] + "api/v1.0/Movies/Index?page=1&pageSize=10", "", "", "GET");
                return View("Index", a1.data);
            }
                
        }

        private async Task<T> APIRequest<T, T1>(ILogger _logger, string APIUrl, string APIToken, T1 pzData, string Method = "POST")
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(APIUrl);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Method = Method;
            httpWebRequest.Headers.Add("Authorization", "Bearer " + APIToken);
            _logger.LogInformation($"APIUrl: {APIUrl}\n APIToken: {APIToken}\n");
            string result = await GetURL(httpWebRequest, _logger, pzData, Method);
            return JsonConvert.DeserializeObject<T>(result);
        }

        private async Task<string> GetURL<T>(HttpWebRequest httpWebRequest, ILogger _logger, T pzData, string Method = "POST")
        {
            string result = string.Empty;
            if (pzData != null && Method == "POST")
            {
                _logger.LogInformation($"pzData: {JsonConvert.SerializeObject(pzData)}");
                using (var streamWriter = new StreamWriter(await httpWebRequest.GetRequestStreamAsync()))
                {
                    string json = JsonConvert.SerializeObject(pzData);
                    await streamWriter.WriteAsync(json);
                    await streamWriter.FlushAsync();
                    streamWriter.Close();
                }
            }
            
            try
            {
                var httpResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = await streamReader.ReadToEndAsync();
                    _logger.LogInformation($"result: {result}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"result: {ex.ToString()}");
                result = "";
            }
            return result;
        }
    }
}

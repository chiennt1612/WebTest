using BackEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Net;
using WebTest.Helper;

namespace WebTest.Controllers
{
    [SecurityHeaders]
    [Authorize]
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
        [AllowAnonymous]
        public async Task<IActionResult> Index(int? page)
        {
            int _page = page.HasValue? page.Value : 1;if (_page < 1) _page = 1;
            ResponseOK a = await APIRequest(_logger, _configuration["APIConfig:URL"] + "v1.0/Movies/Index?page=" + _page.ToString() + "&pageSize=10", "", "");
            if (a.Status == 1)
                return View(a.data);
            else
                return View(null);
        }


        private async Task<ResponseOK> APIRequest<T1>(ILogger _logger, string APIUrl, string APIToken, T1 pzData)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(APIUrl);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("Authorization", "Bearer " + APIToken);
            _logger.LogInformation($"APIUrl: {APIUrl}\n APIToken: {APIToken}\n");
            string result = await GetURL(httpWebRequest, _logger, pzData);
            return JsonConvert.DeserializeObject<ResponseOK>(result);
        }

        private async Task<string> GetURL<T>(HttpWebRequest httpWebRequest, ILogger _logger, T pzData)
        {
            string result = string.Empty;
            if (pzData != null)
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

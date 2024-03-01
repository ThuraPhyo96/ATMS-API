using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace ATMS.Web.Mvc.Controllers
{
    public class BaseController : Controller
    {
        //public const string BaseUrl = "http://localhost:27162/api";
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        protected readonly string BaseUrl;

        public BaseController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient("ATMAPI");

            BaseUrl = _configuration.GetValue<string>("ApiUrl");
            //BaseUrl = _configuration.GetSection("ApiUrl").Value;
        }
    }
}

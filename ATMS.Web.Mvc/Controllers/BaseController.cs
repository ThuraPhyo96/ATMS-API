using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace ATMS.Web.Mvc.Controllers
{
    public class BaseController : Controller
    {
        public const string BaseUrl = "http://localhost:27162/api";
        private readonly HttpClient _httpClient;

        public BaseController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ATMAPI");
        }
    }
}

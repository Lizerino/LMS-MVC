using Lms.MVC.UI.Models.DTO;
using Lms.MVC.UI.Models.ViewModels.AuthorViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Lms.MVC.UI.Controllers
{
    public class AuthorsController : Controller
    {
        private HttpClient httpClient;

        public AuthorsController()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:44302/");
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<IActionResult> Index()
        {

            var request = new HttpRequestMessage(HttpMethod.Get, "api/authors");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            content = content.TrimStart('\"');
            content = content.TrimEnd('\"');
            content = content.Replace("\\", "");
            var authors = JsonConvert.DeserializeObject<List<IndexAuthorViewModel>>(content);

            return View(authors);

            //var client = new HttpClient();
            //var response = await client.GetAsync("https://localhost:44302/api/Authors");
            //var authors = await response.Content.ReadAsAsync<IEnumerable<AuthorDto>>();//TODO What is sent and what is recieved
            //return View(authors);
        }
    }
}

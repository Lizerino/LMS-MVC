using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Lms.API.Core.Entities;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

namespace Lms.MVC.UI.Controllers
{
    public class PublicationsController : Controller
    {
        string Baseurl = "https://localhost:44302/";

        public async Task<IActionResult> Index(string search)
        {
            List<Publication> publications = new List<Publication>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage Res = string.IsNullOrEmpty(search) ?
                    await client.GetAsync("api/Publications") :
                    await client.GetAsync("api/Publications/search/" + search);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var PublicationResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    publications = JsonConvert.DeserializeObject<List<Publication>>(PublicationResponse);
                    return View(publications);
                }
                else
                {
                    return View();
                }
            }
        }
    }
}


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
    public class Literatures : Controller
    {
        string Baseurl = "https://localhost:44302/";

        public async Task<IActionResult> Index()
        {
            List<Literature> literature = new List<Literature>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/Literatures");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var LiteratureResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    literature = JsonConvert.DeserializeObject<List<Literature>>(LiteratureResponse);
                    return View(literature);
                }
                else
                {
                    return View();
                }
            }
        }
    }
}


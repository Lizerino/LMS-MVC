using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Lms.API.Core.Entities;
using Lms.MVC.UI.Filters;
using Lms.MVC.UI.Utilities.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

namespace Lms.MVC.UI.Controllers
{
    public class PublicationsController : Controller
    {
        string Baseurl = "https://localhost:44302/";

        [Authorize]
        public async Task<IActionResult> Index(string search, string sortOrder, int page)
        {
            if (search != null)
            {
                page = 1;
            }

            ViewData["CurrentFilter"] = search;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["SubjectSortParm"] = sortOrder == "subject" ? "subject_desc" : "subject";
            ViewData["AuthorSortParm"] = sortOrder == "author" ? "author_desc" : "author";

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

                    switch (sortOrder)
                    {
                        case "title_desc":
                            publications = publications.OrderByDescending(p => p.Title).ToList();
                            break;
                        case "subject":
                            publications = publications.OrderBy(p => p.Subject.Title).ToList();
                            break;
                        case "subject_desc":
                            publications = publications.OrderByDescending(p => p.Subject.Title).ToList();
                            break;
                        case "author":
                            publications = publications
                                .OrderBy(p => p.Authors.FirstOrDefault() != null ?
                                p.Authors.FirstOrDefault().LastName : "").ToList();
                            break;
                        case "author_desc":
                            publications = publications
                                .OrderByDescending(p => p.Authors.FirstOrDefault() != null ?
                                p.Authors.FirstOrDefault().LastName : "").ToList();
                            break;
                        default:
                            publications = publications.OrderBy(p => p.Title).ToList();
                            break;
                    }

                    var paginatedResult = publications.AsQueryable().GetPagination(page, 10);

                    return View(paginatedResult);
                }
                else
                {
                    return View();
                }
            }
        }

        [HttpGet]
        [Authorize][ModelNotNull]
        public async Task<IActionResult> Edit(int? id)
        {
            using (var client = new HttpClient())
            {
                var publication = new Publication();
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = 
                    await client.GetAsync("api/Publications/" + id);

                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var PublicationResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    publication = JsonConvert.DeserializeObject<Publication>(PublicationResponse);

                    return View(publication);
                }
                else
                {
                    return NotFound();
                }
            }
        }


        [HttpPut]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Publication publication)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res =
                    await client.PutAsJsonAsync<Publication>("api/Publications" + id, publication);

                if (Res.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View();
                }
                
            }
        }
    }
}


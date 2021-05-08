﻿using AutoMapper;
using Lms.API.Core.Entities;
using Lms.MVC.Core.Repositories;
using Lms.MVC.UI.Filters;
using Lms.MVC.UI.Models.ViewModels.PublicationViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Lms.MVC.UI.Controllers
{
    public class PublicationsController : Controller
    {
        string Baseurl = "https://localhost:44302/";
        private readonly IMapper mapper;
        private readonly IUoW uow;

        public PublicationsController(IMapper mapper, IUoW uow)
        {
            this.mapper = mapper;
            this.uow = uow;
        }

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

        [HttpGet]
        public ActionResult Create()
        {
            var model = new CreatePublicationViewModel();
            model.Subjects = uow.PublicationRepository.GetSubjects();
            
            return  View(model);
        }


        [HttpPost]
        [ModelValid, ModelNotNull]
        public async Task<IActionResult> Create(CreatePublicationViewModel model)
        {
            model.Authors = new List<Author>();
            model.Authors.Add(uow.PublicationRepository.CreateAuthor(model.AuthorFirstName, model.AuthorLastName));
            model.Subject = uow.PublicationRepository.CreateSubject(model.SubjectTitle);
            //model.Author = new Author() { FirstName = model.AuthorFirstName, LastName = model.AuthorFirstName }; //TODO MOVE TO EXTENSION
            //model.Subject = new Subject() { Title = model.SubjectTitle }; //TODO MOVE TO EXTENSION

            
            mapper.Map<Publication>(model);//TODO Fix Mapping issue
            
            using (var client = new HttpClient())
            {
                // Building request url
                client.BaseAddress = new Uri(Baseurl);

                // Fixing request head
                client.DefaultRequestHeaders.Clear();
                // Define request format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                

                // Build Request
                var jsonData = JsonConvert.SerializeObject(model);
                var url = Baseurl + "api/Publications/create";


                var response = await client.PostAsJsonAsync(url, jsonData);
                response.EnsureSuccessStatusCode();

                return RedirectToAction("Index");

            }
        }
        
    }
}


﻿using AutoMapper;
using Lms.MVC.Core.Repositories;
using Lms.MVC.UI.Filters;
using Lms.MVC.UI.Models.ViewModels.ApplicationUserViewModels;
using Lms.MVC.UI.Utilities.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lms.MVC.UI.Controllers
{
    [Authorize(Roles = "Teacher,Admin")]
    public class ApplicationUsersController : Controller
    {
        private readonly ILogger<ApplicationUsersController> _logger;

        private readonly IUoW uoW;

        private readonly IMapper mapper;

        public ApplicationUsersController(

            ILogger<ApplicationUsersController> logger,
            IEmailSender emailSender, IUoW uoW, IMapper mapper)
        {
            _logger = logger;

            this.uoW = uoW;
            this.mapper = mapper;
        }

        // GET: ApplicationUsersController
        public async Task<IActionResult> Index(string search, string sortOrder, int page)
        {
            if (search != null)
            {
                page = 1;
            }

            var users = await uoW.UserRepository.GetAllUsersAsync();

            var model = mapper.Map<IEnumerable<ListApplicationUsersViewModel>>(users);

            // Add to get the search to work
            if (!String.IsNullOrWhiteSpace(search))
            {
                model = model.Where(u => u.Name.ToLower().StartsWith(search.ToLower()) || u.Email.ToLower().Contains(search.ToLower()) || u.Role.ToLower().Trim() == search.ToLower());
            }

            ViewData["CurrentFilter"] = search;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewData["EmailSortParm"] = sortOrder == "Email" ? "Email_desc" : "Email";
            ViewData["RoleSortParm"] = sortOrder == "Email" ? "Role_desc" : "Role";

            switch (sortOrder)
            {
                case "Name_desc":
                    model = model.OrderByDescending(s => s.Name);
                    break;

                case "Email":
                    model = model.OrderBy(s => s.Email);
                    break;

                case "Email_desc":
                    model = model.OrderByDescending(s => s.Email);
                    break;

                case "Role":
                    model = model.OrderBy(s => s.Role);
                    break;

                case "Role_desc":
                    model = model.OrderByDescending(s => s.Role);
                    break;

                default:
                    model = model.OrderBy(s => s.Name);
                    break;
            }

            var paginatedResult = model.AsQueryable().GetPagination(page, 10);

            return View(paginatedResult);
        }

        // GET: ApplicationUsersController/Details/5

        [ModelNotNull]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await uoW.UserRepository.FindAsync(id, true);
           
            var model = mapper.Map<DetailsApplicationUserViewModel>(user);

            return View(model);
        }
        [ModelNotNull]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await uoW.UserRepository.FindAsync(id, true);

            var model = mapper.Map<EditApplicationUserViewModel>(user);


            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(string id, EditApplicationUserViewModel viewmodel)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await uoW.UserRepository.FindAsync(id, true);
            viewmodel.Courses = user.Courses;
            if (user.Email.ToLower().Contains("admin@lms.se"))
            {
                ModelState.AddModelError("Email", "You can't edit the admin");
            }

            if (await TryUpdateModelAsync(viewmodel, "", u => u.Name,
                                                    u => u.Email,
                                                    u => u.Role,
                                                    user => user.PhoneNumber,
                                                    user => user.Courses))
                if (viewmodel.Role is null)
                {
                    ModelState.AddModelError("Role", "Choose a role");
                }
            if (viewmodel.Role != "Admin" && viewmodel.Role != "Teacher" && viewmodel.Role != "Student")
            {
                ModelState.AddModelError("Role", "Choose a valid role");
            }
            if (ModelState.IsValid)
            {
                mapper.Map(viewmodel, user);

                await uoW.UserRepository.ChangeRoleAsync(user);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await uoW.CompleteAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(viewmodel);
        }

        
        private bool UserExists(string id)
        {
            return uoW.UserRepository.Any(id);
        }
        [ModelNotNull]
        public async Task<IActionResult> Remove(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userToBeRemoved = await uoW.UserRepository.FindAsync(id, true);
            
            var model = mapper.Map<DeleteApplicationUserViewModel>(userToBeRemoved);

            return View(model);
        }

        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        [ModelNotNull]
        public async Task<IActionResult> RemoveConfirmed(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userToBeRemoved = await uoW.UserRepository.FindAsync(id, true);
            
            uoW.UserRepository.Remove(userToBeRemoved);
            await uoW.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> EmailExistsEdit(string email, string id)
        {
            var users = await uoW.UserRepository.GetAllUsersAsync();
            if (users.Any(u => email == u.Email && id != u.Id))
            {
                return Json($"Email already exits");
            }
            return Json(true);
        }

        public async Task<IActionResult> EmailExistsCreate([Bind(Prefix = "Input.Email")] string email)
        {
            var users = await uoW.UserRepository.GetAllUsersAsync();
            if (users.Any(u => email == u.Email))
            {
                return Json($"Email already exits");
            }
            return Json(true);
        }
    }
}
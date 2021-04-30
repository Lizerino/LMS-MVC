﻿using AutoMapper;
using Lms.MVC.Core.Repositories;
using Lms.MVC.UI.Models.ViewModels;
using Lms.MVC.UI.Models.ViewModels.ApplicationUserViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lms.MVC.UI.Controllers
{
    public class ApplicationUsersController : Controller
    {
        //private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ApplicationUsersController> _logger;

        //private readonly IEmailSender _emailSender;
        private readonly IUoW uoW;

        private readonly IMapper mapper;

        public ApplicationUsersController(
            // UserManager<ApplicationUser> userManager,
            //SignInManager<ApplicationUser> signInManager,
            ILogger<ApplicationUsersController> logger,
            IEmailSender emailSender, IUoW uoW, IMapper mapper)
        {
            //  _userManager = userManager;
            //_signInManager = signInManager;
            _logger = logger;
            // _emailSender = emailSender;
            this.uoW = uoW;
            this.mapper = mapper;
        }

        // GET: ApplicationUsersController
        public async Task<IActionResult> Index()
        {
            var users = await uoW.UserRepository.GetAllUsersAsync();

            var model = mapper.Map<IEnumerable<ApplicationUsersListViewModel>>(users);

            return View(model);
        }

        // GET: ApplicationUsersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ApplicationUsersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApplicationUsersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ApplicationUsersController/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await uoW.UserRepository.FindAsync(id, true);

            var model = mapper.Map<ApplicationUserEditViewModel>(user);

            if (user == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: ApplicationUsersController/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(string id , ApplicationUserEditViewModel viewmodel)
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
                if ( viewmodel.Role is null)
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



            // ToDo: Connect the role proporty with the role manager.

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

        // GET: ApplicationUsersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ApplicationUsersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
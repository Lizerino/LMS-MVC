﻿using Lms.MVC.Core.Entities;
using Lms.MVC.Core.Repositories;
using Lms.MVC.Data.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.MVC.Data.Repositories
{
  public  class UoW : IUoW
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        public ICourseRepository CourseRepository { get; }
        public IModuleRepository ModuleRepository { get; }
        public IActivityRepository ActivityRepository { get; }
        public IUserRepository UserRepository { get; }

        public UoW(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
            CourseRepository = new CourseRepository(this.db);
            ActivityRepository = new ActivityRepository(this.db);
            ModuleRepository = new ModuleRepository(this.db);
            UserRepository = new UserRepository(this.db, this.userManager);
        }

        public async Task CompleteAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}

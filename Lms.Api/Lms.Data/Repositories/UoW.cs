using Lms.API.Core.Repositories;
using Lms.MVC.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.API.Data.Repositories
{
  public  class UoW : IUoW
    {
        private readonly ApplicationDbContext db;
        public ICourseRepository CourseRepository { get; }

        public IModuleRepository ModuleRepository { get; }

        public UoW(ApplicationDbContext db)
        {
            this.db = db;
            CourseRepository = new CourseRepository(db);
            ModuleRepository = new ModuleRepository(db);
        }

        public async Task CompleteAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}

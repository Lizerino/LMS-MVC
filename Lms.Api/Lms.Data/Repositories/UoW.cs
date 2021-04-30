using Lms.API.Core.Repositories;
using Lms.API.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.API.Data.Repositories
{
  public  class UoW : IUoW
    {
        private readonly LmsAPIContext db;
        public ICourseRepository CourseRepository { get; }

        public IModuleRepository ModuleRepository { get; }

        public ILiteratureRepository LiteratureRepository { get; }

        public IAuthorRepository AuthorRepository { get; }

        public UoW(LmsAPIContext db)
        {
            this.db = db;
            CourseRepository = new CourseRepository(db);
            ModuleRepository = new ModuleRepository(db);
            LiteratureRepository = new LiteratureRepository(db);
            AuthorRepository = new AuthorRepository(db);
        }

        public async Task CompleteAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}

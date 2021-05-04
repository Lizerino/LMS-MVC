using Lms.API.Core.Repositories;
using Lms.API.Data.Data;
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
        private readonly LmsAPIContext db;

        public ILiteratureRepository LiteratureRepository { get; }

        public IAuthorRepository AuthorRepository { get; }

        public UoW(LmsAPIContext db)
        {
            this.db = db;
            LiteratureRepository = new LiteratureRepository(db);
            AuthorRepository = new AuthorRepository(db);
        }

        public async Task CompleteAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}

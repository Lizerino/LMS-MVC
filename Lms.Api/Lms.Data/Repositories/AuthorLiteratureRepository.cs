using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lms.API.Core.Entities;
using Lms.API.Core.Repositories;
using Lms.API.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace Lms.API.Data.Repositories
{
    class AuthorLiteratureRepository : IAuthorLiteratureReporitory
    {
        private readonly LmsAPIContext db;

        public AuthorLiteratureRepository(LmsAPIContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<Author>> GetAuthors(int? literatureId)
        {
            return await db.Authorship
                .Include(l => l.Author)
                .Where(l => l.LiteratureId == literatureId)
                .Select(l => l.Author).ToListAsync();
        }

        public async Task<IEnumerable<Literature>> GetBibliography(int? authorId)
        {
            return await db.Authorship
                .Include(a => a.Literature)
                .Where(a => a.AuthorId == authorId)
                .Select(a => a.Literature).ToListAsync();
        }

        public void Add(Author author, Literature literature)
        {
            db.Add(new AuthorLiterature
            {
                AuthorId = author.Id,
                Author = author,
                LiteratureId = literature.Id,
                Literature = literature
            });
        }

        public void Remove(AuthorLiterature authorship)
        {
            db.Remove(authorship);
        }
    }
}

using Lms.API.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.API.Core.Repositories
{
    public interface IAuthorLiteratureReporitory
    {
        public Task<IEnumerable<Author>> GetAuthors(int? literatureId);
        public Task<IEnumerable<Literature>> GetBibliography(int? authorId);
        public void Add(Author author, Literature literature);
        public void Remove(AuthorLiterature authorship);
    }
}

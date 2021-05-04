using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.API.Core.Repositories
{
    public interface IUoW
    {
        ILiteratureRepository LiteratureRepository { get; }
        IAuthorRepository AuthorRepository { get; }
        Task CompleteAsync();
    }
}

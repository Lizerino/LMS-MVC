using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lms.API.Core.Entities;

namespace Lms.API.Core.Repositories
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAllAuthorsAsync();
        Task<Author> GetAuthorAsync(int? id);
        Task<IEnumerable<Author>> GetAuthorByNameAsync(string name);
        Task<bool> SaveAsync();
        Task AddAsync<T>(T added);
        void Remove(Author removed);
    }
}

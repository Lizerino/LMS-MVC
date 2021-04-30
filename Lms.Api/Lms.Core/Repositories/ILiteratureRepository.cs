using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lms.API.Core.Entities;

namespace Lms.API.Core.Repositories
{
    public interface ILiteratureRepository
    {
        Task<IEnumerable<Literature>> GetAllLiteratureAsync();
        Task<Literature> GetLiteratureAsync(int? id);
        Task<IEnumerable<Literature>> GetLiteratureByTitleAsync(string title);
        Task<bool> SaveAsync();
        Task AddAsync<T>(T added);
        void Remove(Literature removed);
    }
}

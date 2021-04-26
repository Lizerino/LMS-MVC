using Lms.API.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.API.Core.Repositories
{
    public interface IModuleRepository
    {

        Task<IEnumerable<Module>> GetAllModulesAsync(int id);
        Task<Module> GetModuleAsync(int id, int moduleId);
        Task<Module> GetModuleByTitleAsync(int id, string title);
        Task<bool> SaveAsync();
        Task AddAsync<T>(T added);
        void Remove(Module removed);
    }
}

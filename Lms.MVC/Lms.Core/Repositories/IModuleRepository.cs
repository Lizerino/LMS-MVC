using System.Collections.Generic;
using System.Threading.Tasks;

using Lms.MVC.Core.Entities;

namespace Lms.MVC.Core.Repositories
{
    public interface IModuleRepository
    {
        Task AddAsync<T>(T added);
        Task<IEnumerable<Module>> GetAllModulesAsync(bool includeActivities);
        Task<IEnumerable<Module>> GetAllModulesAsync(int id);
        Task<Module> GetModuleAsync(int id, int moduleId);
        Task<Module> GetModuleByTitleAsync(int id, string title);
        void Remove(Module removed);
        void Remove<T>(T removed);
        Task<bool> SaveAsync();            
        
    }

}

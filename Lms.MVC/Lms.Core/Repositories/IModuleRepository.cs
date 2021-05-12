using Lms.MVC.Core.Entities;
using System.Threading.Tasks;

using System.Collections.Generic;
using System.Threading.Tasks;

using Lms.MVC.Core.Entities;

namespace Lms.MVC.Core.Repositories
{
    public interface IModuleRepository
    {
        Task AddAsync<T>(T added);
        Task<IEnumerable<Module>> GetAllModulesAsync(bool includeActivities);
        Task<Module> GetModuleAsync(int id);
        Task<Module> GetModuleAsync(int id, int moduleId);
        Task<Module> GetModuleByTitleAsync(int id, string title);
        void Remove(Module removed);
        void Remove<T>(T removed);
        void Update(Module module);
        Task<bool> SaveAsync();            
        
    }

}

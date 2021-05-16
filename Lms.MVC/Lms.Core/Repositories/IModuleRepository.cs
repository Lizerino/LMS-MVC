using System.Collections.Generic;
using System.Threading.Tasks;

using Lms.MVC.Core.Entities;

namespace Lms.MVC.Core.Repositories
{
    public interface IModuleRepository
    {
        Task AddAsync(Module added);

        Task<IEnumerable<Module>> GetAllModulesAsync(bool includeActivities);        

        Task<Module> GetModuleAsync(int id);

        Task<Module> GetModuleWithFilesAsync(int? id);
        Task<Module> GetModuleAsync(int id, int moduleId);
        Task<IEnumerable<Module>> GetAllModulesByCourseIdAsync(int id);

        Task<Module> GetModuleByTitleAsync(int id, string title);
        string GetCurrentModule(int? courseId, int? moduleId);
        string GetNextModule(int? courseId, int? moduleId);
        void Remove(Module removed);

        void Update(Module module);


        Task<bool> SaveAsync();
        Task<ICollection<ApplicationFile>> GetAllFilesByModuleId(int id);
       
    }
}
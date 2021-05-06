using Lms.MVC.Core.Entities;
using System.Threading.Tasks;

namespace Lms.MVC.Core.Repositories
{
    public interface IModuleRepository
    {
        Task AddAsync<T>(T added);

        Task<Module> GetModuleAsync(int moduleId);

    }
}
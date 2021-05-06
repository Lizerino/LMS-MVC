using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.MVC.Core.Repositories
{
    //Sugestion To have One Generic Repo To Role Them All :)
    public interface IGenericRepo
    {
        Task<T> GetT<T>();
        Task<IEnumerable<T>> GetTs<T>();
        Task AddAsync<T>(T added);
        void Remove<T>(T Removed);
    }
}

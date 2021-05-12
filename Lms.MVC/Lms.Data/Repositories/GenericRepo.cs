//TODO GitFix
using Lms.MVC.Core.Repositories;
using Lms.MVC.Data.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lms.MVC.Data.Repositories
{
    class GenericRepo : IGenericRepo
    {
        private readonly ApplicationDbContext db;
        public GenericRepo(ApplicationDbContext db) => this.db = db;

        public async Task AddAsync<T>(T added) => await db.AddAsync(added);
        public void Remove<T>(T removed) => db.Remove(removed);
        
        public Task<IEnumerable<T>> GetTs<T>() //=> await db.T.TolistAsync();
        {
            throw new NotImplementedException();
        }

        public Task<T> GetT<T>()
        {
            throw new NotImplementedException();
        }

    }
}

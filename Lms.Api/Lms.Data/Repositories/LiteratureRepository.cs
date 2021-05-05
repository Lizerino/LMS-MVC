using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lms.API.Core.Entities;
using Lms.API.Core.Repositories;
using Lms.API.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace Lms.API.Data.Repositories
{
    class LiteratureRepository : ILiteratureRepository
    {
        private readonly LmsAPIContext db;

        public LiteratureRepository(LmsAPIContext db)
        {
            this.db = db;
        }

        public async Task AddAsync<T>(T added)
        {
            await db.AddAsync(added);
        }

        public async Task<IEnumerable<Literature>> GetAllLiteratureAsync()
        {
            return await db.Literature.Include(l => l.Subject).Include(l => l.Level).ToListAsync();
        }

        public async Task<Literature> GetLiteratureAsync(int? id)
        {
            return await db.Literature.Include(l => l.Subject).Include(l => l.Level).FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<Literature>> GetLiteratureByTitleAsync(string title)
        {
            return await db.Literature.Include(l => l.Subject).Include(l => l.Level).Where(l => l.Title.Contains(title)).ToListAsync();
        }

        public void Remove(Literature removed)
        {
            db.Remove(removed);
        }

        public async Task<bool> SaveAsync()
        {
            return (await db.SaveChangesAsync()) >= 0;
        }
    }
}

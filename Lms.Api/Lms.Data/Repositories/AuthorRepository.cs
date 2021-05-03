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
    class AuthorRepository : IAuthorRepository
    {
        private readonly LmsAPIContext db;

        public AuthorRepository(LmsAPIContext db)
        {
            this.db = db;
        }

        public async Task AddAsync<T>(T added)
        {
            await db.AddAsync(added);
        }

        public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
        {
            return await db.Authors.ToListAsync();
        }

        public async Task<Author> GetAuthorAsync(int? id)
        {
            return await db.Authors.Include(a => a.Bibliography).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Author>> GetAuthorByNameAsync(string name)
        {
            return await db.Authors.Where(a => $"{a.FirstName} {a.LastName}".Contains(name)).ToListAsync();
        }

        public void Remove(Author removed)
        {
            db.Remove(removed);
        }

        public async Task<bool> SaveAsync()
        {
            return (await db.SaveChangesAsync()) >= 0;
        }
    }
}

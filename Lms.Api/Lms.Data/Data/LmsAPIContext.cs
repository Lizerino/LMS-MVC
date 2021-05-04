using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lms.API.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lms.API.Data.Data
{
    public class LmsAPIContext : DbContext
    {
        public LmsAPIContext (DbContextOptions<LmsAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Literature> Literature { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Subject> Subjects { get; set; }

    }
}

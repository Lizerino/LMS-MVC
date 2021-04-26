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

        public DbSet<Course> Courses { get; set; }


        public DbSet<Module> Modules { get; set; }
    }
}

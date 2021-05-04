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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthorLiterature>().HasKey(al => new { al.AuthorId, al.LiteratureId});

            modelBuilder.Entity<AuthorLiterature>()
                .HasOne<Author>(al => al.Author)
                .WithMany(a => a.Bibliography)
                .HasForeignKey(al => al.AuthorId);

            modelBuilder.Entity<AuthorLiterature>()
                .HasOne<Literature>(al => al.Literature)
                .WithMany(l => l.Authors)
                .HasForeignKey(al => al.LiteratureId);
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Literature> Literature { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<AuthorLiterature> Authorship { get; set; }

    }
}

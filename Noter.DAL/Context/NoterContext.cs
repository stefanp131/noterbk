using Microsoft.EntityFrameworkCore;
using Noter.DAL.Entities;
using Noter.DAL.User;
using System;

namespace Noter.DAL.Context
{
    public class NoterContext: DbContext
    {
        public NoterContext(DbContextOptions<NoterContext> options)
            :base(options)
        {
            Database.Migrate();
        }

        public DbSet<Topic> Topics { get; set; }
        public DbSet<Commentary> Commentaries { get; set; }
        public DbSet<NoterUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var date = new DateTime(2018, 11, 4);
            modelBuilder.Entity<Commentary>().Property(e => e.Created).HasDefaultValue(date);
        }
    }
}

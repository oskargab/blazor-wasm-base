using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static DbRepository.MyDbContext;

namespace DbRepository
{
    public class MyDbContext : DbContext
    {
        public MyDbContext()
        {

        }
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }
        //Might be needed for db migrations design time
        //protected override void OnConfiguring(DbContextOptionsBuilder
        // optionsBuilder)
        //{
        //    optionsBuilder.UseNpgsql("CONNECTION STRING FOR DB");
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);

        }
        public DbSet<User> Users { get; set; }
        public DbSet<GoogleAccount> GoogleAccounts { get; set; }
    }
}

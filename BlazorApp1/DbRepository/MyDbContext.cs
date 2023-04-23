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
        protected override void OnConfiguring(DbContextOptionsBuilder
         optionsBuilder)
        {
            optionsBuilder.UseNpgsql("");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().HasOne(u => u.InternalAccount).WithOne().HasForeignKey<User>("InternalAccountId");
            modelBuilder.Entity<User>().HasOne(u => u.GoogleAccount).WithOne().HasForeignKey<User>("GoogleAccountId");
            modelBuilder.Entity<User>().HasOne(u => u.DiscordAccount).WithOne().HasForeignKey<User>("DiscordAccountId");
            


        }
        public DbSet<User> Users { get; set; }
        public DbSet<InternalAccount> InternalAccounts { get; set; }
        public DbSet<GoogleAccount> GoogleAccounts { get; set; }
        public DbSet<DiscordAccount> DiscordAccounts { get; set; }
    }
}

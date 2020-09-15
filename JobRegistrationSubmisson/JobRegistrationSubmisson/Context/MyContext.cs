using JobRegistrationSubmisson.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobRegistrationSubmisson.Context
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }
        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<AccRoles> AccRoles { get; set; }
        public DbSet<Employees> Employees { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<UserRole>().HasKey(sc => new { sc.UserId, sc.RoleId });
            modelBuilder.Entity<AccRoles>().HasKey(sc => sc.UserId);
            modelBuilder.Entity<Employees>().HasKey(sc => sc.EmpId);

            modelBuilder.Entity<AccRoles>()
                .HasOne<Accounts>(sc => sc.Accounts)
                .WithMany(s => s.AccRoles)
                .HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<Employees>()
                .HasOne<Accounts>(s => s.Accounts)
                .WithOne(ad => ad.Employees)
                .HasForeignKey<Employees>(ad => ad.EmpId);
        }

    }
}

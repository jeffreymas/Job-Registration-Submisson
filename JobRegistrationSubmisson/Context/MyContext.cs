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
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRole { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<UserRole>().HasKey(sc => new { sc.UserId, sc.RoleId });
            modelBuilder.Entity<UserRole>().HasKey(sc => sc.UserId);
            modelBuilder.Entity<Employees>().HasKey(sc => sc.EmpId);
            modelBuilder.Entity<JobSeeker>().HasKey(sc => sc.JobSId);
            modelBuilder.Entity<JobSeeker>().HasKey(sc => sc.JoblistId);


            modelBuilder.Entity<UserRole>()
                .HasOne<User>(sc => sc.User)
                .WithMany(s => s.userRoles)
                .HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<Employees>()
                .HasOne<User>(s => s.User)
                .WithOne(ad => ad.Employees)
                .HasForeignKey<Employees>(ad => ad.EmpId);

            modelBuilder.Entity<JobSeeker>()
                .HasOne<User>(s => s.User)
                .WithOne(ad => ad.JobSeeker)
                .HasForeignKey<JobSeeker>(ad => ad.JobSId);

            modelBuilder.Entity<JobSeeker>()
                .HasOne<Joblist>(sc => sc.Joblist)
                .WithMany(s => s.jobSeekers)
                .HasForeignKey(sc => sc.JoblistId);

            //base.OnModelCreating(modelBuilder);
        }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<JobSeeker> JobSeekers { get; set; }
        public DbSet<Joblist> Joblists { get; set; }

    }
}

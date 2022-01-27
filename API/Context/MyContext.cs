using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Context

{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions <MyContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Profiling> Profilings { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<University> Universities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Account)
                .WithOne(a => a.Employee)
                .HasForeignKey<Account>(a => a.NIK);

            modelBuilder.Entity<Account>()
                .HasOne(a => a.Profiling)
                .WithOne(p => p.Account)
                .HasForeignKey<Profiling>(p => p.NIK);

            modelBuilder.Entity<Education>()
                .HasMany(ed => ed.Profilings)
                .WithOne(p => p.Education);

            modelBuilder.Entity<Education>()
               .HasOne(ed => ed.University)
               .WithMany(u => u.Educations);
        }
    }
}

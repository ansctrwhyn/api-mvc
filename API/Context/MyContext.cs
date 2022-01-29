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
        public DbSet<AccountRole> AccountRoles { get; set; }
        public DbSet<Role> Roles { get; set; }

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

            modelBuilder.Entity<AccountRole>()
                .HasKey(ar => new { ar.Account_NIK, ar.Role_Id });

            modelBuilder.Entity<AccountRole>()
                .HasOne(ar => ar.Account)
                .WithMany(a => a.AccountRoles)
                .HasForeignKey(ar => ar.Account_NIK);

            modelBuilder.Entity<AccountRole>()
                .HasOne(ar => ar.Role)
                .WithMany(r => r.AccountRoles)
                .HasForeignKey(ar => ar.Role_Id);
        }
    }
}

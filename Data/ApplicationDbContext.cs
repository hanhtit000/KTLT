using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StaffManagement.Models;

namespace StaffManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Nguyen Van A", Address = "Ha Noi", Description = "Nope", DOB = DateOnly.Parse("1-1-2002"), Email = "saolaithenhi@gmail.com", PhoneNumber = "0123456789", StaffNumber = "123" },
                new User { Id = 2, Name = "Kwame Adu-Darkwa", Address = "Africa", Description = "Nope", DOB = DateOnly.Parse("1-1-2002"), Email = "saolaithenhi@gmail.com", PhoneNumber = "0123456789", StaffNumber = "123" },
                new User { Id = 3, Name = "Nguyen Van C", Address = "Ha Noi", Description = "Nope", DOB = DateOnly.Parse("1-1-2002"), Email = "saolaithenhi@gmail.com", PhoneNumber = "0123456789", StaffNumber = "123" }
            );
            modelBuilder.Entity<Department>().HasData(
                new Department { DepartmentId = 1, Code = "A", Name = "Department A", UserId = 1 },
                new Department { DepartmentId = 2, Code = "B", Name = "Department B", UserId = 1 },
                new Department { DepartmentId = 3, Code = "C", Name = "Department C", UserId = 1 }
            );
            modelBuilder.Entity<Account>().HasData(
                new Account { Id = 1, Username = "admin", Password = "1", IsActive = true, IsAdmin = true, UserId = 1 },
                new Account { Id = 2, Username = "staff", Password = "1", IsActive = true, IsAdmin = false, UserId = 2 },
                new Account { Id = 3, Username = "employee", Password = "1", IsActive = true, IsAdmin = false, UserId = 3 }
            );
        }
    }
}

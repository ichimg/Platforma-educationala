using EducationalPlatform.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

using System.Configuration;
using System.Diagnostics.Contracts;
using System.Reflection.Metadata;


namespace EducationalPlatform.DataAccess
{
    public class EducationalPlatformDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Absence> Absences { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Absence>().HasOne(c => c.Teacher).WithOne().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Absence>().HasOne(c => c.Student).WithOne().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Classroom>()
            .HasOne(c => c.Teacher)
            .WithOne()
            .HasForeignKey<Classroom>(c => c.TeacherId);

        }
    }
}

using eGradeBook.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace eGradeBook.Infrastructure
{
    public class GradeBookContext : IdentityDbContext<GradeBookUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public GradeBookContext() : base("eGradeBookContext")
        {
            Database.SetInitializer<GradeBookContext>(new GradeBookInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AdminUser>().ToTable("AdminUser");
            modelBuilder.Entity<StudentUser>().ToTable("StudentUser");
            modelBuilder.Entity<TeacherUser>().ToTable("TeacherUser");
            modelBuilder.Entity<ParentUser>().ToTable("ParentUser");
            modelBuilder.Entity<ClassMasterUser>().ToTable("ClassMasterUser");
        }

        public DbSet<SchoolClass> ClassRooms { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<FinalGrade> FinalGrades { get; set; }
        public DbSet<Teaching> TeachingAssignments { get; set; }

        // Additions for new schema
        public DbSet<Program> Programs { get; set; }
        public DbSet<Taking> Takings { get; set; }
    }
}
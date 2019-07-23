using eGradeBook.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AdminUser>().ToTable("AdminUser");
            modelBuilder.Entity<StudentUser>().ToTable("StudentUser");
            modelBuilder.Entity<TeacherUser>().ToTable("TeacherUser");
            modelBuilder.Entity<ParentUser>().ToTable("ParentUser");
            modelBuilder.Entity<ClassMasterUser>().ToTable("ClassMasterUser");
        }

        public DbSet<SchoolClass> ClassRooms { get; set; }
        public DbSet<Course> Subjects { get; set; }
        public DbSet<Curriculum> Curricula { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Teaching> TeachingAssignments { get; set; }
    }
}
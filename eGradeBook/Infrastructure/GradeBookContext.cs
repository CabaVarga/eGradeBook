using eGradeBook.Migrations;
using eGradeBook.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace eGradeBook.Infrastructure
{
    /// <summary>
    /// The main entry in the Data Access layer.
    /// Basically defining our database schema and mapping the domain model into the database and vice-versa
    /// </summary>
    public class GradeBookContext : IdentityDbContext<GradeBookUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        /// <summary>
        /// The constructor. Seeding is happening inside here
        /// </summary>
        public GradeBookContext() : base("DefaultConnection")
        {
            Database.SetInitializer<GradeBookContext>(new MigrateDatabaseToLatestVersion<GradeBookContext, Configuration>());
        }

        /// <summary>
        /// Fluent API lives here. What can't be achieved with annotations (or through convention)
        /// must be defined here.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            try
            {
                modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

                base.OnModelCreating(modelBuilder);
                modelBuilder.Entity<AdminUser>().ToTable("AdminUser");
                modelBuilder.Entity<StudentUser>().ToTable("StudentUser");
                modelBuilder.Entity<TeacherUser>().ToTable("TeacherUser");
                modelBuilder.Entity<ParentUser>().ToTable("ParentUser");
                modelBuilder.Entity<ClassMasterUser>().ToTable("ClassMasterUser");
            }

            catch (EntityException ex)
            {
                Debug.WriteLine("Database cannot be created.");
                throw ex;
            }
        }

        /// <summary>
        /// Mapping the classrooms
        /// </summary>
        public DbSet<ClassRoom> ClassRooms { get; set; }

        /// <summary>
        /// Mapping the courses
        /// </summary>
        public DbSet<Course> Courses { get; set; }

        /// <summary>
        /// Mapping the grades
        /// </summary>
        public DbSet<Grade> Grades { get; set; }

        /// <summary>
        /// Mapping the final grades
        /// </summary>
        public DbSet<FinalGrade> FinalGrades { get; set; }

        /// <summary>
        /// Mapping the teacher - course relation
        /// </summary>
        public DbSet<Teaching> TeachingAssignments { get; set; }

        /// <summary>
        /// Mapping the student -  parent relation 
        /// </summary>
        public DbSet<StudentParent> StudentParents { get; set; }

        // Additions for new schema
        /// <summary>
        /// Mapping the classroom - course - teacher relation
        /// </summary>
        public DbSet<Program> Programs { get; set; }

        /// <summary>
        /// Mapping the student - program relation. Base entity for grading.
        /// </summary>
        public DbSet<Taking> Takings { get; set; }

        public DbSet<FileResource> FileResources { get; set; }

        public DbSet<Enrollment> Enrollments { get; set; }
    }
}
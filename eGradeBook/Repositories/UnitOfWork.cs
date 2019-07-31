using eGradeBook.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Unity;

namespace eGradeBook.Repositories
{
    /// <summary>
    /// Unit of work - the repository orchestrator
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private DbContext context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public UnitOfWork(DbContext context)
        {
            this.context = context;
        }
        /// <summary>
        /// Grade book users repository
        /// </summary>
        [Dependency]
        public IGenericRepository<GradeBookUser> GradeBookUsersRepository { get; set; }

        /// <summary>
        /// Auth repository
        /// </summary>
        [Dependency]
        public IAuthRepository AuthRepository { get; set; }

        /// <summary>
        /// Teachers repository
        /// </summary>
        [Dependency]
        public IGenericRepository<TeacherUser> TeachersRepository { get; set; }

        /// <summary>
        /// Students repository
        /// </summary>
        [Dependency]
        public IGenericRepository<StudentUser> StudentsRepository { get; set; }

        /// <summary>
        /// Parents repository
        /// </summary>
        [Dependency]
        public IGenericRepository<ParentUser> ParentsRepository { get; set; }

        /// <summary>
        /// Courses repository
        /// </summary>
        [Dependency]
        public IGenericRepository<Course> CoursesRepository { get; set; }

        /// <summary>
        /// Classrooms repository
        /// </summary>
        [Dependency]
        public IGenericRepository<ClassRoom> ClassRoomsRepository { get; set; }

        /// <summary>
        /// Teaching assignments repository. Connecting teachers with courses.
        /// </summary>
        [Dependency]
        public IGenericRepository<Teaching> TeachingAssignmentsRepository { get; set; }

        /// <summary>
        /// Grades repository.
        /// </summary>
        [Dependency]
        public IGenericRepository<Grade> GradesRepository { get; set; }

        /// <summary>
        /// Programs repository. Connecting classrooms, courses and teachers.
        /// </summary>
        [Dependency]
        public IGenericRepository<Program> ProgramsRepository { get; set; }

        /// <summary>
        /// Takings repository. Connecting students with program entities.
        /// </summary>
        [Dependency]
        public IGenericRepository<Taking> TakingsRepository { get; set; }

        /// <summary>
        /// Final grades repository
        /// </summary>
        [Dependency]
        public IGenericRepository<FinalGrade> FinalGradesRepository { get; set; }

        /// <summary>
        /// StudentParents repository. Connecting students and parents.
        /// </summary>
        [Dependency]
        public IGenericRepository<StudentParent> StudentParentsRepository { get; set; }

        /// <summary>
        /// Save all changes.
        /// </summary>
        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        /// <summary>
        /// Dispose resources
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
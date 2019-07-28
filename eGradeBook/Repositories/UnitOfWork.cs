using eGradeBook.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Unity;

namespace eGradeBook.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private DbContext context;

        public UnitOfWork(DbContext context)
        {
            this.context = context;
        }

        [Dependency]
        public IGenericRepository<GradeBookUser> GradeBookUsersRepository { get; set; }

        [Dependency]
        public IAuthRepository AuthRepository { get; set; }

        [Dependency]
        public IGenericRepository<TeacherUser> TeachersRepository { get; set; }

        [Dependency]
        public IGenericRepository<StudentUser> StudentsRepository { get; set; }

        [Dependency]
        public IGenericRepository<ParentUser> ParentsRepository { get; set; }

        [Dependency]
        public IGenericRepository<Course> CoursesRepository { get; set; }

        [Dependency]
        public IGenericRepository<SchoolClass> ClassRoomsRepository { get; set; }

        [Dependency]
        public IGenericRepository<Teaching> TeachingAssignmentsRepository { get; set; }

        [Dependency]
        public IGenericRepository<Grade> GradesRepository { get; set; }

        [Dependency]
        public IGenericRepository<Program> ProgramsRepository { get; set; }

        [Dependency]
        public IGenericRepository<Taking> TakingsRepository { get; set; }

        [Dependency]
        public IGenericRepository<FinalGrade> FinalGradesRepository { get; set; }

        [Dependency]
        public IGenericRepository<StudentParent> StudentParentsRepository { get; set; }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
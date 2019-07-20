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
        public IGenericRepository<Subject> SubjectsRepository { get; set; }

        [Dependency]
        public IGenericRepository<Curriculum> CurriculaRepository { get; set; }

        [Dependency]
        public IGenericRepository<ClassRoom> ClassRoomsRepository { get; set; }

        [Dependency]
        public IGenericRepository<TeachingAssignment> TeachingAssignmentsRepository { get; set; }

        [Dependency]
        public IGenericRepository<Grade> GradesRepository { get; set; }

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
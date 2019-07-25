using eGradeBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<GradeBookUser> GradeBookUsersRepository { get; }
        IAuthRepository AuthRepository { get; }

        IGenericRepository<TeacherUser> TeachersRepository { get; }
        IGenericRepository<StudentUser> StudentsRepository { get; }
        IGenericRepository<ParentUser> ParentsRepository { get; }
        IGenericRepository<Grade> GradesRepository { get; }
        IGenericRepository<Course> SubjectsRepository { get; }
        IGenericRepository<SchoolClass> ClassRoomsRepository { get; }
        IGenericRepository<Teaching> TeachingAssignmentsRepository { get; }
        IGenericRepository<Program> ProgramsRepository { get; }

        void Save();
    }
}
using eGradeBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Repositories
{
    /// <summary>
    /// Unit of work, orchestrating the various repositories
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Grade book users repository
        /// </summary>
        IGenericRepository<GradeBookUser> GradeBookUsersRepository { get; }

        /// <summary>
        /// Authentication repository
        /// </summary>
        IAuthRepository AuthRepository { get; }

        /// <summary>
        /// Teachers repository
        /// </summary>
        IGenericRepository<TeacherUser> TeachersRepository { get; }

        /// <summary>
        /// Students repository
        /// </summary>
        IGenericRepository<StudentUser> StudentsRepository { get; }

        /// <summary>
        /// Parents repository
        /// </summary>
        IGenericRepository<ParentUser> ParentsRepository { get; }

        /// <summary>
        /// Grades repository
        /// </summary>
        IGenericRepository<Grade> GradesRepository { get; }

        /// <summary>
        /// Courses repository
        /// </summary>
        IGenericRepository<Course> CoursesRepository { get; }

        /// <summary>
        /// Classrooms repository
        /// </summary>
        IGenericRepository<ClassRoom> ClassRoomsRepository { get; }

        /// <summary>
        /// Teaching assignments repository
        /// </summary>
        IGenericRepository<Teaching> TeachingAssignmentsRepository { get; }

        /// <summary>
        /// Programs repository
        /// </summary>
        IGenericRepository<Program> ProgramsRepository { get; }

        /// <summary>
        /// Takings repository, connecting students with classroom, course and teacher
        /// </summary>
        IGenericRepository<Taking> TakingsRepository { get; }

        /// <summary>
        /// Final grades repository
        /// </summary>
        IGenericRepository<FinalGrade> FinalGradesRepository { get; }

        /// <summary>
        /// Student parents connection repository
        /// </summary>
        IGenericRepository<StudentParent> StudentParentsRepository { get; }

        /// <summary>
        /// Save changes
        /// </summary>
        void Save();
    }
}
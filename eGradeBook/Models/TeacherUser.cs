using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models
{
    /// <summary>
    /// A teacher is someone who is teaching courses in given classrooms to specified students
    /// </summary>
    public class TeacherUser : GradeBookUser
    {
        /// <summary>
        /// Title
        /// NOTE Mr, Miss, Mrs
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Degree
        /// NOTE College, Master, Phd
        /// </summary>
        public string Degree { get; set; }

        /// <summary>
        /// A collection of courses taught by the teacher
        /// </summary>
        public virtual ICollection<Teaching> Teachings { get; set; }
    }
}
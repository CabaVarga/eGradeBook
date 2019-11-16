using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eGradeBook.Models
{
    /// <summary>
    /// Taking model -- A student is taking a given program, a combination of classroom, course and teacher
    /// </summary>
    public class Taking
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The programs Id
        /// NOTE Program is teacher + course + classroom
        /// </summary>
        //[Index("IX_Student_Program", IsUnique = true, Order = 1)]
        public int ProgramId { get; set; }

        /// <summary>
        /// The students Id
        /// </summary>
        //[Index("IX_Student_Program", IsUnique = true, Order = 2)]
        //public int StudentId { get; set; }

        /// <summary>
        /// It was student, now it is enrollment
        /// </summary>
        //[Index("IX_Enrollment_Program", IsUnique = true, Order = 2)]
        public int EnrollmentId { get; set; }

        /// <summary>
        /// The program in the relation
        /// </summary>
        [Required]
        [ForeignKey("ProgramId")]
        public virtual Program Program { get; set; }

        /// <summary>
        /// The enrollment in the relation
        /// </summary>
        //[Required]
        [ForeignKey("EnrollmentId")]
        public virtual Enrollment Enrollment { get; set; }

        /// <summary>
        /// Grades for the current course
        /// </summary>
        public virtual ICollection<Grade> Grades { get; set; }

        /// <summary>
        /// Final grades for the current course
        /// </summary>
        public virtual ICollection<FinalGrade> FinalGrades { get; set; }
    }
}
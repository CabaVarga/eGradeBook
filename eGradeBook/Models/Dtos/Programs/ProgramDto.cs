using eGradeBook.Models.Dtos.Teachings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Programs
{
    public class ProgramDto : TeachingDto
    {
        /// <summary>
        /// Program Id
        /// </summary>
        public int ProgramId { get; set; }
        
        /// <summary>
        /// Classroom Id
        /// </summary>
        [Required]
        public int ClassRoomId { get; set; }

        /// <summary>
        /// Weekly hours
        /// </summary>
        [Required]
        public virtual int WeeklyHours { get; set; }

        /// <summary>
        /// Classroom name
        /// </summary>
        public string ClassRoomName { get; set; }

        /// <summary>
        /// School grade
        /// </summary>
        public virtual int SchoolGrade { get; set; }
    }
}
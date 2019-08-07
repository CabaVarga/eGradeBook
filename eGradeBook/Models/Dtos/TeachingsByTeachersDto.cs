using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos
{
    /// <summary>
    /// Courses grouped by teachers teaching them
    /// </summary>
    public class TeachingsByTeachersDto
    {
        /// <summary>
        /// Teacher
        /// NOTE need id
        /// </summary>
        public string Teacher { get; set; }

        public int TeacherId { get; set; }

        /// <summary>
        /// Courses
        /// NOTE only strings, this is not optimal
        /// </summary>
        public ICollection<object> Courses { get; set; }
    }
}
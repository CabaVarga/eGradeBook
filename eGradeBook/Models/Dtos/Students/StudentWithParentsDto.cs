using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Students
{
    /// <summary>
    /// Student with parents listed
    /// </summary>
    public class StudentWithParentsDto : StudentDto
    {
        /// <summary>
        /// The students' parents
        /// </summary>
        public IEnumerable<ParentsDto> Parents { get; set; }
    }

    /// <summary>
    /// Parent dto without circular reference
    /// </summary>
    public class ParentsDto
    {
        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Parent Id
        /// </summary>
        public int ParentId { get; set; }
    }
}
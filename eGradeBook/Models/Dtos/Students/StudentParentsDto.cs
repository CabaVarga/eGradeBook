using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Students
{
    /// <summary>
    /// Students and parents dto
    /// NOTE too little info
    /// </summary>
    public class StudentParentsDto
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StudentParentsDto()
        {
            this.Parents = new List<string>();
        }

        /// <summary>
        /// The students' full name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The list of parents -- their full name only
        /// </summary>
        public List<string> Parents { get; set; }
    }
}
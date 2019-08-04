using eGradeBook.Models.Dtos.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Parents
{
    /// <summary>
    /// Structure to list parents and their children
    /// </summary>
    public class ParentChildrenDto
    {
        /// <summary>
        /// The parents name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The parents' Id
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// The parents' children
        /// </summary>
        public List<StudentDto> Children { get; set; }
    }
}
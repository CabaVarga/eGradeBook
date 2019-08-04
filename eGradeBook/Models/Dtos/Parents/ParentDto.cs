using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Parents
{
    /// <summary>
    /// The most basic Parent Dto possible
    /// NOTE too little details
    /// </summary>
    public class ParentDto
    {
        /// <summary>
        /// The Parents Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The parents Full name
        /// NOTE temporary solution
        /// </summary>
        public string FullName { get; set; }
    }
}
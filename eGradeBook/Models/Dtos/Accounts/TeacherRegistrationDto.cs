using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Accounts
{
    /// <summary>
    /// Registration data structure for Teacher users
    /// </summary>
    public class TeacherRegistrationDto : UserRegistrationDto
    {
        /// <summary>
        /// Title
        /// NOTE Mr, Miss, Mrs
        /// </summary>
        [RegularExpression("(Mr|Miss|Mrs)", ErrorMessage = "Either Mr, Miss, or Mrs")]
        public string Title { get; set; }

        /// <summary>
        /// Degree
        /// NOTE College, Master, Phd
        /// </summary>
        public string Degree { get; set; }
    }
}
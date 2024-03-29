﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos
{
    /// <summary>
    /// Teachers grouped by courses they teach
    /// </summary>
    public class TeachingsByCoursesDto
    {
        /// <summary>
        /// Course, only the name -- suboptimal
        /// </summary>
        public string Course { get; set; }

        /// <summary>
        /// List of teachers full name
        /// </summary>
        public ICollection<string> Teachers { get; set; }
    }
}
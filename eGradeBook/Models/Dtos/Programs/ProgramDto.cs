using eGradeBook.Models.Dtos.Teachings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Programs
{
    public class ProgramDto : ProgramOptionalHoursDto
    {
        /// <summary>
        /// Weekly hours
        /// </summary>
        [Required]
        public new int WeeklyHours { get; set; }
    }
}
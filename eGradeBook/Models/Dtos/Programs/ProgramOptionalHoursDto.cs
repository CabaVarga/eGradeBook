using eGradeBook.Models.Dtos.Teachings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Programs
{

    public class ProgramOptionalHoursDto : TeachingDto
    {
        /// <summary>
        /// Program Id
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ProgramId { get; set; }

        /// <summary>
        /// Classroom Id
        /// </summary>
        [Required]
        public int ClassRoomId { get; set; }

        /// <summary>
        /// Weekly hours
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int WeeklyHours { get; set; }

        /// <summary>
        /// Classroom name
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ClassRoomName { get; set; }

        /// <summary>
        /// School grade
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int SchoolGrade { get; set; }

    }
}
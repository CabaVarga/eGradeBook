using eGradeBook.Models.Dtos.Programs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Takings
{
    public class TakingDto : ProgramDto
    {
        /// <summary>
        /// Taking Id
        /// </summary>
        public int TakingId { get; set; }

        /// <summary>
        /// Weekly hours override
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Include, Required = Required.Default)]
        [JsonIgnore]
        public override int WeeklyHours { get; set; }

        /// <summary>
        /// School grade override
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Include, Required = Required.Default)]
        [JsonIgnore]
        public override int SchoolGrade { get; set; }

        /// <summary>
        /// Student Id
        /// </summary>
        [Required]
        public int StudentId { get; set; }

        /// <summary>
        /// Students username
        /// </summary>
        public string StudentName { get; set; }
    }
}
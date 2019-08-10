using eGradeBook.Models.Dtos.Programs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Takings
{
    public class TakingDto : ProgramOptionalHoursDto
    {
        /// <summary>
        /// Taking Id
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int TakingId { get; set; }

        /// <summary>
        /// Student Id
        /// </summary>
        [Required]
        public int StudentId { get; set; }

        /// <summary>
        /// Students username
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string StudentName { get; set; }
    }
}
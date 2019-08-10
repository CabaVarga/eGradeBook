using eGradeBook.Models.Dtos.Takings;
using eGradeBook.Utilities.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Grades
{
    /// <summary>
    /// Dto for grades, limited for now
    /// For viewing, not for grade assignment
    /// </summary>
    public class GradeDto : TakingDto
    {
        /// <summary>
        /// Grade Id
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int GradeId { get; set; }

        /// <summary>
        /// School term
        /// </summary>
        [Required]
        [Range(1, 2, ErrorMessage = "Semester must be 1 or 2")]
        public int Semester { get; set; }

        /// <summary>
        /// Assignment date
        /// </summary>
        // [JsonConverter(typeof(DateFormatConverter), "yyyy - MM - dd")]
        [Required]
        public DateTime AssignmentDate { get; set; }

        /// <summary>
        /// Notes
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Grade point
        /// </summary>
        [Required]
        [Range(1, 5, ErrorMessage = "Grade point must be in range 1 to 5")]
        public int GradePoint { get; set; }
    }
}
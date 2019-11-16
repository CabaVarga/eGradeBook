using eGradeBook.Models.Dtos.Takings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.FinalGrades
{
    /// <summary>
    /// Dto for final grades
    /// </summary>
    public class FinalGradeDto
    {
        /// <summary>
        /// Grade Id
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int FinalGradeId { get; set; }

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
        [Range(1, 5, ErrorMessage = "Final Grade point must be in range 1 to 5")]
        public int FinalGradePoint { get; set; }

        public int TakingId { get; set; }
    }
}
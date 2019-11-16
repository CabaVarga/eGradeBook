using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eGradeBook.Models
{
    /// <summary>
    /// Associative class connecting Students with Classrooms
    /// </summary>
    public class Enrollment
    {
        /// <summary>
        /// Enrollment entity id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The students id
        /// </summary>
        public int StudentId { get; set; }

        /// <summary>
        /// Classroom id
        /// </summary>
        public int ClassRoomId { get; set; }

        /// <summary>
        /// The student entity in the relation
        /// </summary>
        [ForeignKey("StudentId")]
        public virtual StudentUser Student { get; set; }

        /// <summary>
        /// The classroom entity in the relation
        /// </summary>
        [ForeignKey("ClassRoomId")]
        public virtual ClassRoom ClassRoom { get; set; }

        [JsonIgnore]
        public virtual ICollection<Taking> Takings { get; set; }
    }
}
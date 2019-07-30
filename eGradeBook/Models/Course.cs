using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eGradeBook.Models
{
    /// <summary>
    /// A course, or subject. It can be teached by different programs in different grades, with differing number of hours per week.
    /// </summary>
    public class Course
    {
        /// <summary>
        /// Id of the course
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The course's name. It has to be unique.
        /// </summary>
        [Index("IX_Course_Grade", IsUnique = true)]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Something like matis, probably will be replaced with a description.
        /// </summary>
        public string ColloqialName { get; set; }

        /// <summary>
        /// A course by itself is worthless if it is not being teached in a classroom of a given grade and by a given teacher
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<Program> Programs { get; set; }

        /// <summary>
        /// Not every teacher can teach every course. We must connect the teachers that can teach a given course.
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<Teaching> Teachings { get; set; }
    }
}
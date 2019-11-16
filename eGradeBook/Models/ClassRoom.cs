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
    /// Class Room model
    /// </summary>
    public class ClassRoom
    {
        ///// <summary>
        ///// Class Room constructor
        ///// </summary>
        //public ClassRoom()
        //{
        //    this.Students = new HashSet<StudentUser>();
        //}

        /// <summary>
        /// Identifies a classroom by an integer number
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Class Grade is from 1 to 8 -- we will not use 1-4 though
        /// </summary>
        [Required]
        [Range(1, 8, ErrorMessage = "Class grades are from one to eight")]
        public int ClassGrade { get; set; }

        /// <summary>
        /// Every Class Room has a name. Either with the grade identifier or without
        /// </summary>
        [Required]
        [StringLength(4, ErrorMessage = "Name should be between 2 and 4 characters long", MinimumLength = 2)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        /// <summary>
        /// The classmaster's id -- the foreign key in the database
        /// </summary>
        [ForeignKey("ClassMaster")]
        public int? ClassMasterId { get; set; }

        /// <summary>
        /// A classroom has a classmaster -- though not in the model yet
        /// </summary>
        public virtual ClassMasterUser ClassMaster { get; set; }

        ///// <summary>
        ///// A classroom consists of students
        ///// </summary>
        //[JsonIgnore]
        //public virtual ICollection<StudentUser> Students { get; set; }

        /// <summary>
        /// Every classroom has a program, a list of courses the students can / must take
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<Program> Program { get; set; }

        /// <summary>
        /// Every classroom has students enrolled
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eGradeBook.Models
{
    public class Curriculum
    {
        public int Id { get; set; }

        [ForeignKey("ClassRoom")]
        public int ClassRoomId { get; set; }

        [ForeignKey("Subject")]
        public int SubjectId { get; set; }

        [Required]
        public int HoursPerWeek { get; set; }

        public int? HoursPerSchoolYear { get; set; }

        public virtual ClassRoom ClassRoom { get; set; }

        public virtual Subject Subject { get; set; }
    }
}
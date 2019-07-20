using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eGradeBook.Models
{
    public class TeachingAssignment
    {
        public int Id { get; set; }

        [ForeignKey("Teacher")]
        public int TeacherId { get; set; }

        [ForeignKey("Subject")]
        public int SubjectId { get; set; }

        [ForeignKey("ClassRoom")]
        public int ClassRoomId { get; set; }

        public virtual TeacherUser Teacher { get; set; }

        public virtual Subject Subject { get; set; }

        public virtual ClassRoom ClassRoom { get; set; }

    }
}
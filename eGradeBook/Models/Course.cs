using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eGradeBook.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Index("IX_Course_Grade", IsUnique = true)]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public string ColloqialName { get; set; }

        [JsonIgnore]
        public virtual ICollection<Program> Programs { get; set; }

        [JsonIgnore]
        public virtual ICollection<Teaching> Teachings { get; set; }
    }
}
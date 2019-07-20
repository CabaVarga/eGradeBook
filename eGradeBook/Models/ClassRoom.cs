using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eGradeBook.Models
{
    public class ClassRoom
    {
        public ClassRoom()
        {
            this.Students = new HashSet<StudentUser>();
        }

        public int Id { get; set; }

        [Required]
        [Range(1, 8, ErrorMessage = "Class grades are from one to eight")]
        public int ClassGrade { get; set; }

        [Required]
        [StringLength(4, ErrorMessage = "Name should be between 2 and 4 characters long", MinimumLength = 2)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        [ForeignKey("ClassMaster")]
        public int? ClassMasterId { get; set; }

        public virtual ClassMasterUser ClassMaster { get; set; }

        [JsonIgnore]
        public virtual ICollection<StudentUser> Students { get; set; }
    }
}
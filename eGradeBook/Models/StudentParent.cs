using System.ComponentModel.DataAnnotations.Schema;

namespace eGradeBook.Models
{
    public class StudentParent
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public int ParentId { get; set; }

        [ForeignKey("StudentId")]
        public virtual StudentUser Student { get; set; }

        [ForeignKey("ParentId")]
        public virtual ParentUser Parent { get; set; }
    }
}
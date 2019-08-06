using System.ComponentModel.DataAnnotations.Schema;

namespace eGradeBook.Models
{
    /// <summary>
    /// Association class between a student and a parent
    /// </summary>
    public class StudentParent
    {
        /// <summary>
        /// Id number
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The students Id
        /// </summary>
        public int StudentId { get; set; }

        /// <summary>
        /// The parents Id
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// The student in the relation
        /// </summary>
        [ForeignKey("StudentId")]
        public virtual StudentUser Student { get; set; }

        /// <summary>
        /// The parent in the relation
        /// </summary>
        [ForeignKey("ParentId")]
        public virtual ParentUser Parent { get; set; }
    }
}
using eGradeBook.Models.Dtos.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Parents
{
    public class ParentChildrenDto
    {
        public string Name { get; set; }
        public int ParentId { get; set; }

        public List<StudentDto> Children { get; set; }
    }
}
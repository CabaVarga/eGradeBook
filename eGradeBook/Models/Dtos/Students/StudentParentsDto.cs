using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Students
{
    public class StudentParentsDto
    {
        public StudentParentsDto()
        {
            this.Parents = new List<string>();
        }

        public string Name { get; set; }
        public List<string> Parents { get; set; }
    }
}
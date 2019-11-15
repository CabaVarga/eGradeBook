using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models
{
    public class FileResource
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
    }
}
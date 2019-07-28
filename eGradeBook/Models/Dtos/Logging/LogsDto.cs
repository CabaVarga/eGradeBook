using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Logging
{
    public class LogsDto
    {
        public LogsDto()
        {
            this.Logs = new List<LogDto>();
        }
        public class LogDto
        {
            public string FileName { get; set; }
            public string Path { get; set; }
            public string Size { get; set; }
            public string LastModified { get; set; }
        }

        public List<LogDto> Logs { get; set; }
    }
}
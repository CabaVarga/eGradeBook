using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Logging
{
    /// <summary>
    /// Not really a model, better place is maybe utilities?
    /// TODO add properties for direct access (URI) and download
    /// </summary>
    public class LogsDto
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LogsDto()
        {
            this.Logs = new List<LogDto>();
        }

        /// <summary>
        /// Nested helper class with log file data
        /// </summary>
        public class LogDto
        {
            /// <summary>
            /// File name
            /// </summary>
            public string FileName { get; set; }

            /// <summary>
            /// Path to file
            /// </summary>
            public string Path { get; set; }

            /// <summary>
            /// File size
            /// </summary>
            public string Size { get; set; }

            /// <summary>
            /// Last modified
            /// </summary>
            public string LastModified { get; set; }
        }

        /// <summary>
        /// List of log files
        /// </summary>
        public List<LogDto> Logs { get; set; }
    }
}
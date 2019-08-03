using eGradeBook.Utilities.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Registration
{
    /// <summary>
    /// Information about newly created user
    /// </summary>
    public class CreatedResourceDto
    {
        /// <summary>
        /// User's id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Path to user resource
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Type of user
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public UserRole UserRole { get; set; }
    }
}
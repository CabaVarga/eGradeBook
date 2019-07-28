using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Registration
{
    public class CreatedResourceDto
    {
        public int Id { get; set; }
        public string Location { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public UserType Type { get; set; }
    }

    public enum UserType { ADMIN, TEACHER, STUDENT, PARENT }
}
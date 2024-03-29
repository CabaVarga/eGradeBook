﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Teachings
{
    public class TeachingDto
    {
        /// <summary>
        /// Teaching Id
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int TeachingId { get; set; }

        /// <summary>
        /// Course Id
        /// </summary>
        [Required]
        public int CourseId { get; set; }

        /// <summary>
        /// Teacher Id
        /// </summary>
        [Required]
        public int TeacherId { get; set; }

        /// <summary>
        /// Course name
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string CourseName { get; set; }

        /// <summary>
        /// Teachers username
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string TeacherName { get; set; }
    }
}
﻿using eGradeBook.Models;
using eGradeBook.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    public interface ITeachingsService
    {
        IEnumerable<TeachingsByCoursesDto> GetAllTeachingAssignmentsByCourses();
        IEnumerable<TeachingsByTeachersDto> GetAllTeachingAssignmentsByTeachers();
    }
}
using eGradeBook.Models;
using eGradeBook.Models.Dtos;
using eGradeBook.Models.Dtos.Teachings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    /// <summary>
    /// Service for working with Teaching entities.
    /// Teaching entities are connecting teachers and courses.
    /// NOTE do they need a special service or I should handle them through
    /// TeachersService and CoursesService?
    /// Or both, but other services using Teachings service when they are needing its' services?
    /// </summary>
    public interface ITeachingsService
    {
        /// <summary>
        /// Create a teaching from TeachingDto, return TeachingDto
        /// </summary>
        /// <param name="teachingDto"></param>
        /// <returns></returns>
        TeachingDto CreateTeaching(TeachingDto teachingDto);

        /// <summary>
        /// Get all teachings as dtos
        /// </summary>
        /// <returns></returns>
        IEnumerable<TeachingDto> GetAllTeachings();

        /// <summary>
        /// Get teaching by teachingId, return teachingDto
        /// </summary>
        /// <param name="teachingId"></param>
        /// <returns></returns>
        TeachingDto GetTeachingById(int teachingId);


        /// <summary>
        /// Delete Teaching by teachingId
        /// </summary>
        /// <param name="teachingId"></param>
        /// <returns></returns>
        TeachingDto DeleteTeaching(int teachingId);


        IEnumerable<TeachingDto> GetTeachingsByQuery(
            int? teacherId = null,
            int? studentId = null,
            int? parentId = null,
            int? courseId = null,
            int? classRoomId = null,
            int? schoolGrade = null);
    }
}
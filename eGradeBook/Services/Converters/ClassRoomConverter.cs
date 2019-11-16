using eGradeBook.Models;
using eGradeBook.Models.Dtos.ClassRooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services.Converters
{
    /// <summary>
    /// Conversion of classroom related resources
    /// </summary>
    public static class ClassRoomConverter
    {
        /// <summary>
        /// Convert classroom entity to classroom dto
        /// TODO consolidate, program null or not null and so on
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static ClassRoomDto ClassRoomToClassRoomDto(ClassRoom c)
        {
            return new ClassRoomDto()
            {
                ClassRoomId = c.Id,
                Name = c.Name,
                SchoolGrade = c.ClassGrade
            };
        }
    }
}
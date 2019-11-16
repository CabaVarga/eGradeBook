using eGradeBook.Models;
using eGradeBook.Models.Dtos.Accounts;
using eGradeBook.Models.Dtos.Teachers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services.Converters
{
    /// <summary>
    /// Conversion for teahcer users 
    /// </summary>
    public static class TeachersConverter
    {
        /// <summary>
        /// Convert a teacher model to teacher dto
        /// </summary>
        /// <param name="teacher">A teacher (full) model</param>
        /// <returns>Teacher Dto object, ready for Json serialization</returns>
        public static TeacherDto TeacherToTeacherDto(TeacherUser teacher)
        {
            return new TeacherDto()
            {
                TeacherId = teacher.Id,
                UserName = teacher.UserName,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                Gender = teacher.Gender,
                Email = teacher.Email,
                Phone = teacher.PhoneNumber,
                Degree = teacher.Degree,
                Title = teacher.Title,
                AvatarId = teacher.AvatarId
            };
        }

        /// <summary>
        /// Update full entity from dto before sending to the storage
        /// </summary>
        /// <param name="user"></param>
        /// <param name="dto"></param>
        public static void UpdateTeachersPersonalData(TeacherUser user, TeacherUpdateDto dto)
        {
            user.UserName = dto.UserName;
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Gender = dto.Gender;
            user.Email = dto.Email;
            user.PhoneNumber = dto.PhoneNumber;
            user.Title = dto.Title;
            user.Degree = dto.Degree;
            user.AvatarId = dto.AvatarId;
        }

        /// <summary>
        /// Convert a registration dto to full entity
        /// </summary>
        /// <param name="teacherReg"></param>
        /// <returns></returns>
        public static TeacherUser TeacherRegistrationDtoToTeacher(TeacherRegistrationDto teacherReg)
        {
            return new TeacherUser()
            {
                UserName = teacherReg.UserName,
                FirstName = teacherReg.FirstName,
                LastName = teacherReg.LastName,
                Gender = teacherReg.Gender,
                Email = teacherReg.Email,
                PhoneNumber = teacherReg.PhoneNumber,
                Degree = teacherReg.Degree,
                Title = teacherReg.Title,
                AvatarId = teacherReg.AvatarId
            };
        }
    }
}
using eGradeBook.Models;
using eGradeBook.Models.Dtos.Accounts;
using eGradeBook.Models.Dtos.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services.Converters
{
    /// <summary>
    /// Converters for student users and their relations
    /// </summary>
    public static class StudentsConverter
    {
        /// <summary>
        /// Convert a student model to student dto
        /// </summary>
        /// <param name="student">A student (full) model</param>
        /// <returns>Student Dto object, ready for Json serialization</returns>
        public static StudentDto StudentToStudentDto(StudentUser student)
        {
            return new StudentDto()
            {
                StudentId = student.Id,
                UserName = student.UserName,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Gender = student.Gender,
                Email = student.Email,
                Phone = student.PhoneNumber,
                PlaceOfBirth = student.PlaceOfBirth,
                DateOfBirth = student.DateOfBirth,
                AvatarId = student.AvatarId
            };
        }

        /// <summary>
        /// Convert registration dto to full entity
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static StudentUser StudentRegistrationDtoToStudent(StudentRegistrationDto dto)
        {
            return new StudentUser()
            {
                UserName = dto.UserName,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Gender = dto.Gender,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                PlaceOfBirth = dto.PlaceOfBirth,
                DateOfBirth = dto.DateOfBirth,
                AvatarId = dto.AvatarId
            };
        }

        /// <summary>
        /// Update full entity from dto before sending to the storage
        /// </summary>
        /// <param name="user"></param>
        /// <param name="dto"></param>
        public static void UpdateStudentsPersonalData(StudentUser user, StudentUpdateDto dto)
        {
            user.UserName = dto.UserName;
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Gender = dto.Gender;
            user.Email = dto.Email;
            user.PhoneNumber = dto.PhoneNumber;
            user.PlaceOfBirth = dto.PlaceOfBirth;
            user.DateOfBirth = dto.DateOfBirth;
            user.AvatarId = dto.AvatarId;
        }
    }
}
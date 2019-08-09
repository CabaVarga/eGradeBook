using eGradeBook.Models;
using eGradeBook.Models.Dtos.Accounts;
using eGradeBook.Models.Dtos.Parents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services.Converters
{
    /// <summary>
    /// Converter for Parent entities
    /// </summary>
    public static class ParentsConverter
    {
        /// <summary>
        /// Convert a parent model to parent dto
        /// </summary>
        /// <param name="parent">A parent (full) model</param>
        /// <returns>Parent Dto object, ready for Json serialization</returns>
        public static ParentDto ParentToParentDto(ParentUser parent)
        {
            return new ParentDto()
            {
                Id = parent.Id,
                FirstName = parent.FirstName,
                LastName = parent.LastName,
                Gender = parent.Gender,
                Email = parent.Email,
                PhoneNumber = parent.PhoneNumber,

            };
        }

        /// <summary>
        /// Update full entity from dto before sending to the storage
        /// </summary>
        /// <param name="user"></param>
        /// <param name="dto"></param>
        public static void UpdateParentsPersonalData(ParentUser user, ParentUpdateDto dto)
        {
            user.UserName = dto.UserName;
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Gender = dto.Gender;
            user.Email = dto.Email;
            user.PhoneNumber = dto.PhoneNumber;
        }

        /// <summary>
        /// Registration dto to full entity
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static ParentUser ParentRegistrationDtoToParent(ParentRegistrationDto dto)
        {
            return new ParentUser()
            {
                UserName = dto.UserName,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Gender = dto.Gender,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };
        }

        public static ParentReportDto ParentToParentReportDto(ParentUser parent)
        {
            var report = new ParentReportDto()
            {
                ParentId = parent.Id,
                FirstName = parent.FirstName,
                LastName = parent.LastName,
                Email = parent.Email,
                Gender = parent.Gender,
                Students = parent.StudentParents.Select(sp => Converters.StudentsConverter.StudentToStudentReportDto(sp.Student))
            };

            return report;
        }
    }
}
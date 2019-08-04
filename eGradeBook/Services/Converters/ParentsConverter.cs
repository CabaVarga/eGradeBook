using eGradeBook.Models;
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
                FullName = parent.FirstName + " " + parent.LastName
            };
        }
    }
}
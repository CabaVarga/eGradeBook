using eGradeBook.Models.Dtos.Accounts;
using Swashbuckle.Examples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.SwaggerHelpers.Examples
{
    public class RegisterStudentExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new StudentRegistrationDto()
            {
                UserName = "noviucenik",
                FirstName = "Ime",
                LastName = "Prezime",
                Gender = "m",
                Email = "ime@mail.com",
                PhoneNumber = "0234255",
                Password = "password",
                ConfirmPassword = "password",
                PlaceOfBirth = "Novi Sad",
                DateOfBirth = new DateTime(2000, 10, 10)
            };
        }
    }
}
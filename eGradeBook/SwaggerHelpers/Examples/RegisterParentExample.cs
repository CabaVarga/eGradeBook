using eGradeBook.Models.Dtos.Accounts;
using Swashbuckle.Examples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.SwaggerHelpers.Examples
{
    public class RegisterParentExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new ParentRegistrationDto()
            {
                UserName = "noviroditelj",
                FirstName = "Ime",
                LastName = "Prezime",
                Gender = "m",
                Email = "ime@mail.com",
                PhoneNumber = "0234255",
                Password = "password",
                ConfirmPassword = "password",
            };
        }
    }
}
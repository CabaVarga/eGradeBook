using eGradeBook.Infrastructure;
using eGradeBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace eGradeBook.Controllers
{
    [RoutePrefix("api/hidden")]
    public class HiddenController : ApiController
    {
        [Route("delete-all")]
        [HttpGet]
        public IHttpActionResult DeleteAllData()
        {
            Dictionary<string, int> report = new Dictionary<string, int>();

            using (var ctx = new GradeBookContext())
            {
                var finalGrades = ctx.FinalGrades;
                report["FinalGrades"] = finalGrades.Count();

                foreach (var finalGrade in finalGrades)
                {
                    ctx.FinalGrades.Remove(finalGrade);
                }

                var grades = ctx.Grades;
                report["Grades"] = grades.Count();

                foreach (var grade in grades)
                {
                    ctx.Grades.Remove(grade);
                }

                var takings = ctx.Takings;
                report["Takings"] = takings.Count();

                foreach (var taking in takings)
                {
                    ctx.Takings.Remove(taking);
                }

                // The ParentChildren invisible table does not let me delete everything...
                var parents = ctx.Users.OfType<ParentUser>();
                report["Parents"] = parents.Count();

                foreach (var parent in parents)
                {
                    parent.Children = new List<StudentUser>();
                }

                ctx.SaveChanges();

                var students = ctx.Users.OfType<StudentUser>();
                report["Students"] = students.Count();

                foreach (var student in students)
                {
                    student.Parents = new List<ParentUser>();
                }

                ctx.SaveChanges();

                foreach (var parent in parents)
                {
                    ctx.Users.Remove(parent);
                }

                foreach (var student in students)
                {
                    ctx.Users.Remove(student);
                }

                
                ctx.SaveChanges();
            }

            return Ok(report);
        }
    }
}

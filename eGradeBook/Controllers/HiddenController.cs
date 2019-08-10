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
    /// <summary>
    /// Web api controller for direct database manipulation
    /// Deletions, insertions and the like
    /// </summary>
    [RoutePrefix("api/hidden")]
    [Authorize(Roles = "admins")]
    public class HiddenController : ApiController
    {
        /// <summary>
        /// Delete all data from the database
        /// </summary>
        /// <returns></returns>
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

                var studentParents = ctx.StudentParents;
                report["StudentParents"] = studentParents.Count();

                foreach (var studentParent in studentParents)
                {
                    ctx.StudentParents.Remove(studentParent);
                }

                // --- PARENTS
                var parents = ctx.Users.OfType<ParentUser>().ToList();
                report["Parents"] = parents.Count();

                foreach (var parent in parents)
                {
                    var roles = ctx.Set<CustomUserRole>().Where(cur => cur.UserId == parent.Id).ToList();

                    if (roles.Count != 0)
                    {
                        foreach (var role in roles)
                        {
                            ctx.Set<CustomUserRole>().Remove(role);
                        }
                    }

                    ctx.Users.Remove(parent);
                }

                var students = ctx.Users.OfType<StudentUser>().ToList();
                report["Students"] = students.Count();

                foreach (var student in students)
                {
                    var roles = ctx.Set<CustomUserRole>().Where(cur => cur.UserId == student.Id).ToList();

                    if (roles.Count != 0)
                    {
                        foreach (var role in roles)
                        {
                            ctx.Set<CustomUserRole>().Remove(role);
                        }
                    }

                    // public async Task<IdentityResult> DeleteUser(int userId)
                    ctx.Users.Remove(student);
                }

                // TODO remove ClassRooms, Programs, Teachings, Courses and Teachers

                // --- PROGRAMS
                var programs = ctx.Programs.ToList();
                report["Programs"] = programs.Count();

                foreach (var program in programs)
                {
                    ctx.Programs.Remove(program);
                }

                // --- CLASSROOMS
                var classRooms = ctx.ClassRooms.ToList();
                report["ClassRooms"] = classRooms.Count();

                foreach (var classRoom in classRooms)
                {
                    ctx.ClassRooms.Remove(classRoom);
                }

                // --- TEACHINGS
                var teachings = ctx.TeachingAssignments.ToList();
                report["Teachings"] = teachings.Count();

                foreach (var teaching in teachings)
                {
                    ctx.TeachingAssignments.Remove(teaching);
                }

                // --- COURSES
                var courses = ctx.Courses.ToList();
                report["Courses"] = courses.Count();

                foreach (var course in courses)
                {
                    ctx.Courses.Remove(course);
                }

                // --- TEACHERS
                var teachers = ctx.Users.OfType<TeacherUser>().ToList();
                report["Teachers"] = teachers.Count();

                foreach (var teacher in teachers)
                {
                    var roles = ctx.Set<CustomUserRole>().Where(cur => cur.UserId == teacher.Id).ToList();

                    if (roles.Count != 0)
                    {
                        foreach (var role in roles)
                        {
                            ctx.Set<CustomUserRole>().Remove(role);
                        }
                    }

                    ctx.Users.Remove(teacher);
                }

                ctx.SaveChanges();
            }

            return Ok(report);
        }
    }
}

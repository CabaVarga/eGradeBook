using eGradeBook.Infrastructure;
using eGradeBook.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

        /// <summary>
        /// Fill all data into the database
        /// </summary>
        /// <returns></returns>
        [Route("fill-all")]
        [HttpGet]
        public IHttpActionResult FillDatabase()
        {
            Dictionary<string, int> report = new Dictionary<string, int>();

            using (var context = new GradeBookContext())
            {
                try
                {
                    UserManager<GradeBookUser, int> _userManager =
                        new UserManager<GradeBookUser, int>(
                            new UserStore<GradeBookUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>(context));

                    RoleManager<CustomRole, int> _roleManager =
                        new RoleManager<CustomRole, int>(new RoleStore<CustomRole, int, CustomUserRole>(context));

                    //#region Roles
                    //CustomRole adminRole = new CustomRole() { Name = "admins" };
                    //CustomRole studentRole = new CustomRole() { Name = "students" };
                    //CustomRole teacherRole = new CustomRole() { Name = "teachers" };
                    //CustomRole parentRole = new CustomRole() { Name = "parents" };

                    //_roleManager.Create(adminRole);
                    //_roleManager.Create(studentRole);
                    //_roleManager.Create(teacherRole);
                    //_roleManager.Create(parentRole);
                    //#endregion

                    #region Roles
                    CustomRole adminRole = _roleManager.FindByName("admins");
                    CustomRole studentRole = _roleManager.FindByName("students");
                    CustomRole teacherRole = _roleManager.FindByName("teachers");
                    CustomRole parentRole = _roleManager.FindByName("parents");
                    #endregion

                    //#region Admins
                    //List<AdminUser> admins = new List<AdminUser>();
                    //admins.Add(new AdminUser() { UserName = "peraperic", FirstName = "Pera", LastName = "Peric", Email = "pera@gmail.com", Gender = "m", PhoneNumber = "0604546321" });
                    //admins.Add(new AdminUser() { UserName = "milanmilic", FirstName = "Milan", LastName = "Milic", Email = "milan@gmail.com", Gender = "m", PhoneNumber = "0604546321" });

                    //foreach (var admin in admins)
                    //{
                    //    _userManager.Create(admin, "password");
                    //    _userManager.AddToRole(admin.Id, "admins");
                    //}
                    //#endregion

                    #region Courses
                    List<Course> courses = new List<Course>();
                    courses.Add(new Course() { Name = "Mathematics" });
                    courses.Add(new Course() { Name = "Physics" });
                    courses.Add(new Course() { Name = "English language" });
                    courses.Add(new Course() { Name = "German language" });
                    courses.Add(new Course() { Name = "Serbian language and literature" });

                    context.Courses.AddRange(courses);

                    context.SaveChanges();
                    #endregion

                    #region Teachers
                    List<TeacherUser> teachers = new List<TeacherUser>();
                    teachers.Add(new TeacherUser() { UserName = "radaradic", FirstName = "Rada", LastName = "Radic", Email = "rada@gmail.com", Gender = "f", PhoneNumber = "063154845", Title = "Ms", Degree = "MMathPhys" });
                    teachers.Add(new TeacherUser() { UserName = "tomatomic", FirstName = "Toma", LastName = "Tomic", Email = "toma@gmail.com", Gender = "m", PhoneNumber = "064542365", Title = "Mr", Degree = "MLit" });
                    teachers.Add(new TeacherUser() { UserName = "raderadic", FirstName = "Rade", LastName = "Radic", Email = "rade@gmail.com", Gender = "m", PhoneNumber = "063256456", Title = "Mr", Degree = "MLit" });
                    teachers.Add(new TeacherUser() { UserName = "ivaivic", FirstName = "Iva", LastName = "Ivic", Email = "iva@gmail.com", Gender = "f", PhoneNumber = "021546895", Title = "Mrs", Degree = "MMathPhys" });

                    foreach (var teacher in teachers)
                    {
                        _userManager.Create(teacher, "password");
                        _userManager.AddToRole(teacher.Id, "teachers");
                    }
                    #endregion

                    #region Teachings
                    List<Teaching> teachings = new List<Teaching>();
                    teachings.Add(new Teaching() { Teacher = teachers[0], Course = courses[0] });
                    teachings.Add(new Teaching() { Teacher = teachers[0], Course = courses[1] });
                    teachings.Add(new Teaching() { Teacher = teachers[1], Course = courses[4] });
                    teachings.Add(new Teaching() { Teacher = teachers[1], Course = courses[2] });
                    teachings.Add(new Teaching() { Teacher = teachers[2], Course = courses[4] });
                    teachings.Add(new Teaching() { Teacher = teachers[2], Course = courses[3] });
                    teachings.Add(new Teaching() { Teacher = teachers[3], Course = courses[0] });
                    teachings.Add(new Teaching() { Teacher = teachers[3], Course = courses[1] });

                    context.TeachingAssignments.AddRange(teachings);
                    context.SaveChanges();
                    #endregion

                    #region ClassRooms
                    List<ClassRoom> classRooms = new List<ClassRoom>();
                    classRooms.Add(new ClassRoom() { Name = "5-A", ClassGrade = 5 });
                    classRooms.Add(new ClassRoom() { Name = "5-B", ClassGrade = 5 });
                    classRooms.Add(new ClassRoom() { Name = "6-A", ClassGrade = 6 });
                    classRooms.Add(new ClassRoom() { Name = "6-B", ClassGrade = 6 });

                    context.ClassRooms.AddRange(classRooms);
                    context.SaveChanges();
                    #endregion

                    #region Programs
                    List<Program> programs = new List<Program>();
                    programs.Add(new Program() { Teaching = teachings[0], ClassRoom = classRooms[0], WeeklyHours = 5, Course = courses[0] });
                    programs.Add(new Program() { Teaching = teachings[7], ClassRoom = classRooms[0], WeeklyHours = 3, Course = courses[1] });
                    programs.Add(new Program() { Teaching = teachings[2], ClassRoom = classRooms[0], WeeklyHours = 5, Course = courses[4] });
                    programs.Add(new Program() { Teaching = teachings[3], ClassRoom = classRooms[0], WeeklyHours = 3, Course = courses[2] });
                    programs.Add(new Program() { Teaching = teachings[6], ClassRoom = classRooms[1], WeeklyHours = 5, Course = courses[0] });
                    programs.Add(new Program() { Teaching = teachings[1], ClassRoom = classRooms[1], WeeklyHours = 3, Course = courses[1] });
                    programs.Add(new Program() { Teaching = teachings[2], ClassRoom = classRooms[1], WeeklyHours = 5, Course = courses[4] });
                    programs.Add(new Program() { Teaching = teachings[5], ClassRoom = classRooms[1], WeeklyHours = 3, Course = courses[3] });
                    programs.Add(new Program() { Teaching = teachings[6], ClassRoom = classRooms[2], WeeklyHours = 4, Course = courses[0] });
                    programs.Add(new Program() { Teaching = teachings[7], ClassRoom = classRooms[2], WeeklyHours = 4, Course = courses[1] });
                    programs.Add(new Program() { Teaching = teachings[4], ClassRoom = classRooms[2], WeeklyHours = 4, Course = courses[4] });
                    programs.Add(new Program() { Teaching = teachings[3], ClassRoom = classRooms[2], WeeklyHours = 4, Course = courses[2] });
                    programs.Add(new Program() { Teaching = teachings[0], ClassRoom = classRooms[3], WeeklyHours = 4, Course = courses[0] });
                    programs.Add(new Program() { Teaching = teachings[1], ClassRoom = classRooms[3], WeeklyHours = 4, Course = courses[1] });
                    programs.Add(new Program() { Teaching = teachings[4], ClassRoom = classRooms[3], WeeklyHours = 4, Course = courses[4] });
                    programs.Add(new Program() { Teaching = teachings[5], ClassRoom = classRooms[3], WeeklyHours = 4, Course = courses[3] });


                    context.Programs.AddRange(programs);
                    context.SaveChanges();
                    #endregion

                    #region Students
                    List<StudentUser> students = new List<StudentUser>();
                    students.Add(new StudentUser() { UserName = "zoranzoric", FirstName = "Zoran", LastName = "Zoric", Email = "zoran@gmail.com", Gender = "m", PhoneNumber = "064548458", PlaceOfBirth = "Novi Sad", DateOfBirth = DateTime.Parse("2010-05-23"), ClassRoom = classRooms[0] });
                    students.Add(new StudentUser() { UserName = "zoricazoric", FirstName = "Zorica", LastName = "Zoric", Email = "zorica@gmail.com", Gender = "f", PhoneNumber = "063565485", PlaceOfBirth = "Novi Sad", DateOfBirth = DateTime.Parse("2010-05-23"), ClassRoom = classRooms[0] });
                    students.Add(new StudentUser() { UserName = "dragankljajic", FirstName = "Dragan", LastName = "Kljajic", Email = "dragan@gmail.com", Gender = "m", PhoneNumber = "02365465", PlaceOfBirth = "Becej", DateOfBirth = DateTime.Parse("2010-01-23"), ClassRoom = classRooms[1] });
                    students.Add(new StudentUser() { UserName = "draganamihic", FirstName = "Dragana", LastName = "Mihic", Email = "dragana@gmail.com", Gender = "f", PhoneNumber = "062456895", PlaceOfBirth = "Temerin", DateOfBirth = DateTime.Parse("2010-04-12"), ClassRoom = classRooms[1] });
                    students.Add(new StudentUser() { UserName = "milanmihic", FirstName = "Milan", LastName = "Mihic", Email = "milan@gmail.com", Gender = "m", PhoneNumber = "06548575", PlaceOfBirth = "Rumenka", DateOfBirth = DateTime.Parse("2009-11-05"), ClassRoom = classRooms[2] });
                    students.Add(new StudentUser() { UserName = "milanaivic", FirstName = "Milana", LastName = "Ivic", Email = "milana@gmail.com", Gender = "f", PhoneNumber = "02365847", PlaceOfBirth = "Novi Sad", DateOfBirth = DateTime.Parse("2009-08-07"), ClassRoom = classRooms[2] });
                    students.Add(new StudentUser() { UserName = "ivanmihic", FirstName = "Ivan", LastName = "Mihic", Email = "aleksandar@gmail.com", Gender = "m", PhoneNumber = "01254822", PlaceOfBirth = "Novi Sad", DateOfBirth = DateTime.Parse("2009-03-30"), ClassRoom = classRooms[3] });
                    students.Add(new StudentUser() { UserName = "ivanazoric", FirstName = "Ivana", LastName = "Zoric", Email = "aleksandra@gmail.com", Gender = "f", PhoneNumber = "01525465", PlaceOfBirth = "Beograd", DateOfBirth = DateTime.Parse("2009-02-28"), ClassRoom = classRooms[3] });

                    foreach (var student in students)
                    {
                        _userManager.Create(student, "password");
                        _userManager.AddToRole(student.Id, "teachers");
                    }
                    #endregion

                    #region Takings
                    List<Taking> takings = new List<Taking>();
                    takings.Add(new Taking() { Program = programs[0], Student = students[0] });
                    takings.Add(new Taking() { Program = programs[1], Student = students[0] });
                    takings.Add(new Taking() { Program = programs[2], Student = students[0] });
                    takings.Add(new Taking() { Program = programs[3], Student = students[0] });
                    takings.Add(new Taking() { Program = programs[0], Student = students[1] });
                    takings.Add(new Taking() { Program = programs[1], Student = students[1] });
                    takings.Add(new Taking() { Program = programs[2], Student = students[1] });
                    takings.Add(new Taking() { Program = programs[3], Student = students[1] });
                    takings.Add(new Taking() { Program = programs[4], Student = students[2] });
                    takings.Add(new Taking() { Program = programs[5], Student = students[2] });
                    takings.Add(new Taking() { Program = programs[6], Student = students[2] });
                    takings.Add(new Taking() { Program = programs[7], Student = students[2] });
                    takings.Add(new Taking() { Program = programs[4], Student = students[3] });
                    takings.Add(new Taking() { Program = programs[5], Student = students[3] });
                    takings.Add(new Taking() { Program = programs[6], Student = students[3] });
                    takings.Add(new Taking() { Program = programs[7], Student = students[3] });
                    takings.Add(new Taking() { Program = programs[8], Student = students[4] });
                    takings.Add(new Taking() { Program = programs[9], Student = students[4] });
                    takings.Add(new Taking() { Program = programs[10], Student = students[4] });
                    takings.Add(new Taking() { Program = programs[11], Student = students[4] });
                    takings.Add(new Taking() { Program = programs[8], Student = students[5] });
                    takings.Add(new Taking() { Program = programs[9], Student = students[5] });
                    takings.Add(new Taking() { Program = programs[10], Student = students[5] });
                    takings.Add(new Taking() { Program = programs[11], Student = students[5] });
                    takings.Add(new Taking() { Program = programs[12], Student = students[6] });
                    takings.Add(new Taking() { Program = programs[13], Student = students[6] });
                    takings.Add(new Taking() { Program = programs[14], Student = students[6] });
                    takings.Add(new Taking() { Program = programs[15], Student = students[6] });
                    takings.Add(new Taking() { Program = programs[12], Student = students[7] });
                    takings.Add(new Taking() { Program = programs[13], Student = students[7] });
                    takings.Add(new Taking() { Program = programs[14], Student = students[7] });
                    takings.Add(new Taking() { Program = programs[15], Student = students[7] });

                    context.Takings.AddRange(takings);
                    context.SaveChanges();
                    #endregion

                    #region Parents
                    List<ParentUser> parents = new List<ParentUser>();
                    parents.Add(new ParentUser() { UserName = "milicamihic", FirstName = "Milica", LastName = "Mihic", Email = "caba.varga@gmail.com", Gender = "f", PhoneNumber = "065326584" });
                    parents.Add(new ParentUser() { UserName = "markozoric", FirstName = "Marko", LastName = "Zoric", Email = "caba.varga@gmail.com", Gender = "m", PhoneNumber = "023568954" });
                    parents.Add(new ParentUser() { UserName = "anakljajic", FirstName = "Ana", LastName = "Kljajic", Email = "caba.varga@gmail.com", Gender = "f", PhoneNumber = "063568595" });
                    parents.Add(new ParentUser() { UserName = "evazoric", FirstName = "Eva", LastName = "Zoric", Email = "caba.varga@gmail.com", Gender = "f", PhoneNumber = "024568958" });
                    parents.Add(new ParentUser() { UserName = "dusanmihic", FirstName = "Dusan", LastName = "Mihic", Email = "caba.varga@gmail.com", Gender = "m", PhoneNumber = "062458785" });
                    
                    foreach (var parent in parents)
                    {
                        _userManager.Create(parent, "password");
                        _userManager.AddToRole(parent.Id, "parents");
                    }
                    #endregion
                    
                    #region StudentParents
                    List<StudentParent> studentParents = new List<StudentParent>();
                    studentParents.Add(new StudentParent() { Student = students[0], Parent = parents[1] });
                    studentParents.Add(new StudentParent() { Student = students[0], Parent = parents[3] });
                    studentParents.Add(new StudentParent() { Student = students[1], Parent = parents[1] });
                    studentParents.Add(new StudentParent() { Student = students[1], Parent = parents[3] });
                    studentParents.Add(new StudentParent() { Student = students[2], Parent = parents[0] });
                    studentParents.Add(new StudentParent() { Student = students[2], Parent = parents[4] });
                    studentParents.Add(new StudentParent() { Student = students[3], Parent = parents[2] });
                    studentParents.Add(new StudentParent() { Student = students[3], Parent = parents[4] });
                    studentParents.Add(new StudentParent() { Student = students[4], Parent = parents[0] });
                    studentParents.Add(new StudentParent() { Student = students[4], Parent = parents[4] });
                    studentParents.Add(new StudentParent() { Student = students[5], Parent = parents[2] });
                    studentParents.Add(new StudentParent() { Student = students[6], Parent = parents[0] });
                    studentParents.Add(new StudentParent() { Student = students[6], Parent = parents[4] });
                    studentParents.Add(new StudentParent() { Student = students[7], Parent = parents[3] });
                    studentParents.Add(new StudentParent() { Student = students[7], Parent = parents[4] });

                    context.StudentParents.AddRange(studentParents);
                    context.SaveChanges();
                    #endregion

                    #region Grades
                    List<Grade> grades = new List<Grade>();
                    grades.Add(new Grade() { Taking = takings[1], GradePoint = 4, Assigned = new DateTime(2019, 9, 25), SchoolTerm = 1, Notes = "Algebra" });
                    grades.Add(new Grade() { Taking = takings[1], GradePoint = 4, Assigned = new DateTime(2019, 10, 15), SchoolTerm = 1, Notes = "Arithmetics" });
                    grades.Add(new Grade() { Taking = takings[1], GradePoint = 5, Assigned = new DateTime(2019, 11, 10), SchoolTerm = 1, Notes = "" });

                    Random random = new Random(100);

                    int taking = 0;
                    int grade = 0;
                    int month = 0;
                    int day = 0;

                    for (int i = 0; i < 200; i++)
                    {
                        taking = random.Next(32);
                        grade = random.Next(1, 6);
                        month = random.Next(9, 13);
                        day = random.Next(1, 31);

                        grades.Add(new Grade() { Taking = takings[taking], GradePoint = grade, Assigned = new DateTime(2019, month, day), SchoolTerm = 1, Notes = "" });
                    }


                    context.Grades.AddRange(grades);
                    context.SaveChanges();
                    #endregion
                }

                catch (SqlException ex)
                {
                    throw ex;
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return Ok(report);
        }
    }
}

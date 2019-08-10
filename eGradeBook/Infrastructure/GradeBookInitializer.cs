using eGradeBook.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace eGradeBook.Infrastructure
{
    /// <summary>
    /// Grade Book Initializer. The name tells it all.
    /// </summary>
    public class GradeBookInitializer : DropCreateDatabaseAlways<GradeBookContext>
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The main entry to the database seed. Needs a half day's work to clean up the code.
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(GradeBookContext context)
        {
            logger.Info("Seeding started");

            // SeedByAlgorithm(context);
            SeedByHand(context);

            base.Seed(context);
            logger.Info("Seeding ended");
        }

        private void SeedByAlgorithm(GradeBookContext context)
        {
            try
            {
                logger.Info("Seeding began");
                UserManager<GradeBookUser, int> _userManager =
                    new UserManager<GradeBookUser, int>(
                        new UserStore<GradeBookUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>(context));

                RoleManager<CustomRole, int> _roleManager =
                    new RoleManager<CustomRole, int>(new RoleStore<CustomRole, int, CustomUserRole>(context));

                #region Roles
                CustomRole adminRole = new CustomRole() { Name = "admins" };
                CustomRole studentRole = new CustomRole() { Name = "students" };
                CustomRole teacherRole = new CustomRole() { Name = "teachers" };
                CustomRole parentRole = new CustomRole() { Name = "parents" };

                _roleManager.Create(adminRole);
                _roleManager.Create(studentRole);
                _roleManager.Create(teacherRole);
                _roleManager.Create(parentRole);
                #endregion

                logger.Info("Roles created");

                #region Admins
                AdminUser admin_pera = new AdminUser() { UserName = "peraperic", FirstName = "Pera", LastName = "Peric" };
                AdminUser admin_milan = new AdminUser() { UserName = "milanmilic", FirstName = "Milan", LastName = "Milic" };

                _userManager.Create(admin_pera, "password");
                _userManager.Create(admin_milan, "password");

                _userManager.AddToRole(admin_pera.Id, "admins");
                _userManager.AddToRole(admin_milan.Id, "admins");
                #endregion

                logger.Info("Admins created");

                #region Courses, FOR NOW LET's use these
                List<Course> courses = new List<Course>();
                courses.Add(new Course() { Name = "Mathematics", ColloqialName = "Matis" });
                courses.Add(new Course() { Name = "Chemistry", ColloqialName = "Hemija" });
                courses.Add(new Course() { Name = "Biology", ColloqialName = "Biologija" });
                courses.Add(new Course() { Name = "Informatics", ColloqialName = "Informatika" });
                courses.Add(new Course() { Name = "German", ColloqialName = "Nemacki" });
                courses.Add(new Course() { Name = "History", ColloqialName = "Istorija" });
                courses.Add(new Course() { Name = "English", ColloqialName = "Engleski" });

                context.Courses.AddRange(courses);

                context.SaveChanges();
                #endregion

                logger.Info("Courses cretated");

                #region Students

                List<StudentUser> students = SeederHelper.CreateStudents(20);

                foreach (var s in students)
                {
                    context.Users.Add(s);
                    context.SaveChanges();

                    CustomUserRole st = new CustomUserRole() { RoleId = studentRole.Id, UserId = s.Id };
                    context.SaveChanges();
                }

                #endregion

                logger.Info("Students created");

                #region Teachers
                List<TeacherUser> teachers = SeederHelper.CreateTeachers(10);

                foreach (var t in teachers)
                {
                    context.Users.Add(t);
                    context.SaveChanges();

                    CustomUserRole st = new CustomUserRole() { RoleId = teacherRole.Id, UserId = t.Id };
                    context.SaveChanges();
                }
                #endregion

                logger.Info("Teachers created");

                #region Parents
                List<StudentParent> studentParents = SeederHelper.CreateRealisticParents(students);

                List<ParentUser> parents = studentParents.Select(sp => sp.Parent).Distinct().ToList();

                foreach (var p in parents)
                {
                    context.Users.Add(p);

                    context.SaveChanges();

                    CustomUserRole st = new CustomUserRole() { RoleId = parentRole.Id, UserId = p.Id };
                    context.SaveChanges();
                }

                context.StudentParents.AddRange(studentParents);
                context.SaveChanges();
                #endregion

                logger.Info("Parents created");

                #region Teaching (assignments)

                // --- Teaching assignments *** This one could have been added to curriculum....
                List<Teaching> teachings = SeederHelper.AssignTeaching(teachers, courses);

                context.TeachingAssignments.AddRange(teachings);
                context.SaveChanges();

                #endregion

                logger.Info("Teaching assignments created");

                #region Classrooms
                List<ClassRoom> classes = SeederHelper.CreateSchoolClasses(2, 5, 6);
                context.ClassRooms.AddRange(classes);

                context.SaveChanges();

                #endregion

                logger.Info("Classrooms created");

                #region Student enrollments, Programs
                SeederHelper.AssignStudentsToClasses(students, classes, 2);
                context.SaveChanges();

                logger.Info("Students assigned to classrooms");

                List<Program> programs = SeederHelper.AssignProgram(teachings, classes);
                context.Programs.AddRange(programs);

                logger.Info("Students assigned to takings");

                context.SaveChanges();
                #endregion

                logger.Info("Students enrolled, programs created");

                #region Learning (student learning a subject)

                List<Taking> takings = SeederHelper.AssignTakings(students, programs);
                context.Takings.AddRange(takings);
                context.SaveChanges();
                #endregion

                logger.Info("Takings creadte");

                #region Grades 
                List<Grade> grades = SeederHelper.AssignGrades(takings, new DateTime(2018, 9, 1), new DateTime(2018, 12, 31), 1, 2, 5);
                context.Grades.AddRange(grades);
                context.SaveChanges();
                #endregion

                logger.Info("Grades assigned");

                #region Final Grades
                List<FinalGrade> finalGrades = SeederHelper.AssignFinalGrades(takings, new DateTime(2018, 9, 1), new DateTime(2018, 12, 31), 1);
                context.FinalGrades.AddRange(finalGrades);
                context.SaveChanges();

                #endregion

                logger.Info("Final grades assigned");
            }

            catch (SqlException ex)
            {
                Debug.WriteLine("Database cannot be accessed.");
                throw ex;
            }

            catch (Exception ex)
            {
                Debug.WriteLine("Seeding the database failed");
                throw ex;
            }
        }

        private void SeedByHand(GradeBookContext context)
        {
            try
            {
                logger.Info("Seeding began");
                UserManager<GradeBookUser, int> _userManager =
                    new UserManager<GradeBookUser, int>(
                        new UserStore<GradeBookUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>(context));

                RoleManager<CustomRole, int> _roleManager =
                    new RoleManager<CustomRole, int>(new RoleStore<CustomRole, int, CustomUserRole>(context));

                #region Roles
                CustomRole adminRole = new CustomRole() { Name = "admins" };
                CustomRole studentRole = new CustomRole() { Name = "students" };
                CustomRole teacherRole = new CustomRole() { Name = "teachers" };
                CustomRole parentRole = new CustomRole() { Name = "parents" };

                _roleManager.Create(adminRole);
                _roleManager.Create(studentRole);
                _roleManager.Create(teacherRole);
                _roleManager.Create(parentRole);
                #endregion

                #region Admins
                List<AdminUser> admins = new List<AdminUser>();
                admins.Add(new AdminUser() { UserName = "peraperic", FirstName = "Pera", LastName = "Peric", Email = "pera@gmail.com", Gender = "m", PhoneNumber = "0604546321" });
                admins.Add(new AdminUser() { UserName = "milanmilic", FirstName = "Milan", LastName = "Milic", Email = "milan@gmail.com", Gender = "m", PhoneNumber = "0604546321" });

                foreach (var admin in admins)
                {
                    _userManager.Create(admin, "password");
                    _userManager.AddToRole(admin.Id, "admins");
                }
                #endregion

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
                Debug.WriteLine("Database cannot be accessed.");
                throw ex;
            }

            catch (Exception ex)
            {
                Debug.WriteLine("Seeding the database failed");
                throw ex;
            }
        }
    }
}
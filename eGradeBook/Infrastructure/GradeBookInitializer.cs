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

            SeedByAlgorithm(context);

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
                AdminUser admin_pera = new AdminUser() { UserName = "peraperic", FirstName = "Pera", LastName = "Peric" };
                AdminUser admin_milan = new AdminUser() { UserName = "milanmilic", FirstName = "Milan", LastName = "Milic" };

                _userManager.Create(admin_pera, "password");
                _userManager.Create(admin_milan, "password");

                _userManager.AddToRole(admin_pera.Id, "admins");
                _userManager.AddToRole(admin_milan.Id, "admins");
                #endregion

                #region Teachers
                //TeacherUser teacher_eva = new TeacherUser() { UserName = "evaras", FirstName = "Eva", LastName = "Ras" };
                //TeacherUser teacher_moma = new TeacherUser() { UserName = "momamomic", FirstName = "Moma", LastName = "Momic" };
                //TeacherUser teacher_rastko = new TeacherUser() { UserName = "rastkocvetkovic", FirstName = "Rastko", LastName = "Cvetkovic" };
                //TeacherUser teacher_mira = new TeacherUser() { UserName = "miramiric", FirstName = "Mira", LastName = "Miric" };
                //TeacherUser teacher_ivan = new TeacherUser() { UserName = "ivantepic", FirstName = "Ivan", LastName = "Tepic" };
                //TeacherUser teacher_petar = new TeacherUser() { UserName = "petarpetric", FirstName = "Petar", LastName = "Petric" };
                //TeacherUser teacher_marko = new TeacherUser() { UserName = "markomarkovic", FirstName = "Marko", LastName = "Markovic" };
                //TeacherUser teacher_jova = new TeacherUser() { UserName = "jovajovic", FirstName = "Jova", LastName = "Jovic" };

                //_userManager.Create(teacher_eva);
                //_userManager.Create(teacher_moma);
                //_userManager.Create(teacher_rastko);
                //_userManager.Create(teacher_mira);
                //_userManager.Create(teacher_ivan);
                //_userManager.Create(teacher_petar);
                //_userManager.Create(teacher_marko);
                //_userManager.Create(teacher_jova);

                //_userManager.AddToRole(teacher_eva.Id, "teachers");
                //_userManager.AddToRole(teacher_moma.Id, "teachers");
                //_userManager.AddToRole(teacher_rastko.Id, "teachers");
                //_userManager.AddToRole(teacher_mira.Id, "teachers");
                //_userManager.AddToRole(teacher_ivan.Id, "teachers");
                //_userManager.AddToRole(teacher_petar.Id, "teachers");
                //_userManager.AddToRole(teacher_marko.Id, "teachers");
                //_userManager.AddToRole(teacher_jova.Id, "teachers");
                #endregion

                #region ClassRooms
                //// --- Class rooms

                //SchoolClass classRoomOne = new SchoolClass() { ClassGrade = 5, Name = "5 A" };
                //SchoolClass classRoomTwo = new SchoolClass() { ClassGrade = 5, Name = "5 B" };
                //SchoolClass classRoomThree = new SchoolClass() { ClassGrade = 6, Name = "6 A" };

                //context.ClassRooms.Add(classRoomOne);
                //context.ClassRooms.Add(classRoomTwo);
                //context.ClassRooms.Add(classRoomThree);

                //context.SaveChanges();
                #endregion

                #region Courses, FOR NOW LET's use these
                //// --- Subjects
                List<Course> courses = new List<Course>();
                courses.Add(new Course() { Name = "Mathematics", ColloqialName = "Matis" });
                courses.Add(new Course() { Name = "Chemistry", ColloqialName = "Hemija" });
                courses.Add(new Course() { Name = "Biology", ColloqialName = "Biologija" });
                courses.Add(new Course() { Name = "Informatics", ColloqialName = "Infroaa" });
                courses.Add(new Course() { Name = "German", ColloqialName = "Physicdfafds 5" });
                courses.Add(new Course() { Name = "History", ColloqialName = "History 6" });
                courses.Add(new Course() { Name = "English", ColloqialName = "Englishaf 6" });


                context.Courses.AddRange(courses);

                context.SaveChanges();
                #endregion

                // HAVE ROLES AND SOME ADMINS

                // CREATE Students.

                List<StudentUser> students = SeederHelper.CreateStudents(20);
                //foreach (var s in students)
                //{
                //    _userManager.Create(s);

                //    // THIS IS CRITICAL!!!! stupid thing won't update id without manually setting it, tsss
                //    s.Id = _userManager.FindByName(s.UserName).Id;

                //    // context.SaveChanges();
                //    Debug.WriteLine(s.FirstName + ", Id: " + s.Id);

                //    _userManager.AddToRole(s.Id, "students");
                //}

                // let's see if manual approach works better...
                foreach (var s in students)
                {
                    context.Users.Add(s);

                    context.SaveChanges();

                    CustomUserRole st = new CustomUserRole() { RoleId = studentRole.Id, UserId = s.Id };
                    // Debug.WriteLine(s.FirstName + ", Id: " + s.Id);
                    context.SaveChanges();
                }

                List<TeacherUser> teachers = SeederHelper.CreateTeachers(10);

                foreach (var t in teachers)
                {
                    context.Users.Add(t);

                    context.SaveChanges();

                    CustomUserRole st = new CustomUserRole() { RoleId = teacherRole.Id, UserId = t.Id };
                    // Debug.WriteLine(t.FirstName + ", Id: " + t.Id);
                    context.SaveChanges();
                }

                List<StudentParent> studentParents = SeederHelper.CreateRealisticParents(students);

                List<ParentUser> parents = studentParents.Select(sp => sp.Parent).Distinct().ToList();

                foreach (var p in parents)
                {
                    context.Users.Add(p);

                    context.SaveChanges();

                    CustomUserRole st = new CustomUserRole() { RoleId = parentRole.Id, UserId = p.Id };
                    // Debug.WriteLine(p.FirstName + ", Id: " + p.Id);
                    context.SaveChanges();
                }

                context.StudentParents.AddRange(studentParents);
                context.SaveChanges();


                #region Teaching (assignments)

                // --- Teaching assignments *** This one could have been added to curriculum....
                List<Teaching> teachings = SeederHelper.AssignTeaching(teachers, courses);

                context.TeachingAssignments.AddRange(teachings);
                context.SaveChanges();

                #endregion
                List<ClassRoom> classes = SeederHelper.CreateSchoolClasses(2, 5, 6);
                context.ClassRooms.AddRange(classes);

                context.SaveChanges();

                // --- PROGRAMS
                SeederHelper.AssignStudentsToClasses(students, classes, 2);
                context.SaveChanges();


                List<Program> programs = SeederHelper.AssignProgram(teachings, classes);
                context.Programs.AddRange(programs);

                context.SaveChanges();

                #region Students
                //StudentUser student_lazaL = new StudentUser() { UserName = "lazalazic", FirstName = "Laza", LastName = "Lazic" };
                //StudentUser student_djokaDj = new StudentUser() { UserName = "djokadjokic", FirstName = "Djoka", LastName = "Djokic" };
                //StudentUser student_zivaZ = new StudentUser() { UserName = "zivazivic", FirstName = "Ziva", LastName = "Zivic" };
                //StudentUser student_ivaI = new StudentUser() { UserName = "ivaivic", FirstName = "Iva", LastName = "Ivic" };
                //StudentUser student_sekaS = new StudentUser() { UserName = "sekasekic", FirstName = "Seka", LastName = "Sekic" };
                //StudentUser student_ruzaR = new StudentUser() { UserName = "ruzaruzic", FirstName = "Ruza", LastName = "Ruzic" };
                //StudentUser student_anaS = new StudentUser() { UserName = "anastanic", FirstName = "Ana", LastName = "Stanic" };
                //StudentUser student_evaT = new StudentUser() { UserName = "evatomic", FirstName = "Eva", LastName = "Tomic" };
                //StudentUser student_evaS = new StudentUser() { UserName = "evasic", FirstName = "Eva", LastName = "Sic" };
                //StudentUser student_anaE = new StudentUser() { UserName = "anaeric", FirstName = "Ana", LastName = "Eric" };
                //StudentUser student_anaM = new StudentUser() { UserName = "anamiric", FirstName = "Ana", LastName = "Miric" };
                //StudentUser student_enaM = new StudentUser() { UserName = "enamirovic", FirstName = "Ena", LastName = "Mirovic" };

                //_userManager.Create(student_lazaL, "password");
                //_userManager.Create(student_djokaDj, "password");
                //_userManager.Create(student_zivaZ, "password");
                //_userManager.Create(student_ivaI, "password");
                //_userManager.Create(student_sekaS, "password");
                //_userManager.Create(student_ruzaR, "password");
                //_userManager.Create(student_anaS, "password");
                //_userManager.Create(student_evaT, "password");
                //_userManager.Create(student_evaS, "password");
                //_userManager.Create(student_anaE, "password");
                //_userManager.Create(student_anaM, "password");
                //_userManager.Create(student_enaM, "password");

                //_userManager.AddToRole(student_lazaL.Id, "students");
                //_userManager.AddToRole(student_djokaDj.Id, "students");
                //_userManager.AddToRole(student_zivaZ.Id, "students");
                //_userManager.AddToRole(student_ivaI.Id, "students");
                //_userManager.AddToRole(student_sekaS.Id, "students");
                //_userManager.AddToRole(student_ruzaR.Id, "students");
                //_userManager.AddToRole(student_anaS.Id, "students");
                //_userManager.AddToRole(student_evaT.Id, "students");
                //_userManager.AddToRole(student_evaS.Id, "students");
                //_userManager.AddToRole(student_anaE.Id, "students");
                //_userManager.AddToRole(student_anaM.Id, "students");
                //_userManager.AddToRole(student_enaM.Id, "students");
                #endregion

                #region Learning (student learning a subject)

                List<Taking> takings = SeederHelper.AssignTakings(students, programs);
                context.Takings.AddRange(takings);
                context.SaveChanges();
                #endregion

                #region Grading (teacher teaching a subject to the student)



                #endregion

                #region Grades 
                List<Grade> grades = SeederHelper.AssignGrades(takings, new DateTime(2018, 9, 1), new DateTime(2018, 12, 31), 1, 2, 5);
                context.Grades.AddRange(grades);
                context.SaveChanges();
                #endregion

                #region Final Grades
                List<FinalGrade> finalGrades = SeederHelper.AssignFinalGrades(takings, new DateTime(2018, 9, 1), new DateTime(2018, 12, 31), 1);
                context.FinalGrades.AddRange(finalGrades);
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
using eGradeBook.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace eGradeBook.Infrastructure
{
    public class GradeBookInitializer : DropCreateDatabaseAlways<GradeBookContext>
    {
        protected override void Seed(GradeBookContext context)
        {
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
            _roleManager.Create(teacherRole);
            _roleManager.Create(teacherRole);
            _roleManager.Create(parentRole);
            #endregion

            #region Admins
            AdminUser admin_pera = new AdminUser() { UserName = "peraperic", FirstName = "Pera", LastName = "Peric" };
            AdminUser admin_milan = new AdminUser() { UserName = "milanmilic", FirstName = "Milan", LastName = "Milic" };

            _userManager.Create(admin_pera);
            _userManager.Create(admin_milan);

            _userManager.AddToRole(admin_pera.Id, "admins");
            _userManager.AddToRole(admin_milan.Id, "admins");
            #endregion

            #region Teachers
            TeacherUser teacher_eva = new TeacherUser() { UserName = "evaras", FirstName = "Eva", LastName = "Ras" };
            TeacherUser teacher_moma = new TeacherUser() { UserName = "momamomic", FirstName = "Moma", LastName = "Momic" };
            TeacherUser teacher_rastko = new TeacherUser() { UserName = "rastkocvetkovic", FirstName = "Rastko", LastName = "Cvetkovic" };
            TeacherUser teacher_mira = new TeacherUser() { UserName = "miramiric", FirstName = "Mira", LastName = "Miric" };
            TeacherUser teacher_ivan = new TeacherUser() { UserName = "ivantepic", FirstName = "Ivan", LastName = "Tepic" };
            TeacherUser teacher_petar = new TeacherUser() { UserName = "petarpetric", FirstName = "Petar", LastName = "Petric" };
            TeacherUser teacher_marko = new TeacherUser() { UserName = "markomarkovic", FirstName = "Marko", LastName = "Markovic" };
            TeacherUser teacher_jova = new TeacherUser() { UserName = "jovajovic", FirstName = "Jova", LastName = "Jovic" };

            _userManager.Create(teacher_eva);
            _userManager.Create(teacher_moma);
            _userManager.Create(teacher_rastko);
            _userManager.Create(teacher_mira);
            _userManager.Create(teacher_ivan);
            _userManager.Create(teacher_petar);
            _userManager.Create(teacher_marko);
            _userManager.Create(teacher_jova);

            _userManager.AddToRole(teacher_eva.Id, "teachers");
            _userManager.AddToRole(teacher_moma.Id, "teachers");
            _userManager.AddToRole(teacher_rastko.Id, "teachers");
            _userManager.AddToRole(teacher_mira.Id, "teachers");
            _userManager.AddToRole(teacher_ivan.Id, "teachers");
            _userManager.AddToRole(teacher_petar.Id, "teachers");
            _userManager.AddToRole(teacher_marko.Id, "teachers");
            _userManager.AddToRole(teacher_jova.Id, "teachers");
            #endregion

            #region ClassRooms
            // --- Class rooms

            SchoolClass classRoomOne = new SchoolClass() { ClassGrade = 5, Name = "5 A" };
            SchoolClass classRoomTwo = new SchoolClass() { ClassGrade = 5, Name = "5 B" };
            SchoolClass classRoomThree = new SchoolClass() { ClassGrade = 6, Name = "6 A" };

            context.ClassRooms.Add(classRoomOne);
            context.ClassRooms.Add(classRoomTwo);
            context.ClassRooms.Add(classRoomThree);

            context.SaveChanges();
            #endregion

            #region Courses
            // --- Subjects

            Course subject_math_5 = new Course() { ClassGrade = 5, Name = "Mathematics", ShortName = "Math 5" };
            Course subject_math_6 = new Course() { ClassGrade = 6, Name = "Mathematics", ShortName = "Math 6" };
            Course subject_biology_5 = new Course() { ClassGrade = 5, Name = "Biology", ShortName = "Biology 5" };
            Course subject_biology_6 = new Course() { ClassGrade = 6, Name = "Biology", ShortName = "Biology 6" };
            Course subject_physics_5 = new Course() { ClassGrade = 5, Name = "Physics", ShortName = "Physics 5" };
            Course subject_physics_6 = new Course() { ClassGrade = 6, Name = "Physics", ShortName = "Physics 6" };

            context.Courses.Add(subject_math_5);
            context.Courses.Add(subject_math_6);
            context.Courses.Add(subject_biology_5);
            context.Courses.Add(subject_biology_6);
            context.Courses.Add(subject_physics_5);
            context.Courses.Add(subject_physics_6);

            context.SaveChanges();
            #endregion

            #region Teaching (assignments)

            // --- Teaching assignments *** This one could have been added to curriculum....


            #endregion
            context.SaveChanges();

            #region Students
            StudentUser student_lazaL = new StudentUser() { UserName = "lazalazic", FirstName = "Laza", LastName = "Lazic" };
            StudentUser student_djokaDj = new StudentUser() { UserName = "djokadjokic", FirstName = "Djoka", LastName = "Djokic" };
            StudentUser student_zivaZ = new StudentUser() { UserName = "zivazivic", FirstName = "Ziva", LastName = "Zivic" };
            StudentUser student_ivaI = new StudentUser() { UserName = "ivaivic", FirstName = "Iva", LastName = "Ivic" };
            StudentUser student_sekaS = new StudentUser() { UserName = "sekasekic", FirstName = "Seka", LastName = "Sekic" };
            StudentUser student_ruzaR = new StudentUser() { UserName = "ruzaruzic", FirstName = "Ruza", LastName = "Ruzic" };
            StudentUser student_anaS = new StudentUser() { UserName = "anastanic", FirstName = "Ana", LastName = "Stanic" };
            StudentUser student_evaT = new StudentUser() { UserName = "evatomic", FirstName = "Eva", LastName = "Tomic" };
            StudentUser student_evaS = new StudentUser() { UserName = "evasic", FirstName = "Eva", LastName = "Sic" };
            StudentUser student_anaE = new StudentUser() { UserName = "anaeric", FirstName = "Ana", LastName = "Eric" };
            StudentUser student_anaM = new StudentUser() { UserName = "anamiric", FirstName = "Ana", LastName = "Miric" };
            StudentUser student_enaM = new StudentUser() { UserName = "enamirovic", FirstName = "Ena", LastName = "Mirovic" };

            _userManager.Create(student_lazaL, "password");
            _userManager.Create(student_djokaDj, "password");
            _userManager.Create(student_zivaZ, "password");
            _userManager.Create(student_ivaI, "password");
            _userManager.Create(student_sekaS, "password");
            _userManager.Create(student_ruzaR, "password");
            _userManager.Create(student_anaS, "password");
            _userManager.Create(student_evaT, "password");
            _userManager.Create(student_evaS, "password");
            _userManager.Create(student_anaE, "password");
            _userManager.Create(student_anaM, "password");
            _userManager.Create(student_enaM, "password");

            _userManager.AddToRole(student_lazaL.Id, "students");
            _userManager.AddToRole(student_djokaDj.Id, "students");
            _userManager.AddToRole(student_zivaZ.Id, "students");
            _userManager.AddToRole(student_ivaI.Id, "students");
            _userManager.AddToRole(student_sekaS.Id, "students");
            _userManager.AddToRole(student_ruzaR.Id, "students");
            _userManager.AddToRole(student_anaS.Id, "students");
            _userManager.AddToRole(student_evaT.Id, "students");
            _userManager.AddToRole(student_evaS.Id, "students");
            _userManager.AddToRole(student_anaE.Id, "students");
            _userManager.AddToRole(student_anaM.Id, "students");
            _userManager.AddToRole(student_enaM.Id, "students");
            #endregion

            #region Learning (student learning a subject)

            #endregion

            #region Grading (teacher teaching a subject to the student)

            #endregion

            #region Grades 

            #endregion

            base.Seed(context);
        }
    }
}
using eGradeBook.Models;
using Microsoft.AspNet.Identity;
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
            CustomRole adminRole = new CustomRole() { Name = "admins" };
            CustomRole studentRole = new CustomRole() { Name = "students" };
            CustomRole teacherRole = new CustomRole() { Name = "teachers" };
            CustomRole parentRole = new CustomRole() { Name = "parents" };
            CustomRole classMasterRole = new CustomRole() { Name = "classmasters" };

            context.Roles.Add(adminRole);
            context.Roles.Add(studentRole);
            context.Roles.Add(teacherRole);
            context.Roles.Add(parentRole);
            context.Roles.Add(classMasterRole);

            context.SaveChanges();

            AdminUser adminOne = new AdminUser() { UserName = "adminOne", FirstName = "adminOne-firstName", LastName = "adminOne-lastName" };

            StudentUser studentOne = new StudentUser() { UserName = "studentOne", FirstName = "studentOne-firstName", LastName = "studentOne-lastName" };
            StudentUser studentTwo = new StudentUser() { UserName = "studentTwo", FirstName = "studentTwo-firstName", LastName = "studentTwo-lastName" };
            StudentUser studentThree = new StudentUser() { UserName = "studentThree", FirstName = "studentThree-firstName", LastName = "studentThree-lastName" };
            StudentUser studentFour = new StudentUser() { UserName = "studentFour", FirstName = "studentFour-firstName", LastName = "studentFour-lastName" };
            StudentUser studentFive = new StudentUser() { UserName = "studentFive", FirstName = "studentFive-firstName", LastName = "studentFive-lastName" };
            StudentUser studentSix = new StudentUser() { UserName = "studentSix", FirstName = "studentSix-firstName", LastName = "studentSix-lastName" };
            StudentUser studentSeven = new StudentUser() { UserName = "studentSeven", FirstName = "studentSeven-firstName", LastName = "studentSeven-lastName" };
            StudentUser studentEight = new StudentUser() { UserName = "studentEight", FirstName = "studentEight-firstName", LastName = "studentEight-lastName" };

            PasswordHasher hasher = new PasswordHasher();
            string password = hasher.HashPassword("password");

            TeacherUser teacherOne = new TeacherUser() { PasswordHash = password, UserName = "teacherOne", FirstName = "teacherOne-firstName", LastName = "teacherOne-lastName" };

            TeacherUser teacherTwo = new TeacherUser() { UserName = "teacherTwo", FirstName = "teacherTwo-firstName", LastName = "teacherTwo-lastName" };
            TeacherUser teacherThree = new TeacherUser() { UserName = "teacherThree", FirstName = "teacherThree-firstName", LastName = "teacherThree-lastName" };
            TeacherUser teacherFour = new TeacherUser() { UserName = "teacherFour", FirstName = "teacherFour-firstName", LastName = "teacherFour-lastName" };
            TeacherUser teacherFive = new TeacherUser() { UserName = "teacherFive", FirstName = "teacherFive-firstName", LastName = "teacherFive-lastName" };
            TeacherUser teacherSix = new TeacherUser() { UserName = "teacherSix", FirstName = "teacherSix-firstName", LastName = "teacherSix-lastName" };
            TeacherUser teacherSeven = new TeacherUser() { UserName = "teacherSeven", FirstName = "teacherSeven-firstName", LastName = "teacherSeven-lastName" };
            TeacherUser teacherEight = new TeacherUser() { UserName = "teacherEight", FirstName = "teacherEight-firstName", LastName = "teacherEight-lastName" };

            context.Users.Add(adminOne);

            context.Users.Add(studentOne);
            context.Users.Add(studentTwo);
            context.Users.Add(studentThree);
            context.Users.Add(studentFour);
            context.Users.Add(studentFive);
            context.Users.Add(studentSix);
            context.Users.Add(studentSeven);
            context.Users.Add(studentEight);

            context.Users.Add(teacherOne);
            context.Users.Add(teacherTwo);
            context.Users.Add(teacherThree);
            context.Users.Add(teacherFour);
            context.Users.Add(teacherFive);
            context.Users.Add(teacherSix);
            context.Users.Add(teacherSeven);
            context.Users.Add(teacherEight);

            context.SaveChanges();

            CustomUserRole cur100 = new CustomUserRole() { RoleId = adminRole.Id, UserId = adminOne.Id };

            CustomUserRole cur101 = new CustomUserRole() { RoleId = studentRole.Id, UserId = studentOne.Id };
            CustomUserRole cur102 = new CustomUserRole() { RoleId = studentRole.Id, UserId = studentTwo.Id };
            CustomUserRole cur103 = new CustomUserRole() { RoleId = studentRole.Id, UserId = studentThree.Id };
            CustomUserRole cur104 = new CustomUserRole() { RoleId = studentRole.Id, UserId = studentFour.Id };
            CustomUserRole cur105 = new CustomUserRole() { RoleId = studentRole.Id, UserId = studentFive.Id };
            CustomUserRole cur106 = new CustomUserRole() { RoleId = studentRole.Id, UserId = studentSix.Id };
            CustomUserRole cur107 = new CustomUserRole() { RoleId = studentRole.Id, UserId = studentSeven.Id };
            CustomUserRole cur108 = new CustomUserRole() { RoleId = studentRole.Id, UserId = studentEight.Id };

            CustomUserRole cur111 = new CustomUserRole() { RoleId = teacherRole.Id, UserId = teacherOne.Id };
            CustomUserRole cur112 = new CustomUserRole() { RoleId = teacherRole.Id, UserId = teacherTwo.Id };
            CustomUserRole cur113 = new CustomUserRole() { RoleId = teacherRole.Id, UserId = teacherThree.Id };
            CustomUserRole cur114 = new CustomUserRole() { RoleId = teacherRole.Id, UserId = teacherFour.Id };
            CustomUserRole cur115 = new CustomUserRole() { RoleId = teacherRole.Id, UserId = teacherFive.Id };
            CustomUserRole cur116 = new CustomUserRole() { RoleId = teacherRole.Id, UserId = teacherSix.Id };
            CustomUserRole cur117 = new CustomUserRole() { RoleId = teacherRole.Id, UserId = teacherSeven.Id };
            CustomUserRole cur118 = new CustomUserRole() { RoleId = teacherRole.Id, UserId = teacherEight.Id };

            context.Set<CustomUserRole>().Add(cur100);
            context.Set<CustomUserRole>().Add(cur101);
            context.Set<CustomUserRole>().Add(cur102);
            context.Set<CustomUserRole>().Add(cur103);
            context.Set<CustomUserRole>().Add(cur104);
            context.Set<CustomUserRole>().Add(cur105);
            context.Set<CustomUserRole>().Add(cur106);
            context.Set<CustomUserRole>().Add(cur107);
            context.Set<CustomUserRole>().Add(cur108);
            context.Set<CustomUserRole>().Add(cur111);
            context.Set<CustomUserRole>().Add(cur112);
            context.Set<CustomUserRole>().Add(cur113);
            context.Set<CustomUserRole>().Add(cur114);
            context.Set<CustomUserRole>().Add(cur115);
            context.Set<CustomUserRole>().Add(cur116);
            context.Set<CustomUserRole>().Add(cur117);
            context.Set<CustomUserRole>().Add(cur118);

            context.SaveChanges();

            // --- Class rooms

            SchoolClass classRoomOne = new SchoolClass() { ClassGrade = 5, Name = "5 A" };
            SchoolClass classRoomTwo = new SchoolClass() { ClassGrade = 5, Name = "5 B" };
            SchoolClass classRoomThree = new SchoolClass() { ClassGrade = 6, Name = "6 A" };

            context.ClassRooms.Add(classRoomOne);
            context.ClassRooms.Add(classRoomTwo);
            context.ClassRooms.Add(classRoomThree);

            context.SaveChanges();

            // --- Subjects

            Course subjectMath5 = new Course() { ClassGrade = 5, Name = "Mathematics", ShortName = "Math 5" };
            Course subjectMath6 = new Course() { ClassGrade = 6, Name = "Mathematics", ShortName = "Math 6" };
            Course subjectBiology5 = new Course() { ClassGrade = 5, Name = "Biology", ShortName = "Biology 5" };
            Course subjectBiology6 = new Course() { ClassGrade = 6, Name = "Biology", ShortName = "Biology 6" };
            Course subjectPhysics5 = new Course() { ClassGrade = 5, Name = "Physics", ShortName = "Physics 5" };
            Course subjectPhysics6 = new Course() { ClassGrade = 6, Name = "Physics", ShortName = "Physics 6" };

            context.Subjects.Add(subjectMath5);
            context.Subjects.Add(subjectMath6);
            context.Subjects.Add(subjectBiology5);
            context.Subjects.Add(subjectBiology6);
            context.Subjects.Add(subjectPhysics5);
            context.Subjects.Add(subjectPhysics6);

            context.SaveChanges();

            // --- Curriculum

            Curriculum cur1 = new Curriculum() { ClassRoom = classRoomOne, Subject = subjectMath5, HoursPerWeek = 4 };
            Curriculum cur2 = new Curriculum() { ClassRoom = classRoomOne, Subject = subjectBiology5, HoursPerWeek = 3 };
            Curriculum cur3 = new Curriculum() { ClassRoom = classRoomOne, Subject = subjectPhysics5, HoursPerWeek = 5 };
            Curriculum cur4 = new Curriculum() { ClassRoom = classRoomTwo, Subject = subjectMath5, HoursPerWeek = 4 };
            Curriculum cur5 = new Curriculum() { ClassRoom = classRoomTwo, Subject = subjectBiology5, HoursPerWeek = 4 };
            Curriculum cur6 = new Curriculum() { ClassRoom = classRoomThree, Subject = subjectMath6, HoursPerWeek = 4 };
            Curriculum cur7 = new Curriculum() { ClassRoom = classRoomThree, Subject = subjectPhysics6, HoursPerWeek = 3 };

            context.Curricula.Add(cur1);
            context.Curricula.Add(cur2);
            context.Curricula.Add(cur3);
            context.Curricula.Add(cur4);
            context.Curricula.Add(cur5);
            context.Curricula.Add(cur6);
            context.Curricula.Add(cur7);

            context.SaveChanges();

            // --- Teaching assignments *** This one could have been added to curriculum....

            Teaching ta1 = new Teaching() { Teacher = teacherOne, Subject = subjectBiology5, ClassRoom = classRoomOne };
            Teaching ta2 = new Teaching() { Teacher = teacherTwo, Subject = subjectBiology6, ClassRoom = classRoomThree };
            Teaching ta3 = new Teaching() { Teacher = teacherThree, Subject = subjectMath5, ClassRoom = classRoomTwo };
            Teaching ta4 = new Teaching() { Teacher = teacherFour, Subject = subjectMath6, ClassRoom = classRoomThree };

            context.TeachingAssignments.Add(ta1);
            context.TeachingAssignments.Add(ta2);
            context.TeachingAssignments.Add(ta3);

            context.SaveChanges();

            // --- Let's try to add students to classes (that is classes to students...)

            studentOne.ClassRoom = classRoomOne;
            studentTwo.ClassRoom = classRoomOne;
            studentThree.ClassRoom = classRoomTwo;
            studentFour.ClassRoom = classRoomThree;
            context.SaveChanges();

            // --- Grades

            // For studentOne from classRoomOne 
            Grade gradeOne = new Grade() { Assigned = DateTime.UtcNow, Subject = subjectMath5, GradePoint = 1, Student = studentOne, Teacher = teacherThree };
            Grade gradeTwo = new Grade() { Assigned = DateTime.UtcNow, Subject = subjectMath5, GradePoint = 5, Student = studentOne, Teacher = teacherThree };
            Grade gradeThree = new Grade() { Assigned = DateTime.UtcNow, Subject = subjectBiology5, GradePoint = 1, Student = studentOne, Teacher = teacherOne };

            // For studentTwo from classRoomOne
            Grade gradeFour = new Grade() { Assigned = DateTime.UtcNow, Subject = subjectMath5, GradePoint = 4, Student = studentTwo, Teacher = teacherThree };
            Grade gradeFive = new Grade() { Assigned = DateTime.UtcNow, Subject = subjectMath5, GradePoint = 5, Student = studentTwo, Teacher = teacherThree };
            Grade gradeSix = new Grade() { Assigned = DateTime.UtcNow, Subject = subjectBiology5, GradePoint = 1, Student = studentTwo, Teacher = teacherOne };

            // For studentFour from ClassRoomThree
            Grade gradeSeven = new Grade() { Assigned = DateTime.UtcNow, Subject = subjectMath6, GradePoint = 3, Student = studentFour, Teacher = teacherFour };
            Grade gradeEight = new Grade() { Assigned = DateTime.UtcNow, Subject = subjectBiology6, GradePoint = 5, Student = studentFour, Teacher = teacherTwo };

            context.Grades.Add(gradeOne);
            context.Grades.Add(gradeTwo);
            context.Grades.Add(gradeThree);
            context.Grades.Add(gradeFour);
            context.Grades.Add(gradeFive);
            context.Grades.Add(gradeSix);
            context.Grades.Add(gradeSeven);
            context.Grades.Add(gradeEight);
            context.SaveChanges();

            // Add a few parents
            ParentUser parentOne = new ParentUser() { PasswordHash = password, UserName = "ParentOne", FirstName = "firstname", LastName = "lastname" };
            ParentUser parentTwo = new ParentUser() { UserName = "ParentTwo", FirstName = "firstname", LastName = "lastname" };
            ParentUser parentThree = new ParentUser() { UserName = "ParentThree", FirstName = "firstname", LastName = "lastname" };
            ParentUser parentFour = new ParentUser() { UserName = "ParentFour", FirstName = "firstname", LastName = "lastname" };

            parentOne.Children.Add(studentOne);
            parentOne.Children.Add(studentTwo);
            parentTwo.Children.Add(studentTwo);
            parentTwo.Children.Add(studentThree);
            parentThree.Children.Add(studentFour);

            context.Users.Add(parentOne);
            context.Users.Add(parentTwo);
            context.Users.Add(parentThree);

            context.SaveChanges();

            // dont forget the roles!

            CustomUserRole cur141 = new CustomUserRole() { RoleId = parentRole.Id, UserId = parentOne.Id };
            CustomUserRole cur142 = new CustomUserRole() { RoleId = parentRole.Id, UserId = parentTwo.Id };
            CustomUserRole cur143 = new CustomUserRole() { RoleId = parentRole.Id, UserId = parentThree.Id };

            // and dont forget to add them to context
            context.Set<CustomUserRole>().Add(cur141);
            context.Set<CustomUserRole>().Add(cur142);
            context.Set<CustomUserRole>().Add(cur143);

            context.SaveChanges();

            base.Seed(context);
        }
    }
}
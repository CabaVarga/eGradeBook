using eGradeBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Infrastructure
{
    public static class SeederHelper
    {
        public static List<string> maleNames;
        public static List<string> femaleNames;
        public static List<string> lastNames;
        public static Random random;
        public static List<string> userNames;

        static SeederHelper()
        {
            random = new Random(100);
            maleNames = new List<string>() { "Adam", "Vuk", "Luka", "Uros", "Sava", "Ivan", "Relja", "Igor", "Sasa", "Vanja", "Toma", "Milan" };
            femaleNames = new List<string>() { "Iva", "Lea", "Mia", "Masa", "Tina", "Eva", "Ena", "Ana", "Ema", "Minja", "Sanja", "Sara", "Lara", "Lenka", "Tanja" };
            lastNames = new List<string>() { "Sic", "Vulic", "Mihic", "Ivic", "Ilic", "Savic", "Tomic", "Laric", "Antic", "Arsic", "Bajac", "Bobic", "Buha", "Beric", "Bilic", "Caric", "Cvejin", "Cuk", "Ciric", "Pantic", "Pesic" };
            userNames = new List<string>();
        }

        public static string Randomize(List<string> list)
        {
            return list.ElementAt(random.Next(list.Count));
        }

        public static List<StudentUser> CreateStudents(int howMany)
        {
            List<StudentUser> students = new List<StudentUser>();
            Dictionary<string, string> values = new Dictionary<string, string>();

            for (int i = 0; i < howMany; i++)
            {
                values = CreateName();

                students.Add(new StudentUser() {
                    FirstName = values["firstname"],
                    LastName = values["lastname"],
                    UserName = values["username"],
                    Gender = values["gender"]
                });
            }

            return students;
        }

        public static List<TeacherUser> CreateTeachers(int howMany)
        {
            List<TeacherUser> teachers = new List<TeacherUser>();
            Dictionary<string, string> values = new Dictionary<string, string>();

            for (int i = 0; i < howMany; i++)
            {
                values = CreateName();

                teachers.Add(new TeacherUser()
                {
                    FirstName = values["firstname"],
                    LastName = values["lastname"],
                    UserName = values["username"],
                    Gender = values["gender"]
                });
            }

            return teachers;
        }

        public static List<ParentUser> CreateParents(int howMany)
        {
            List<ParentUser> parents = new List<ParentUser>();
            Dictionary<string, string> values = new Dictionary<string, string>();

            for (int i = 0; i < howMany; i++)
            {
                values = CreateName();

                parents.Add(new ParentUser()
                {
                    FirstName = values["firstname"],
                    LastName = values["lastname"],
                    UserName = values["username"],
                    Gender = values["gender"]
                });
            }

            return parents;
        }

        // FOR REALISTIC PARENT - STUDENT CONNECTION:
        // CREATE BY STUDENT LASTNAME, occassionally mix things up... like they are divorced or something ... 2 siblings with 3 parents for example
        // for same lastname sometimes same parents, sometimes a mix, sometimes two different pairs of parents
        // sometimes instead of new parent add an existing parent into a mix, like in 5% of the time?
        // not an easy task....

        public static List<Teaching> AssignTeaching(List<TeacherUser> teachers, List<Course> courses)
        {
            // This also can't be completely random... if someone teaches math, the weight for teaching another math or physics is greater...
            List<Teaching> assignments = new List<Teaching>();
            Deal<Course> dealer = new Deal<Course>(courses);

            // the trivial approach
            foreach (var t in teachers)
            {
                int numberOfCoursesTeached = random.Next(3);

                for (int i = 0; i <= numberOfCoursesTeached; i++)
                {
                    // Need to prevent multiples...
                    // A 'Take' would be much better... but that one needs state
                    // Also 'Shuffle'
                    assignments.Add(new Teaching()
                    {
                        Teacher = t,
                        // Subject = courses.ElementAt(random.Next(courses.Count()))
                        Subject = dealer.DealOne()
                    });
                }
            }

            return assignments;
        }

        public static Dictionary<string, string> CreateName()
        {
            Dictionary<string, string> valuePairs = new Dictionary<string, string>();

            valuePairs["gender"] = random.Next(2) < 2 ? "m" : "f";
            valuePairs["firstname"] = valuePairs["gender"] == "f" ? Randomize(femaleNames) : Randomize(maleNames);
            valuePairs["lastname"] = Randomize(lastNames);
            valuePairs["username"] = valuePairs["firstname"].ToLower() + valuePairs["lastname"].ToLower();

            valuePairs["username"] = UserNameTakenCheck(valuePairs["username"]);

            return valuePairs;
        }

        public static string UserNameTakenCheck(string username)
        {
            // we will need this for numbering
            int number = 1;

            var taken = userNames.Contains(username);

            if (taken)
            {
                // is there any username2, 3 and so on?
                if (userNames.Exists(u => u.StartsWith(username) && !u.EndsWith(username)))
                {
                    // let's find out max number then
                    number = userNames.Where(u => u.StartsWith(username) && !u.EndsWith(username))
                        // Remove all alphabetic characters, basically
                        .Select(un => int.Parse(un.TrimStart(username.ToCharArray())))
                        .Max();
                }

                username = username + (number + 1);
            }

            // We can safely add it here....
            userNames.Add(username);

            return username;
        }
    }
}
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

        public static List<SchoolClass> CreateSchoolClasses(int howManyPerGrade = 2, int lowGrade = 5, int highGrade = 8)
        {
            List<SchoolClass> classes = new List<SchoolClass>();
            for (int grade = lowGrade; grade <= highGrade; grade++)
            {
                for (int cnt = 1; cnt <= howManyPerGrade; cnt++)
                {
                    var schoolClass = new SchoolClass()
                    {
                        ClassGrade = grade,
                        Name = grade + "-" + cnt
                    };

                    classes.Add(schoolClass);
                }
            }
            return classes;
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

        public static void AssignStudentsToClasses(List<StudentUser> students, List<SchoolClass> classes, int minPerClass = 0, int maxPerClas = int.MaxValue)
        {
            int classCount = classes.Count();

            foreach(var s in students)
            {
                s.SchoolClass = classes.ElementAt(random.Next(classCount));
            }

            // What about context save? we'll see if it works...
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

        public static List<ParentUser> CreateRealisticParents(List<StudentUser> students)
        {
            List<ParentUser> parents = new List<ParentUser>();

            // group students by lastname
            var studentsByLastname = students.GroupBy(s => s.LastName);

            // go by group
            foreach(var sg in studentsByLastname)
            {
                var parentsWithSameLastname = new List<ParentUser>();

                // one possibility: they all have the same parents...
                var fatherOne = new ParentUser()
                {
                    FirstName = Randomize(maleNames),
                    LastName = random.Next(100) < 5 ? Randomize(lastNames) : sg.Key
                };

                var username = fatherOne.FirstName.ToLower() + fatherOne.LastName.ToLower();
                username = UserNameTakenCheck(username);
                fatherOne.UserName = username;

                var motherOne = new ParentUser()
                {
                    FirstName = Randomize(femaleNames),
                    LastName = random.Next(100) < 5 ? Randomize(lastNames) : sg.Key
                };

                username = motherOne.FirstName.ToLower() + motherOne.LastName.ToLower();
                username = UserNameTakenCheck(username);
                motherOne.UserName = username;

                parentsWithSameLastname.Add(fatherOne);
                parentsWithSameLastname.Add(motherOne);
                
                foreach(var s in sg)
                {
                    // second: some of them have the same parents...
                    ParentUser father;

                    if (random.Next(100) < 50)
                    {
                        father = parentsWithSameLastname.ElementAt(random.Next(parentsWithSameLastname.Count));

                        father.Children.Add(s);
                    }
                    else
                    {
                        father = new ParentUser()
                        {
                            Gender = "m",
                            FirstName = Randomize(maleNames),
                            LastName = random.Next(100) < 5 ? Randomize(lastNames) : sg.Key
                        };

                        username = father.FirstName.ToLower() + father.LastName.ToLower();
                        username = UserNameTakenCheck(username);
                        father.UserName = username;

                        father.Children.Add(s);

                        parentsWithSameLastname.Add(father);
                    }


                    // The mother
                    ParentUser mother;

                    if (random.Next(100) < 50)
                    {
                        mother = parentsWithSameLastname.ElementAt(random.Next(parentsWithSameLastname.Count));

                        mother.Children.Add(s);
                    }

                    else
                    {
                        mother = new ParentUser()
                        {
                            Gender = "f",
                            FirstName = Randomize(femaleNames),
                            LastName = random.Next(100) < 5 ? Randomize(lastNames) : sg.Key
                        };

                        username = mother.FirstName.ToLower() + mother.LastName.ToLower();
                        username = UserNameTakenCheck(username);
                        mother.UserName = username;

                        mother.Children.Add(s);

                        parentsWithSameLastname.Add(mother);
                    }
                }


                // and last but not the least, some of them have parents with different lastnames
                // some of those parents are also parents of some other children...

                // THAT NEEDS TO BE ADDED UP THERE


                // Add parents to parents...
                parents.AddRange(parentsWithSameLastname);
            }

            return parents;
        }

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
                        Course = dealer.DealOne()
                    });
                }
            }

            return assignments;
        }

        public static List<Program> AssignProgram(List<Teaching> teachings, List<SchoolClass> classes)
        {
            List<Program> programs = new List<Program>();

            // we need the subjects...
            var courses = teachings.Select(t => t.Course).Distinct();

            foreach (var c in classes)
            {
                // for every SchoolClass
                foreach (var s in courses)
                {
                    // 0. The random element. 10% chance of skipping the course
                    if (random.Next(100) < 10)
                    {
                        continue;
                    }

                    // 1. assign course.

                    // 2. find teachers teaching the course and assign one of them.
                    var teachers = teachings.Where(t => t.Course.Id == c.Id).Select(t => t.Teacher);

                    int teachersPerCourse = teachings.Where(t => t.Course.Id == c.Id).Count();

                    if (teachersPerCourse == 0)
                    {
                        throw new Exception("There are no assigned teachers for the course");
                    }

                    int theRandom = random.Next(teachersPerCourse);

                    var program = new Program()
                    {
                        SchoolClass = c,
                        Course = s,
                        Teaching = teachings.Where(t => t.Course.Id == c.Id).ElementAt(theRandom),
                        WeeklyFund = random.Next(1, 6)
                    };

                    programs.Add(program);                    
                }
            }

            return programs;
        }

        public static List<Taking> AssignTakings(List<StudentUser> students, List<Program> programs)
        {
            List<Taking> takings = new List<Taking>();

            // for each student find the relevant programs...
            foreach(var s in students)
            {
                var programsCanTake = programs.Where(p => p.SchoolClass == s.SchoolClass);

                // how many are there?
                int programsCount = programsCanTake.Count();

                if (programsCount == 0)
                {
                    // throw an exception?
                    // certainly not a desirable state
                    throw new Exception("SchoolClass does not have any programs assigned");
                }

                // let's assume student has 90% chance to take any program offered by the class
                foreach(var p in programsCanTake)
                {
                    if (random.Next(100) < 10)
                    {
                        continue;
                    }

                    var taking = new Taking()
                    {
                        Student = s,
                        Program = p
                    };

                    takings.Add(taking);
                }
            }

            return takings;
        }


        // OK grades are super easy...
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
using eGradeBook.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Infrastructure
{
    public static class SeederHelper
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

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

        /// <summary>
        /// Create a list of School classes (Classrooms).
        /// </summary>
        /// <param name="howManyPerGrade">How many classrooms per school grade (default is 2)</param>
        /// <param name="lowGrade">Starting school grade (default is 5)</param>
        /// <param name="highGrade">Ending school grade (default is 8)</param>
        /// <returns>A list of created school classes</returns>
        public static List<ClassRoom> CreateSchoolClasses(int howManyPerGrade = 2, int lowGrade = 5, int highGrade = 8)
        {
            List<ClassRoom> classes = new List<ClassRoom>();
            for (int grade = lowGrade; grade <= highGrade; grade++)
            {
                for (int cnt = 1; cnt <= howManyPerGrade; cnt++)
                {
                    var schoolClass = new ClassRoom()
                    {
                        ClassGrade = grade,
                        Name = grade + "-" + cnt
                    };

                    classes.Add(schoolClass);
                }
            }
            return classes;
        }

        /// <summary>
        /// Create a list of students.
        /// </summary>
        /// <param name="howMany">How many students should be created</param>
        /// <returns>A list of students</returns>
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

        /// <summary>
        /// Assign students to classes. IMPORTANT: The current implementation does not take into account the min and max values
        /// </summary>
        /// <param name="students">A list of students to assign</param>
        /// <param name="classes">A list of classes where students should be assigned</param>
        /// <param name="minPerClass">The minimum number of students a class must have (default is 0)</param>
        /// <param name="maxPerClas">The maximum number of students a class can have (default is unlimited)</param>
        public static void AssignStudentsToClasses(List<StudentUser> students, List<ClassRoom> classes, int minPerClass = 0, int maxPerClas = int.MaxValue)
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

        public static List<StudentParent> CreateRealisticParents(List<StudentUser> students)
        {
            List<StudentParent> studentParents = new List<StudentParent>();

            List<ParentUser> parents = new List<ParentUser>();

            // group students by lastname
            var studentsByLastname = students.GroupBy(s => s.LastName);

            // go by group
            foreach(var sg in studentsByLastname)
            {
                var parentsWithSameLastname = new List<ParentUser>();

                // one possibility: they all have the same parents...

                // --- Init father
                var fatherOne = new ParentUser()
                {
                    Gender = "m",
                    FirstName = Randomize(maleNames),
                    LastName = random.Next(100) < 5 ? Randomize(lastNames) : sg.Key
                };

                var username = fatherOne.FirstName.ToLower() + fatherOne.LastName.ToLower();
                username = UserNameTakenCheck(username);
                fatherOne.UserName = username;


                // --- Init mother
                var motherOne = new ParentUser()
                {
                    Gender = "f",
                    FirstName = Randomize(femaleNames),
                    LastName = random.Next(100) < 5 ? Randomize(lastNames) : sg.Key
                };

                username = motherOne.FirstName.ToLower() + motherOne.LastName.ToLower();
                username = UserNameTakenCheck(username);
                motherOne.UserName = username;

                parentsWithSameLastname.Add(fatherOne);
                parentsWithSameLastname.Add(motherOne);
                

                // --- at this point we have two entries
                foreach(var s in sg)
                {
                    var studentParentFather = new StudentParent();
                    var studentParentMother = new StudentParent();

                    // second: some of them have the same parents...
                    ParentUser father;

                    if (random.Next(100) < 50)
                    {
                        father = parentsWithSameLastname.ElementAt(random.Next(parentsWithSameLastname.Count));

                        studentParentFather.Parent = father;
                        studentParentFather.Student = s;
                        studentParents.Add(studentParentFather);
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

                        studentParentFather.Parent = father;
                        studentParentFather.Student = s;
                        studentParents.Add(studentParentFather);


                        parentsWithSameLastname.Add(father);
                    }


                    // The mother
                    ParentUser mother;

                    if (random.Next(100) < 50)
                    {
                        mother = parentsWithSameLastname.ElementAt(random.Next(parentsWithSameLastname.Count));

                        studentParentMother.Parent = mother;
                        studentParentMother.Student = s;

                        studentParents.Add(studentParentMother);

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

                        studentParentMother.Parent = mother;
                        studentParentMother.Student = s;
                        studentParents.Add(studentParentMother);

                        parentsWithSameLastname.Add(mother);
                    }
                }


                // and last but not the least, some of them have parents with different lastnames
                // some of those parents are also parents of some other children...

                // THAT NEEDS TO BE ADDED UP THERE


                // Add parents to parents...
                parents.AddRange(parentsWithSameLastname);
            }

            return studentParents;
        }


        /// <summary>
        /// Assign courses to teachers
        /// </summary>
        /// <param name="teachers">List of teachers</param>
        /// <param name="courses">List of courses</param>
        /// <returns></returns>
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

        /// <summary>
        /// Create programs (teacher, course and classroom) for every classroom
        /// </summary>
        /// <param name="teachings">List of course and teacher combos</param>
        /// <param name="classes">List of classrooms</param>
        /// <returns></returns>
        public static List<Program> AssignProgram(List<Teaching> teachings, List<ClassRoom> classes)
        {
            List<Program> programs = new List<Program>();

            // as long as we are dealing with both course and teaching connected.
            // TODO change this
            // we need the courses...
            var courses = teachings.Select(t => t.Course).Distinct();

            // For each classroom
            foreach (var c in classes)
            {
                // Build the program...
                // for every course
                foreach (var course in courses)
                {
                    try
                    {
                        // 0. The random element. 10% chance of skipping the course
                        if (random.Next(100) < 10)
                        {
                            continue;
                        }

                        // 1. assign course.

                        // 2. find teachers teaching the course and assign one of them.
                        var teachers = teachings.Where(t => t.Course.Id == course.Id).Select(t => t.Teacher);

                        int teachersPerCourse = teachings.Where(t => t.Course.Id == course.Id).Count();

                        if (teachersPerCourse == 0)
                        {
                            throw new Exception("There are no assigned teachers for the course");
                        }

                        // In case of multiple teachers teaching the same course
                        // We need to pick one of them, randomly
                        int theRandom = random.Next(teachersPerCourse);

                        var program = new Program()
                        {
                            ClassRoom = c,
                            // TODO we will not attach both course and teaching
                            // only the teaching
                            Course = course,
                            Teaching = teachings.Where(t => t.Course.Id == course.Id).ElementAt(theRandom),
                            WeeklyFund = random.Next(1, 6)
                        };

                        programs.Add(program);
                    }
                    catch (Exception ex)
                    {
                        logger.Debug(ex, "We have found the culprit!");
                        throw ex;
                    }
                }
            }

            return programs;
        }

        /// <summary>
        /// Assign program items (courses of one class) to students
        /// </summary>
        /// <param name="students">List of students</param>
        /// <param name="programs">List of program items</param>
        /// <returns></returns>
        public static List<Taking> AssignTakings(List<StudentUser> students, List<Program> programs)
        {
            List<Taking> takings = new List<Taking>();

            // for each student find the relevant programs...
            foreach(var s in students)
            {
                var programsCanTake = programs.Where(p => p.ClassRoom == s.SchoolClass);

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

        public static List<Grade> AssignGrades(List<Taking> takings, 
            DateTime startPeriod, DateTime endPeriod, 
            int semester = 1, int howManyMin = 1, int howManyMax = 5)
        {
            List<Grade> grades = new List<Grade>();

            var daysbetween = (endPeriod - startPeriod).Days;

            foreach (var taking in takings)
            {
                for (int i = howManyMin; i <= howManyMax; i++)
                {
                    var date = startPeriod + TimeSpan.FromDays(random.Next(daysbetween));
                    var gradePoint = random.Next(1, 6);

                    var grade = new Grade()
                    {
                        Taking = taking,
                        Assigned = date,
                        GradePoint = gradePoint,
                        SchoolTerm = semester
                    };

                    grades.Add(grade);
                }
            }

            return grades;
        }


        public static List<FinalGrade> AssignFinalGrades(List<Taking> takings,
            DateTime startPeriod, DateTime endPeriod,
            int semester = 1)
        {
            List<FinalGrade> grades = new List<FinalGrade>();

            // Some checks would be in order here...
            var daysbetween = (endPeriod - startPeriod).Days;
            // and so on...

            // the real logic stuff should be in giving a final grade by hand!
            foreach (var taking in takings)
            {
                // we need an average

                // oops! for 2nd semester all grades are counting in the average!
                double gpa;

                if (semester == 1)
                {
                    gpa = taking.Grades.Where(g => g.SchoolTerm == semester).Average(g => g.GradePoint);
                }
                else
                {
                    gpa = taking.Grades.Average(g => g.GradePoint);
                }

                int gpaInt = (int)Math.Round(gpa);

                var grade = new FinalGrade()
                {
                    GradePoint = gpaInt,
                    Assigned = endPeriod,
                    SchoolTerm = semester,
                    Taking = taking
                };

                grades.Add(grade);
            }

            return grades;
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
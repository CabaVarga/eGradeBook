using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.ClassRooms
{
    /// <summary>
    /// Dto object with ClassRoom information
    /// </summary>
    public class ClassRoomDto
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ClassRoomDto()
        {
            this.Students = new List<ClassRoomStudentDto>();
            this.Program = new List<ClassRoomProgramDto>();
        }

        /// <summary>
        /// Id of the Class Room
        /// </summary>
        public int ClassRoomId { get; set; }

        /// <summary>
        /// Every classroom has a name (5A, or 5-1)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Every classroom belongs to a given grade (1-8). We are not dealing with so called combined classrooms here.
        /// </summary>
        public int SchoolGrade { get; set; }


        /// <summary>
        /// The students belonging to the classroom
        /// </summary>
        public List<ClassRoomStudentDto> Students { get; set; }

        /// <summary>
        /// The list of courses associated with the classroom
        /// </summary>
        public List<ClassRoomProgramDto> Program { get; set; }

        /// <summary>
        /// A classroom has students. 
        /// We are using an embedded dto with less details than a full StudentDto. 
        /// Not sure if it's an OK approach, though.
        /// </summary>
        public class ClassRoomStudentDto
        {
            /// <summary>
            /// Id of the student, for direct access
            /// </summary>
            public int StudentId { get; set; }

            /// <summary>
            /// The student's full name
            /// </summary>
            public string FullName { get; set; }
        }

        /// <summary>
        /// A classroom has a program -- a list of courses and teachers teaching the course in the classroom.
        /// Using the ProgramDto would lead to circular references and too much details.
        /// </summary>
        public class ClassRoomProgramDto
        {
            /// <summary>
            /// Id of the Course that is part of the program
            /// </summary>
            public int CourseId { get; set; }

            /// <summary>
            /// Id of the Teacher teaching the course in the current classroom
            /// </summary>
            public int TeacherId { get; set; }

            /// <summary>
            /// The course name, basically
            /// </summary>
            public string Course { get; set; }

            /// <summary>
            /// The teachers name
            /// </summary>
            public string Teacher { get; set; }

            /// <summary>
            /// Number of teaching hours per week
            /// </summary>
            public int TeachingHours { get; set; }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace UniversityRegistrar.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public Student(string name, DateTime enrollmentDate, int id = 0)
        {
            Id = id;
            Name = name;
            EnrollmentDate = enrollmentDate;
        }

        public override bool Equals(System.Object otherStudent)
        {
            if (!(otherStudent is Student))
            {
                return false;
            }
            else
            {
                Student newStudent = (Student)otherStudent;
                bool idEquality = (Id == newStudent.Id);
                bool nameEquality = (Name == newStudent.Name);
                bool enrollmentDateEquality = (EnrollmentDate == newStudent.EnrollmentDate);
                return (idEquality && nameEquality && enrollmentDateEquality);
            }
        }


        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM students;";

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<Student> GetAll()
        {
            List<Student> allStudent = new List<Student> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM students;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);
                DateTime enrollmentDate = rdr.GetDateTime(2);
                Student newStudent = new Student(name, enrollmentDate, id);
                allStudent.Add(newStudent);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allStudent;
        }


        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO students (name, enrollment_date) VALUES (@StudentName, @StudentEnrollmentDate);";

            cmd.Parameters.AddWithValue("@StudentName", Name);
            cmd.Parameters.AddWithValue("@StudentEnrollmentDate", EnrollmentDate);

            cmd.ExecuteNonQuery();
            this.Id = (int)cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }


        public static Student Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM `students` WHERE id = @thisId;";

            cmd.Parameters.AddWithValue("@thisId", id);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            int studentId = 0;
            string name = string.Empty;
            DateTime enrollmentDate = DateTime.MinValue;

            while (rdr.Read())
            {
                studentId = rdr.GetInt32(0);
                name = rdr.GetString(1);
                enrollmentDate = rdr.GetDateTime(2);
            }

            Student foundItem = new Student(name, enrollmentDate, studentId);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return foundItem;
        }

        public static void DeleteSingleClient(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM students WHERE id = " + id + ";";

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void AddCourse(Course newCourse)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO courses_students (course_id, student_id) VALUES (@CourseId, @StudentId);";

            MySqlParameter course_id = new MySqlParameter();
            course_id.ParameterName = "@CourseId";
            course_id.Value = newCourse.Id;
            cmd.Parameters.Add(course_id);

            MySqlParameter student_id = new MySqlParameter();
            student_id.ParameterName = "@StudentId";
            student_id.Value = Id;
            cmd.Parameters.Add(student_id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Course> GetCourses()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT courses.* FROM students
            JOIN courses_students ON (students.id = courses_students.student_id)
            JOIN courses ON (courses_students.course_id = courses.id)
            WHERE students.id = @StudentId;";

            MySqlParameter studentIdParameter = new MySqlParameter();
            studentIdParameter.ParameterName = "@StudentId";
            studentIdParameter.Value = Id;
            cmd.Parameters.Add(studentIdParameter);
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Course> courses = new List<Course> { };

            while (rdr.Read())
            {
                int courseId = rdr.GetInt32(0);
                string courseName = rdr.GetString(1);
                string courseNumber = rdr.GetString(2);
                Course newCourse = new Course(courseName, courseNumber, courseId);
                courses.Add(newCourse);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return courses;
        }
    }
}

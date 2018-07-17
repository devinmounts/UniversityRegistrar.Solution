using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace UniversityRegistrar.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }

        public Course(string name, string number, int id = 0)
        {
            Id = id;
            Name = name;
            Number = number;
        }

        public override bool Equals(System.Object otherCourse)
        {
            if (!(otherCourse is Course))
            {
                return false;
            }
            else
            {
                Course newCourse = (Course)otherCourse;
                bool idEquality = (Id == newCourse.Id);
                bool nameEquality = (Name == newCourse.Name);
                bool numberEquality = (Number == newCourse.Number);
                return (idEquality && nameEquality && numberEquality);
            }
        }


        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM couses;";

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<Course> GetAll()
        {
            List<Course> allCourses = new List<Course> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM courses;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);
                string number = rdr.GetString(2);
                Course newCourse = new Course(name, number, id);
                allCourses.Add(newCourse);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allCourses;
        }


        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO courses (name, number) VALUES (@CourseName, @CourseNumber);";

            cmd.Parameters.AddWithValue("@CourseName", Name);
            cmd.Parameters.AddWithValue("@CourseNumber", Number);

            cmd.ExecuteNonQuery();
            this.Id = (int)cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }


        public static Course Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM `courses` WHERE id = @thisId;";

            cmd.Parameters.AddWithValue("@thisId", id);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            int courseId = 0;
            string name = string.Empty;
            string number = string.Empty;

            while (rdr.Read())
            {
                courseId = rdr.GetInt32(0);
                name = rdr.GetString(1);
                number = rdr.GetString(2);
            }

            Course foundItem = new Course(name, number, courseId);

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
            cmd.CommandText = @"DELETE FROM courses WHERE id = " + id + ";";

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void AddStudent(Student newStudent)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO courses_students (course_id, student_id) VALUES (@CourseId, @StudentId);";

            MySqlParameter course_id = new MySqlParameter();
            course_id.ParameterName = "@CourseId";
            course_id.Value = Id;
            cmd.Parameters.Add(course_id);

            MySqlParameter student_id = new MySqlParameter();
            student_id.ParameterName = "@StudentId";
            student_id.Value = newStudent.Id;
            cmd.Parameters.Add(student_id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Student> GetStudents()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT students.* FROM courses
            JOIN courses_students ON (courses.id = courses_students.course_id)
            JOIN students ON (courses_students.student_id = students.id)
            WHERE courses.id = @CourseId;";

            MySqlParameter courseIdParameter = new MySqlParameter();
            courseIdParameter.ParameterName = "@CourseId";
            courseIdParameter.Value = Id;
            cmd.Parameters.Add(courseIdParameter);
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Student> students = new List<Student> { };

            while (rdr.Read())
            {
                int studentId = rdr.GetInt32(0);
                string studentName = rdr.GetString(1);
                DateTime studentEnrollmentDate = rdr.GetDateTime(2);
                Student newStudent = new Student(studentName, studentEnrollmentDate, studentId);
                students.Add(newStudent);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return students;
        }
    }
}

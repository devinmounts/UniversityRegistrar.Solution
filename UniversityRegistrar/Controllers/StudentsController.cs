using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UniversityRegistrar.Models;


namespace UniversityRegistrar.Controllers
{
    public class StudentsController : Controller
    {
        [HttpGet("/students")]
        public ActionResult Index()
        {
            List<Student> allStudents = Student.GetAll();
            return View(allStudents);
        }

        [HttpPost("/students")]
        public ActionResult Index(string name, string enrollment)
        {
            DateTime newEnrollment = DateTime.Parse(enrollment);
            Student newStudent = new Student(name, newEnrollment);
            newStudent.Save();
            return RedirectToAction("Index");
        }

        [HttpGet("/students/add")]
        public ActionResult AddForm()
        {
            List<Course> allCourses = Course.GetAll();
            return View(allCourses);
        }

        [HttpPost("/students/add")]
        public ActionResult Add()
        {
            return View();
        }

        [HttpGet("students/{id}")]
        public ActionResult Details(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object> { };
            Student selectedStudent = Student.Find(id);
            List<Course> studentCourses = selectedStudent.GetCourses();
            List<Course> allCourses = Course.GetAll();
            model.Add("selectedStudent", selectedStudent);
            model.Add("studentCourses", studentCourses);
            model.Add("allCourses", allCourses);
            return View(model);
        }

        [HttpPost("students/{id}")]
        public ActionResult AddCourse(int id, string course)
        {

            Student newStudent = Student.Find(id);
            Course newCourse = Course.Find(Int32.Parse(course));
            newStudent.AddCourse(newCourse);

            return RedirectToAction("Details", new { id = id });
        }
    }
}

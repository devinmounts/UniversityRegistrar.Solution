using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UniversityRegistrar.Models;


namespace UniversityRegistrar.Controllers
{
    public class CoursesController : Controller
    {
        [HttpGet("/courses")]
        public ActionResult Index()
        {
            List<Course> allCourses = Course.GetAll();
            return View(allCourses);
        }

        [HttpPost("/courses")]
        public ActionResult Index(string name, string number)
        {
            Course newCourse = new Course(name, number);
            newCourse.Save();
            return RedirectToAction("Index");
        }

        [HttpGet("/courses/add")]
        public ActionResult AddForm()
        {
            return View();
        }

        [HttpGet("courses/{id}")]
        public ActionResult Details(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object> { };
            Course selectedCourse = Course.Find(id);
            List<Student> courseStudents = selectedCourse.GetStudents();
            List<Student> allStudents = Student.GetAll();
            model.Add("selectedCourse", selectedCourse);
            model.Add("courseStudents", courseStudents);
            model.Add("allStudents", allStudents);
            return View(model);
        }

        [HttpPost("courses/{id}")]
        public ActionResult AddStudent(int id, string student)
        {
            
            Course course = Course.Find(id);
            Student newStudent = Student.Find(Int32.Parse(student));
            course.AddStudent(newStudent);
            
            return RedirectToAction("Details", new { id = id });
        }
    }
}

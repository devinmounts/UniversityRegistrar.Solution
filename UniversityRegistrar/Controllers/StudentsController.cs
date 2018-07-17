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
        public ActionResult Index(string name, DateTime enrollment = new DateTime())
        {
            Student newStudent = new Student(name, enrollment);
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
    }
}

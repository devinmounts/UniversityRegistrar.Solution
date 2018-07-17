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

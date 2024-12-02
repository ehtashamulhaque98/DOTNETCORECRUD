using DotNetCoreCrud.Web.DataAccessLayer;
using DotNetCoreCrud.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;

namespace DotNetCoreCrud.Web.Controllers
{
    public class StudentController : Controller
    {
        private static string _connectionString;
        StudentData studentData;

        public StudentController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            studentData = new(_connectionString);
        }
        private List<SelectListItem> GetCourseTypeList()
        {
            var studentTypes = studentData.GetCourseTypes();

            List<SelectListItem> selectList = new List<SelectListItem>();
            foreach (var type in studentTypes)
            {
                selectList.Add(new SelectListItem()
                {
                    Text = type.TypeName,
                    Value = type.Id.ToString()
                });
            }
            return selectList;
        }


        public IActionResult Index()
        {
            var student = studentData.GetAllStudents();
            return View(student);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.CourseTypes = GetCourseTypeList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                studentData.AddStudent(student);
                return RedirectToAction("Index");
            }

            ViewBag.CourseTypes = GetCourseTypeList();
            return View(student);
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var student = studentData.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewBag.CourseTypes = GetCourseTypeList();
            return View(student);
        }

        [HttpPost]
        public IActionResult Update(int id, Student student)
        {
            if (ModelState.IsValid)
            {
                student.Id = id;
                studentData.UpdateStudent(student);
                return RedirectToAction("Index");
            }

            ViewBag.CourseTypes = GetCourseTypeList();
            return View(student);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var student = studentData.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            studentData.DeleteStudent(id);
            return RedirectToAction("Index");
        }
    }
}

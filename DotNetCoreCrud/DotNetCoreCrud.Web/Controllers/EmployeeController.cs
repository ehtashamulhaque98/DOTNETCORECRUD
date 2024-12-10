using DotNetCoreCrud.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;
using DotNetCoreCrud.Web.DataAccessLayer;
using Microsoft.Extensions.Configuration;

namespace DotNetCoreCrud.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private static string _connectionString;
        EmployeeData employeeData;

        public EmployeeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            employeeData = new(_connectionString);
        }
         
        private List<SelectListItem> GetEmployeeTypeList()
        {
            var employeeTypes = employeeData.GetEmployeeTypes();

            List<SelectListItem> selectList = new List<SelectListItem>();
            foreach (var type in employeeTypes)
            {
                selectList.Add(new SelectListItem
                {
                    Text = type.TypeName,
                    Value = type.Id.ToString()
                });
            }
            return selectList;
        }

        public IActionResult Index(string search)
        {
            var employees = employeeData.GetAllEmployees();

            if (!string.IsNullOrEmpty(search))
            {
                employees = employees.Where(e =>
                e.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                e.City.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                e.EmployeeType.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

                return View(employees);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.EmployeeTypes = GetEmployeeTypeList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                employeeData.AddEmployee(employee);
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeTypes = GetEmployeeTypeList();
            return View(employee);
        }

        [HttpGet]

        public IActionResult Update(int id)
        {
            var employee = employeeData.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }


            ViewBag.EmployeeTypes = GetEmployeeTypeList();

            return View(employee);
        }

        [HttpPost]
        public IActionResult Update(int id, Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.Id = id;
                employeeData.UpdateEmployee(employee);
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeTypes = GetEmployeeTypeList();
            return View(employee);
        }

        [HttpGet]

        public IActionResult Delete(int id)
        {
            var employee = employeeData.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            employeeData.DeleteEmployee(id);
            return RedirectToAction("Index");
        }
    }
}

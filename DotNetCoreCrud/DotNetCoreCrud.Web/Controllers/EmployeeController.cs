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

        public IActionResult Index()
        {
            var employees = employeeData.GetAllEmployees();

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
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("spAddEmployeess", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Name", employee.Name);
                    cmd.Parameters.AddWithValue("@Gender", employee.Gender);
                    cmd.Parameters.AddWithValue("@Age", employee.Age);
                    cmd.Parameters.AddWithValue("@Salary", employee.Salary);
                    cmd.Parameters.AddWithValue("@City", employee.City);
                    cmd.Parameters.AddWithValue("@Email", employee.Email);
                    cmd.Parameters.AddWithValue("@EmployeeTypeId", employee.EmployeeTypeId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeTypes = GetEmployeeTypeList();
            return View(employee);
        }
    }
}

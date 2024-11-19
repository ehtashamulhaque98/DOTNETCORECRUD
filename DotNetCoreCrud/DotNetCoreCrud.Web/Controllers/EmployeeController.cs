using DotNetCoreCrud.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;

namespace DotNetCoreCrud.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly string _connectionString;

        public EmployeeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private List<SelectListItem> GetEmployeeTypeList()
        {
            var employeeTypes = GetEmployeeTypes();

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

        private List<EmployeeType> GetEmployeeTypes()
        {
            var employeeTypes = new List<EmployeeType>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT Id, TypeName FROM EmployeeType";

                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    employeeTypes.Add(new EmployeeType()
                    {
                        Id = (int)reader["Id"],
                        TypeName = reader["TypeName"].ToString()
                    });

                }
            }
            return employeeTypes;
        }



        public IActionResult Index()
        {

            string connectionValue = _connectionString;
            var employees = GetAllEmployees();
            return View(employees);
        }

        private List<Employee> GetAllEmployees()
        {
            var employees = new List<Employee>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spGetAllEmployeess", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    employees.Add(new Employee()
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        Age = (int)reader["Age"],
                        Salary = Convert.ToDecimal(reader["Salary"]),
                        City = reader["City"].ToString(),
                        Email = reader["Email"].ToString(),
                        EmployeeType = reader["EmployeeType"].ToString(),
                        EmployeeTypeId = reader.IsDBNull(reader.GetOrdinal("EmployeeTypeId")) ? 0 : (int)reader["EmployeeTypeId"]
                    });
                }
            }
            return employees;
        }

    }
}

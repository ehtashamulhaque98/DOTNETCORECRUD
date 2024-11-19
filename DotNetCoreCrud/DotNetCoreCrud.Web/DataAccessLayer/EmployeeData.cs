using DotNetCoreCrud.Web.Controllers;
using DotNetCoreCrud.Web.Models;
using System.Data.SqlClient;

namespace DotNetCoreCrud.Web.DataAccessLayer
{
    public class EmployeeData
    {
        private string connectionString = "Data Source=DESKTOP-BRCMGPK\\SQLEXPRESS;Initial Catalog=TestDB;Integrated Security=True;Encrypt=False";

        private readonly string _connectionString;

        public EmployeeData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<EmployeeType> GetEmployeeTypes()
        {
            List<EmployeeType> employeeTypes = new List<EmployeeType>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT Id, TypeName FROM EmployeeTypes", con);
                con.Open();
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


        public List<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("spGetAllEmployeess", connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
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
                            EmployeeTypeId = (int)reader["EmployeeTypeId"]
                        });
                    }
                }
            }
            return employees;
        }

    }
}

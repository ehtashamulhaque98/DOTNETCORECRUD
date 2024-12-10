using DotNetCoreCrud.Web.Controllers;
using DotNetCoreCrud.Web.Models;
using System.Data;
using System.Data.SqlClient;

namespace DotNetCoreCrud.Web.DataAccessLayer
{
    public class EmployeeData
    {
        private readonly string _connectionString;

        public EmployeeData(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<EmployeeType> GetEmployeeTypes()
        {
            List<EmployeeType> employeeTypes = new List<EmployeeType>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT Id, TypeName FROM EmployeeType", con);
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

        public List<Employee> GetAllEmployees(string search = null)
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand command;

                if (!string.IsNullOrEmpty(search))
                {
                    command = new SqlCommand("spGetAllEmployeess", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Search", "%" + search + "%");
                }
                else
                {
                    command = new SqlCommand("spGetAllEmployeess", connection);
                    command.CommandType = CommandType.StoredProcedure;
                }
     
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
                            EmployeeType = reader["EmployeeType"].ToString()
                        });
                    }
                }
            }
            return employees;
        }
        
        public void AddEmployee(Employee employee)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spAddEmployeess", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
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
        }

        public void UpdateEmployee(Employee employee)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spUpdateEmployeess", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", employee.Id);
                cmd.Parameters.AddWithValue("@Name", employee.Name);
                cmd.Parameters.AddWithValue("@Gender", employee.Gender);
                cmd.Parameters.AddWithValue("@Age", employee.Age);
                cmd.Parameters.AddWithValue("@Salary", employee.Salary);
                cmd.Parameters.AddWithValue("@City", employee.City);
                cmd.Parameters.AddWithValue("@Email", employee.Email);
                cmd.Parameters.AddWithValue("@EmployeeTypeId", employee.EmployeeTypeId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteEmployee(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spDeleteEmployeess", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public Employee GetEmployeeById(int id)
        {
            Employee employee = null;


            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Employeess WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@Id", id);

               con.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())

                    {
                        employee = new Employee
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
                        };
                        
                    }
                }
            }
            return employee;
        }
    }
}

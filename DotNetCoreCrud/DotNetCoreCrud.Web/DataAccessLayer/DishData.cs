using DotNetCoreCrud.Web.Models;
using System.Data;
using System.Data.SqlClient;

namespace DotNetCoreCrud.Web.DataAccessLayer
{
    public class DishData
    {
        private readonly string _connectionString;

        public DishData(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<DishCategory> GetDishTypes()
        {
            List<DishCategory> dishCategories = new List<DishCategory>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT Id, TypeName FROM DishCategory", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    dishCategories.Add(new DishCategory()
                    {
                        Id = (int)reader["Id"],
                        TypeName = reader["TypeName"].ToString()
                    });
                }
            }

            return dishCategories;
        }
        public List<Dish> GetAllDishes(int pageNumber, int pageSize, string search = null)
        {
            List<Dish> dishes = new List<Dish>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("spGetPagedDishes", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@PageNumber", pageNumber);
                command.Parameters.AddWithValue("@PageSize", pageSize);
                if (!string.IsNullOrEmpty(search))
                {
                    command.Parameters.AddWithValue("@Search", "%" + search + "%");
                }
                else
                {
                    command.Parameters.AddWithValue("@Search", DBNull.Value);
                }

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dishes.Add(new Dish()
                        {
                            Id = (int)reader["Id"],
                            DishName = reader["DishName"].ToString(),
                            Price = (decimal)reader["Price"],
                            Quantity = (int)reader["Quantity"],
                            DishCategory = reader["DishCategory"].ToString(),
                        });
                    }
                }
            }
            return dishes;
        }
        public int GetTotalDishCount(string search = null)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("spGetTotalDishCount", connection);
                command.CommandType = CommandType.StoredProcedure;

                if (!string.IsNullOrEmpty(search))
                {
                    command.Parameters.AddWithValue("@Search", "%" + search + "%");
                }
                else
                {
                    command.Parameters.AddWithValue("@Search", DBNull.Value);
                }
                connection.Open();
                return (int)command.ExecuteScalar();
            }
        }

        public void AddDish(Dish dish)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spAddDish", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                
                cmd.Parameters.AddWithValue("@DishName", dish.DishName);
                cmd.Parameters.AddWithValue("@Price", dish.Price);
                cmd.Parameters.AddWithValue("@Quantity", dish.Quantity);
                cmd.Parameters.AddWithValue("@DishCategoryId", dish.DishCategory);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateDish(Dish dish)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spUpdateDish", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", dish.Id);
                cmd.Parameters.AddWithValue("@DishName", dish.DishName);
                cmd.Parameters.AddWithValue("@Price", dish.Price);
                cmd.Parameters.AddWithValue("@Quantity", dish.Quantity);
                cmd.Parameters.AddWithValue("@DishCategoryId", dish.DishCategory);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteDish(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spDeleteDish", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public Dish GetDishById(int id)
        {
            Dish dish = null;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Dish WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@Id", id);
                

                con.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        dish = new Dish()
                        {
                            Id = (int)reader["Id"],
                            DishName = reader["DishName"].ToString(),
                            Price = (decimal)reader["Price"],
                            Quantity = (int)reader["Quantity"],
                            DishCategoryId = (int)reader["DishCategoryId"]

                        };
                    }
                }    
            }
            return dish;
        }
    }
}

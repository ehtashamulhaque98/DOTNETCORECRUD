using DotNetCoreCrud.Web.Models;
using System.Data.SqlClient;
using System.Data;

namespace DotNetCoreCrud.Web.DataAccessLayer
{
    public class FruitData
    {
        private readonly string _connectionString;

        public FruitData(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<FruitCategory> GetFruitCategories()
        {
            List<FruitCategory> categories = new List<FruitCategory>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT Id, TypeName FROM FruitCategory", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    categories.Add(new FruitCategory()
                    {
                        Id = (int)reader["Id"],
                        TypeName = reader["TypeName"].ToString()
                    });
                }
            }

            return categories;
        }

        public List<Fruit> GetAllFruits(int pageNumber, int pageSize, string search = null)
        {
            List<Fruit> fruits = new List<Fruit>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("spGetPagedFruits", connection);
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
                        fruits.Add(new Fruit()
                        {
                            FruitId = (int)reader["FruitId"],
                            FruitName = reader["FruitName"].ToString(),
                            Price = (decimal)reader["Price"],
                            Quantity = (int)reader["Quantity"],
                            CategoryType = reader["TypeName"].ToString(),
                        });
                    }
                }
            }
            return fruits;
        }

        public int GetTotalFruitCount(string search = null)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("spGetTotalFruitCount", connection);
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

        public void AddFruit(Fruit fruit)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spAddFruit", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FruitName", fruit.FruitName);
                cmd.Parameters.AddWithValue("@Price", fruit.Price);
                cmd.Parameters.AddWithValue("@Quantity", fruit.Quantity);
                cmd.Parameters.AddWithValue("@CategoryId", fruit.CategoryId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateFruit(Fruit fruit)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spUpdateFruit", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@FruitId", fruit.FruitId);
                cmd.Parameters.AddWithValue("@FruitName", fruit.FruitName);
                cmd.Parameters.AddWithValue("@Price", fruit.Price);
                cmd.Parameters.AddWithValue("@Quantity", fruit.Quantity);
                cmd.Parameters.AddWithValue("@CategoryId", fruit.CategoryId);
                

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteFruit(int fruitId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spDeleteFruit", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@FruitId", fruitId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public Fruit GetFruitById(int fruitId)
        {
            Fruit fruit = null;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Fruit WHERE FruitId = @FruitId";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@FruitId", fruitId);

                con.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        fruit = new Fruit()
                        {
                            FruitId = (int)reader["FruitId"],
                            FruitName = reader["FruitName"].ToString(),
                            Price = (decimal)reader["Price"],
                            Quantity = (int)reader["Quantity"],
                            CategoryId = (int)reader["CategoryId"]
                        };
                    }
                }
            }
            return fruit;
        }
    }

}

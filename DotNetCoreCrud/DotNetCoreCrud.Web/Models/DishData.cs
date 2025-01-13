using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace DotNetCoreCrud.Web.Models
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
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                var dishCategories = con.Query<DishCategory>("SELECT Id, TypeName FROM DishCategory").ToList();
                return dishCategories;
            }
        }

        public List<Dish> GetAllDishes(int pageNumber, int pageSize, string search = null)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var dishes = connection.Query<Dish>("spGetPagedDishes",
                    new { PageNumber = pageNumber, PageSize = pageSize, Search = search ?? (object)DBNull.Value },
                    commandType: CommandType.StoredProcedure).ToList();
                return dishes;
            }
        }

        public int GetTotalDishCount(string search = null)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.ExecuteScalar<int>("spGetTotalDishCount",
                    new { Search = search ?? (object)DBNull.Value }, commandType: CommandType.StoredProcedure);
            }
        }

        public void AddDish(Dish dish)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                conn.Execute("spAddDish", new
                {
                    DishName = dish.DishName,
                    Price = dish.Price,
                    Quantity = dish.Quantity,
                    DishCategoryId = dish.DishCategory
                }, commandType: CommandType.StoredProcedure);
            }
        }

        public void UpdateDish(Dish dish)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                con.Execute("spUpdateDish", new
                {
                    Id = dish.Id,
                    DishName = dish.DishName,
                    Price = dish.Price,
                    Quantity = dish.Quantity,
                    DishCategoryId = dish.DishCategory
                }, commandType: CommandType.StoredProcedure);
            }
        }

        public void DeleteDish(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                con.Execute("spDeleteDish", new { Id = id }, commandType: CommandType.StoredProcedure);
            }
        }

        public Dish GetDishById(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                var dish = con.Query<Dish>("SELECT * FROM Dish WHERE Id = @Id", new { Id = id }).FirstOrDefault();
                return dish;
            }
        }
    }
}

using System.Data.SqlClient;
using System.Data;
using Dapper;

//namespace DotNetCoreCrud.Web.Models
//{
//    public class ProductData
//    {
//        private readonly string _connectionString;

//        public ProductData(string connectionString)
//        {
//            _connectionString = connectionString;
//        }

//        public List<ProductCategory> GetProductTypes()
//        {
//            using (SqlConnection con = new SqlConnection(_connectionString))
//            {
//                con.Open();
//                var productCategories = con.Query<ProductCategory>("SELECT Id, TypeName FROM ProductCategory").ToList();
//                return productCategories;
//            }
//        }

//        public List<Product> GetAllProducts(int pageNumber, int pageSize, string search = null)
//        {
//            using (SqlConnection connection = new SqlConnection(_connectionString))
//            {
//                connection.Open();
//                var products = connection.Query<Product>("spGetPagedProducts",
//                    new { PageNumber = pageNumber, PageSize = pageSize, Search = search ?? (object)DBNull.Value },
//                    commandType: CommandType.StoredProcedure).ToList();
//                return products;
//            }
//        }

//        public int GetTotalProductCount(string search = null)
//        {
//            using (SqlConnection connection = new SqlConnection(_connectionString))
//            {
//                connection.Open();
//                return connection.ExecuteScalar<int>("spGetTotalProductCount",
//                    new { Search = search ?? (object)DBNull.Value }, commandType: CommandType.StoredProcedure);
//            }
//        }

//        public void AddProduct(Product product)
//        {
//            using (SqlConnection conn = new SqlConnection(_connectionString))
//            {
//                conn.Open();
//                conn.Execute("spAddProduct", new
//                {
//                ProductName = product.ProductName,
//                Price = product.Price,
//                Quantity = product.Quantity,
//                ProductCategory= product.ProductCategory
//            }, commandType: CommandType.StoredProcedure);
//            }
//        }

//        public void UpdateProduct(Product product)
//        {
//            using (SqlConnection con = new SqlConnection(_connectionString))
//            {
//                con.Open();
//                con.Execute("spUpdateProduct", new
//                {
//                    Id = product.Id,
//                    ProductName = product.ProductName,
//                    Price = product.Price,
//                    Quantity = product.Quantity,
//                    ProductCategory = product.ProductCategory
//                }, commandType: CommandType.StoredProcedure);
//            }
//        }

//        public void DeleteProduct(int id)
//        {
//            using (SqlConnection con = new SqlConnection(_connectionString))
//            {
//                con.Open();
//                con.Execute("spDeleteProduct", new { Id = id }, commandType: CommandType.StoredProcedure);
//            }
//        }

//        public Dish GetProductById(int id)
//        {
//            using (SqlConnection con = new SqlConnection(_connectionString))
//            {
//                con.Open();
//                var product = con.Query<Product>("SELECT * FROM Product WHERE Id = @Id", new { Id = id }).FirstOrDefault();
//                return product;
//            }
//        }
//    }
//}


namespace DotNetCoreCrud.Web.Models
{
    public class ProductData
    {
        private readonly string _connectionString;

        public ProductData(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ProductCategory> GetProductTypes()
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                var productCategories = con.Query<ProductCategory>("SELECT Id, TypeName FROM ProductCategory").ToList();
                return productCategories;
            }
        }

        public List<Product> GetAllProducts(int pageNumber, int pageSize, string search = null)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var products = connection.Query<Product>("spGetPagedProducts",
                    new { PageNumber = pageNumber, PageSize = pageSize, Search = search ?? (object)DBNull.Value },
                    commandType: CommandType.StoredProcedure).ToList();
                return products;
            }
        }

        public int GetTotalProductCount(string search = null)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.ExecuteScalar<int>("spGetTotalProductCount",
                    new { Search = search ?? (object)DBNull.Value }, commandType: CommandType.StoredProcedure);
            }
        }

        public void AddProduct(Product product)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                conn.Execute("spAddProduct", new
                {
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    ProductCategory = product.ProductCategory
                }, commandType: CommandType.StoredProcedure);
            }
        }

        public void UpdateProduct(Product product)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                con.Execute("spUpdateProduct", new
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    ProductCategory = product.ProductCategory
                }, commandType: CommandType.StoredProcedure);
            }
        }

        public void DeleteProduct(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                con.Execute("spDeleteProduct", new { Id = id }, commandType: CommandType.StoredProcedure);
            }
        }

        public Product GetProductById(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                var product = con.Query<Product>("SELECT * FROM Product WHERE Id = @Id", new { Id = id }).FirstOrDefault();
                return product;
            }
        }
    }
}

using DotNetCoreCrud.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;

namespace DotNetCoreCrud.Web.DataAccessLayer
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
            List<ProductCategory> productCategories = new List<ProductCategory>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT Id, TypeName FROM ProductCategory", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    productCategories.Add(new ProductCategory()
                    {
                        Id = (int)reader["Id"],
                        TypeName = reader["TypeName"].ToString()
                    });
                }
                return productCategories;
            }

        }

        public List<Product> GetAllProducts(int pageNumber, int pageSize, string search = null)
        {
            List<Product> products = new List<Product>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("spGetPagedProducts", connection);
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
                        products.Add(new Product()
                        {
                            Id = (int)reader["Id"],
                            ProductName = reader["ProductName"].ToString(),
                            Price = (decimal)reader["Price"],
                            Quantity = (int)reader["Quantity"],
                            ProductCategory = reader["ProductCategory"].ToString(),
                        });
                    }
                }
            }
            return products;
        }

        public int GetTotalProductCount(string search = null)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("spGetTotalProductCount", connection);
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

        //public List<Product> GetAllProducts(string search = null)
        //{
        //    List<Product> products = new List<Product>();

        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Open();

        //        SqlCommand command;
        //        if (!string.IsNullOrEmpty(search))
        //        {
        //            command = new SqlCommand("spGetAllProducts", connection);
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.Parameters.AddWithValue("@Search", "%" + search + "%");
        //        }
        //        else
        //        {
        //            command = new SqlCommand("spGetAllProducts", connection);
        //            command.CommandType = CommandType.StoredProcedure;
        //        }

        //        using (SqlDataReader reader = command.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                products.Add(new Product()
        //                {
        //                    Id = (int)reader["Id"],
        //                    ProductName = reader["ProductName"].ToString(),
        //                    Price = (decimal)reader["Price"],
        //                    Quantity = (int)reader["Quantity"],
        //                    ProductCategory = reader["ProductCategory"].ToString(),
        //                });
        //            }
        //        }
        //    }

        //    return products;
        //}

        public void AddProduct(Product product)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spAddProduct", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                cmd.Parameters.AddWithValue("@ProductCategory", product.ProductCategory);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateProduct(Product product)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spUpdateProduct", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Id", product.Id);
                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                cmd.Parameters.AddWithValue("@ProductCategory", product.ProductCategory);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteProduct(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spDeleteProduct", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public Product GetProductById(int id)
        {
            Product product = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                
                string query = "SELECT * FROM Product WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@Id", id);

                con.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                      
                        product = new Product()
                        {
                            Id = (int)reader["Id"],
                            ProductName = reader["ProductName"].ToString(),
                            Price = (decimal)reader["Price"],
                            Quantity = (int)reader["Quantity"],
                            ProductCategory = reader["ProductCategory"].ToString(),

                        };
                    }
                }
            }
            return product;
        }

    }
}

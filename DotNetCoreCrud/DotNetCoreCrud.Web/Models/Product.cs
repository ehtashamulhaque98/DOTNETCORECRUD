using System.ComponentModel.DataAnnotations;

namespace DotNetCoreCrud.Web.Models
{
    public class Product
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public Decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        public string? ProductCategory { get; set; }

        [Required]
        public int ProductCategoryId { get; set; }

    }
}

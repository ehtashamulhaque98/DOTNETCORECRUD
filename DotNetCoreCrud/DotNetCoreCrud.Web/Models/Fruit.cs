using System.ComponentModel.DataAnnotations;

namespace DotNetCoreCrud.Web.Models
{
    public class Fruit
    {
        [Key]
        public int FruitId { get; set; }

        [Required]
        public string FruitName { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        
        public int CategoryId { get; set; }

        public string? CategoryType { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace DotNetCoreCrud.Web.Models
{
    public class Dish
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string DishName { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        public string? DishCategory { get; set; }

        [Required]
        public int DishCategoryId { get; set; }
    }
}

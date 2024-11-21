using System.ComponentModel.DataAnnotations;

namespace VienoBackend.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public double Price { get; set; }

        public bool IsActive { get; set; } = true; // статус активности продукта
    }
}

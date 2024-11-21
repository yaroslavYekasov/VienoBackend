using System.ComponentModel.DataAnnotations;

namespace VienoBackend.Models
{
    public class User
    {
        [Key] // Это указывает на то, что поле 'Id' является первичным ключом
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public string Role { get; set; } = "User"; // может быть User или Admin
    }
}

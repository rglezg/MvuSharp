using System.ComponentModel.DataAnnotations;

namespace Crud.Blazor.Models
{
    public class User
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public int Age { get; set; }
    }
}
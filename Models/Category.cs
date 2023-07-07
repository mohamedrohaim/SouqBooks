using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is Reqired!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Display Order is Reqired!"), Range(1, 100)]

        public int DisplayOrder { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}

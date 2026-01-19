using System.ComponentModel.DataAnnotations;

namespace book_shop_back_end.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Title { get; set; } = null!;
        [Required]
        public string Author { get; set; } = null!;
        public string Description { get; set; } = null!;
        [Required]
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string Category { get; set; } = null!;
    }

}

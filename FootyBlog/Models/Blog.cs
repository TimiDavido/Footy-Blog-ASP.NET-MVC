using System.ComponentModel.DataAnnotations;

namespace FootyBlog.Models
{
    public class Blog
    {
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Content { get; set; }
        public DateTime PostDate { get; set; }

        public string? ImagePath { get; set; }
    }
}

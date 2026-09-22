using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FootyBlog.Application.DTOs
{
    public class BlogDto
    {
        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Content { get; set; }

        [Required]
        public IFormFile? Image { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace FootyBlog.Application.DTOs
{
    public  class RegisterDto
    {
        [Required]
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
    }
}

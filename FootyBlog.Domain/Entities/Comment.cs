using System;
using System.Collections.Generic;
using System.Text;

namespace FootyBlog.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int blogId { get; set; }
        public string userId { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public string? UserName { get; set; }
    }
}

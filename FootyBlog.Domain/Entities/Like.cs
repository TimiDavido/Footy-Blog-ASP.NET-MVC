
namespace FootyBlog.Domain.Entities;
public class Like
{
    public int Id { get; set; }

    public string UserId { get; set; } = "";

    public int BlogId { get; set; }

    public DateTime CreatedAt { get; set; }
}
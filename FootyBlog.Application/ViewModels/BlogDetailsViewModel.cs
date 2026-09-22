using FootyBlog.Domain.Entities;

namespace FootyBlog.Application.ViewModels;

public class BlogDetailsViewModel
{
    public Blog? Blog { get; set; }
    public List<Blog>? OtherPosts { get; set; }
    public int LikeCount { get; set; }
    public bool HasLiked { get; set; }
    public List<Comment>? Comments { get; set; }

}

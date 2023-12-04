using Blog.Data.Models;

namespace Blog.BLL.ViewModels;

public class DiscussionPostViewModel
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public int CommentId { get; set; }
    public DateTime PublicationTime { get; set; }
    public string CommentText { get; set; }
    public PostViewModel PostVM { get; set; } = new();
    public ICollection<Comment> CommentVM { get; set; } = new List<Comment>();
}

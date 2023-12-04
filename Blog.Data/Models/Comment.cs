using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Data.Models;

public class Comment
{
    public int Id { get; set; }
    public string? CommentText { get; set; }
    public DateTime CommentPublicationTime { get; set; }
    public int PostId { get; set; }
    public string UserId { get; set; }
}

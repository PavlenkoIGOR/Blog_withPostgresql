namespace Blog.Data.Models;

public class Post
{
    public int Id { get; set; }
    public string postTitle { get; set; }
    public string postText { get; set; }
    public DateTime PublicationDate { get; set; }
    public int UserId { get; set; }

}

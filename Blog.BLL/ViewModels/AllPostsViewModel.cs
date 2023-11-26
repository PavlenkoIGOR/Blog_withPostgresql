namespace Blog.BLL.ViewModels;
public class AllPostsViewModel
{
    public int Id { get; set; }
    public string Author { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public DateTime PublicationTime { get; set; }
    public HashSet<string> TegsList { get; set; }
}
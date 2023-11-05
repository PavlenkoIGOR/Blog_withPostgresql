using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.BLL.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string postTitle { get; set; }
        public string postText { get; set; }
        public int user_id { get; set; }
        public List<Teg> Tegs { get; set; } = new();
    }
}

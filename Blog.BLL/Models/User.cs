﻿using Blog.BLL.Models;
using System.ComponentModel.DataAnnotations;

namespace Blog.BLL.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
        public string Role { get; set; }

        public List<Post>? posts_id { get; set; } = new();
    }
}
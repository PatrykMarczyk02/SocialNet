﻿using System;

namespace SocialNet.Models
{
    public class Post
    {
        public int Id { get; set; } 
        public string Content { get; set; } 
        public string Author { get; set; } 
        public DateTime CreatedAt { get; set; } 
    }
}

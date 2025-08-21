using System.ComponentModel.DataAnnotations;

namespace Blog_API.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Post> Posts { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
    }
}

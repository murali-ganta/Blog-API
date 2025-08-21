namespace Blog_API.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public required string Content { get; set; }
        public int PostId { get; set; }
        public required Post Post { get; set; }
        public int AuthorId { get; set; }
        public required User Author { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

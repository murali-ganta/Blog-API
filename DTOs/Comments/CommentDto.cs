namespace Blog_API.DTOs.Comments
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public string PostTitle { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}

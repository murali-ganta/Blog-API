using Blog_API.Data;
using Blog_API.DTOs.Comments;
using Blog_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly BlogContext _context;

        public CommentsController(BlogContext context)
        {
            _context = context;
        }

        // GET: api/Comments
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments()
        {
            var comments = await _context.Comments
                .Include(c => c.Author)
                .Include(c => c.Post)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    AuthorName = c.Author.Username,
                    PostTitle = c.Post.Title,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync();

            return Ok(comments);
        }

        // GET: api/Comments/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<CommentDto>> GetComment(int id)
        {
            var comment = await _context.Comments
                .Include(c => c.Author)
                .Include(c => c.Post)
                .Where(c => c.Id == id)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    AuthorName = c.Author.Username,
                    PostTitle = c.Post.Title,
                    CreatedAt = c.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (comment == null) return NotFound();

            return Ok(comment);
        }

        // POST: api/Comments
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CommentDto>> CreateComment(CreateCommentDto dto)
        {
            var post = await _context.Posts.FindAsync(dto.PostId);
            if (post == null) return BadRequest("Invalid PostId");

            var author = await _context.Users.FindAsync(dto.AuthorId);
            if (author == null) return BadRequest("Invalid AuthorId");

            var comment = new Comment
            {
                Content = dto.Content,
                PostId = dto.PostId,
                AuthorId = dto.AuthorId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            var commentDto = new CommentDto
            {
                Id = comment.Id,
                Content = comment.Content,
                AuthorName = author.Username,
                PostTitle = post.Title,
                CreatedAt = comment.CreatedAt
            };

            return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, commentDto);
        }

        // PUT: api/Comments/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateComment(int id, CreateCommentDto dto)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return NotFound();

            var post = await _context.Posts.FindAsync(dto.PostId);
            if (post == null) return BadRequest("Invalid PostId");

            var author = await _context.Users.FindAsync(dto.AuthorId);
            if (author == null) return BadRequest("Invalid AuthorId");

            comment.Content = dto.Content;
            comment.PostId = dto.PostId;
            comment.AuthorId = dto.AuthorId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return NotFound();

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

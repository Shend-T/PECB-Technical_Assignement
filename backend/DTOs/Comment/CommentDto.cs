namespace backend.DTOs.Comment;

public class CommentDto
{
    public int id { get; set; }
    public int ticketId { get; set; }
    public string authorName { get; set; } = string.Empty;
    public string body { get; set; } = string.Empty;
    public DateTime createdDate { get; set; }
}
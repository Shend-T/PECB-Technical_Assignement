using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Comment;

public class CreateCommentDto
{
    [Required]
    [StringLength(100)]
    public string authorName { get; set; } = string.Empty;

    [Required]
    public string body { get; set; } = string.Empty;
}
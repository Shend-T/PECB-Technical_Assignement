using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Comment
{
    public int id { get; set; }

    public int ticketId { get; set; }
    public Ticket ticket { get; set; } = null!;

    public string authorName { get; set; } = string.Empty;

    public string body { get; set; } = string.Empty;

    public DateTime createdDate { get; set; }
}
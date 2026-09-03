using backend.Models;
using backend.DTOs.Agent;
using backend.DTOs.Comment;

namespace backend.DTOs.Ticket;

public class TicketDto
{
    public int id { get; set; }

    public string referenceId { get; set; } = string.Empty;

    public string title { get; set; } = string.Empty;

    public string description { get; set; } = string.Empty;

    public string customerName { get; set; } = string.Empty;

    public string customerEmail { get; set; } = string.Empty;

    public Priority priority { get; set; }

    public Status status { get; set; }

    public int? assignedAgentId { get; set; }

    public AgentDto? assignedAgent { get; set; }

    public DateTime createdDate { get; set; }

    public DateTime lastModifiedDate { get; set; }

    public DateTime? resolvedDate { get; set; }

    public DateTime? closedDate { get; set; }

    public DateTime dueDate { get; set; }

    public bool isOverdue { get; set; }

    public List<CommentDto> comments { get; set; } = new();
}
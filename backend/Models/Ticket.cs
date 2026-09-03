namespace backend.Models;

public class Ticket
{
    public int id { get; set; }
    public string referenceId { get; set; }   = string.Empty;
    public string title { get; set; }         = string.Empty;
    public string description { get; set; }   = string.Empty;
    public string customerName { get; set; }  = string.Empty;
    public string customerEmail { get; set; } = string.Empty;

    public Priority priority{ get; set; }     = Priority.Low;
    public Status status{ get; set; }         = Status.New;
    
    public int? assignedAgentId { get; set; }
    public Agent? assignedAgent { get; set; } // null = Un Assigned
    
    public DateTime createdDate { get; set; }       = DateTime.UtcNow;
    public DateTime lastModifiedDate { get; set; }  = DateTime.UtcNow;
    public DateTime? resolvedDate { get; set; }     = null;
    public DateTime? closedDate { get; set; }       = null;
    public DateTime dueDate { get; set; }
}
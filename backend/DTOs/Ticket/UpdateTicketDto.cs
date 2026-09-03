using System.ComponentModel.DataAnnotations;
using backend.Models;

namespace backend.DTOs.Ticket;

public class UpdateTicketDto
{
    [Required]
    [StringLength(200)]
    public string title { get; set; } = string.Empty;

    [Required]
    public string description { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string customerName { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    [EmailAddress]
    public string customerEmail { get; set; } = string.Empty;

    public Priority priority { get; set; }

    public Status status { get; set; }

    public int? assignedAgentId { get; set; }
}
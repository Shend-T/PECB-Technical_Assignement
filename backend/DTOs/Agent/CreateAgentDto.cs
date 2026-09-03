using System.ComponentModel.DataAnnotations;
using backend.Models;

namespace backend.DTOs.Agent;

public class CreateAgentDto
{
    [Required]
    [StringLength(100)]
    public string fullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string email { get; set; } = string.Empty;

    [Required]
    public Department department { get; set; }

    public bool active { get; set; } = true;
}
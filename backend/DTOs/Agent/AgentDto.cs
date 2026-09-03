using System.ComponentModel.DataAnnotations;
using backend.Models;

namespace backend.DTOs.Agent;

public class AgentDto
{
    public int id { get; set; }
    public string fullName { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public Department department { get; set; }
    public bool active { get; set; }
}
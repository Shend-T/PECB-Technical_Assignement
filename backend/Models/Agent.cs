namespace backend.Models;

public class Agent
{
    public int id { get; set; }

    public string fullName {get; set;} = string.Empty;

    public string email { get; set; } = string.Empty;

    public Department department { get; set; } = Department.General;

    public bool active { get; set; } = false;
}
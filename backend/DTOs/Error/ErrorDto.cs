namespace backend.DTOs.Error;

public class ErrorDto
{
    public ErrorCode error { get; set; }
    public string message { get; set; } = string.Empty;
}
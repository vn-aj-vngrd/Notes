namespace NotesClient.Models;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public T? Value { get; set; }
    public string? Error { get; set; }
}

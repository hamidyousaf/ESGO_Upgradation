namespace Domain.DTOs.Requests;

public class AddBookRequest
{
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Author { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
public class UpdateBookRequest
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Author { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}


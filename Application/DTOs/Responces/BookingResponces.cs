namespace Application.DTOs.Responces;

public class GetBookingsResponse
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Details { get; set; }
    public byte Status { get; set; }
    public string DocumentUrl { get; set; }
}

namespace Domain.Entities;

public class StarterFormQuestion
{
    public int Id { get; set; }
    public string Question { get; set; }
    public byte Order { get; set; }
    public bool IsDeleted { get; set; }
}

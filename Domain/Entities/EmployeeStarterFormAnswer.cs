namespace Domain.Entities;

public class EmployeeStarterFormAnswer : BaseEntity<int>
{
    public int QuestionId { get; set; } // from StarterFormQuestions Table.
    public bool YesOrNo { get; set; }
    public int EmployeeId { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }
    // navigation property.
    public StarterFormQuestion Question { get; set; }
    public Employee Employee { get; set; }
}

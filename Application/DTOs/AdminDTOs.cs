namespace Application.DTOs;

public class EmployeeTypeDto
{
    public byte Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal MinRate { get; set; }
}

public class JobTypeDto
{
    public byte Id { get; set; }
    public string Name { get; set; }
}

public class EmployeeCategoryDto
{
    public byte Id { get; set; }
    public string Name { get; set; }
}

public class ShiftDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}


public class GetEmployerByIdContactDetailDto
{
    public int Id { get; set; }
    public string ContactName { get; set; }
    public string Email { get; set; }
    public string CountryCode { get; set; }
    public string PhoneNumber { get; set; }
    public string JobTitle { get; set; }
}

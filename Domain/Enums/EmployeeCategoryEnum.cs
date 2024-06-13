namespace Domain.Enums;

public enum EmployeeCategoryEnum : byte
{
    [Description("ESGO Employee")]
    ESGOEmployee = 1, // Employee with AccountStatus Active.
    [Description("Out of Portal Employee")]
    OutOfPortalEmployee = 2 // Employee with AccountStatus OutOfPortal.
}

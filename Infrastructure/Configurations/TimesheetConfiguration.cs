namespace Infrastructure.Configurations;

public class TimesheetConfiguration : IEntityTypeConfiguration<Timesheet>
{
    public void Configure(EntityTypeBuilder<Timesheet> builder)
    {
        #region Entity configuration
        builder
        .HasKey(x => x.Id);
        builder
            .HasOne(x => x.AssignedJob)
            .WithOne()
            .HasForeignKey<Timesheet>(b => b.AssignedJobId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        builder
            .HasOne(x => x.Job)
            .WithMany()
            .HasForeignKey(b => b.JobId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
        builder
            .HasOne(x => x.Employer)
            .WithMany()
            .HasForeignKey(b => b.EmployerId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
        builder
            .HasOne(x => x.Employee)
            .WithMany()
            .HasForeignKey(b => b.EmployeeId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
        builder
            .Property(x => x.Date)
            .HasColumnType("date")
            .IsRequired();
        builder
            .Property(x => x.StartTime)
            .HasConversion<TimeOnlyConverter>()
            .IsRequired();
        builder
            .Property(x => x.EndTime)
            .HasConversion<TimeOnlyConverter>()
            .IsRequired();
        builder
            .Property(x => x.BreakTime)
            .IsRequired();
        builder
            .Property(x => x.BillableHours)
            .HasConversion<TimeOnlyConverter>()
            .IsRequired();
        builder
            .Property(x => x.BillableHourInDecimal)
            .IsRequired();
        builder
            .Property(x => x.TotalHours)
            .HasConversion<TimeOnlyConverter>()
            .IsRequired();
        builder
            .Property(x => x.HourlyRate)
            .IsRequired();
        builder
            .Property(x => x.Rating)
            .HasDefaultValue(0)
            .IsRequired();
        builder
            .Property(x => x.TotalAmount)
            .IsRequired();
        builder
            .Property(x => x.TotalAmount)
            .IsRequired();
        builder
            .Property(x => x.Status)
            .HasDefaultValue(TimeSheetStatusEnum.NotSubmitted)
            .IsRequired();
        builder
            .Property(x => x.OriginalHourlyRate)
            .IsRequired();
        builder
            .Property(x => x.OrginalTotalAmount)
            .IsRequired();
        builder
            .Property(x => x.TotalHolidayAmount)
            .IsRequired();
        builder
            .Property(x => x.Notes)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.ReviewedBy)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.Reason)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.PlateformFee)
            .IsRequired(false);
        builder
            .Property(x => x.FinalAmount)
            .IsRequired(false);
        builder
            .Property(x => x.ApprovalDate)
            .IsRequired(false);
        builder
            .Property(x => x.RejectionDate)
            .IsRequired(false);
        builder
            .Property(x => x.IsActive)
            .HasDefaultValue(true)
            .IsRequired();
        builder
            .Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();
        builder
             .Property(b => b.CreatedDate)
             .HasDefaultValueSql("GETUTCDATE()")
             .IsRequired();
        builder
            .Property(x => x.UpdatedDate)
            .IsRequired(false);
        #endregion
        #region Global query filter
        builder.HasQueryFilter(x => !x.IsDeleted && x.IsActive);
        #endregion
    }
}

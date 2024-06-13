namespace Infrastructure.Configurations;

public class JobConfiguration : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        #region Entity configuration
        builder
            .HasKey(x => x.Id);
        builder
            .HasOne(x => x.Employer)
            .WithMany()
            .HasForeignKey(b => b.EmployerId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
        builder
            .HasOne(x => x.EmployeeType)
            .WithMany()
            .HasForeignKey(b => b.EmployeeTypeId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
        builder
            .HasOne(x => x.Booking)
            .WithMany()
            .HasForeignKey(b => b.BookingId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(false);
        builder
            .Property(x => x.Date)
            .HasColumnType("date")
            .IsRequired();
        builder
            .Property(x => x.ShiftId)
            .HasDefaultValue(0)
            .IsRequired();
        builder
            .Property(x => x.ShiftStartTime)
            .HasConversion<TimeOnlyConverter>()
            .IsRequired();
        builder
            .Property(x => x.ShiftEndTime)
            .HasConversion<TimeOnlyConverter>()
            .IsRequired();
        builder
            .Property(x => x.HourlyRate)
            .IsRequired();
        builder
            .Property(x => x.HourlyRateAfterLimitedCommission)
            .HasDefaultValue(0)
            .IsRequired();
        builder
            .Property(x => x.LimitedCommission)
            .HasDefaultValue(0)
            .IsRequired();
        builder
            .Property(x => x.HourlyRateAfterPayrollCommission)
            .HasDefaultValue(0)
            .IsRequired();
        builder
            .Property(x => x.PayrollCommission)
            .HasDefaultValue(0)
            .IsRequired();
        builder
            .Property(x => x.HourlyRateAfterSelfCommission)
            .HasDefaultValue(0)
            .IsRequired();
        builder
            .Property(x => x.SelfCommission)
            .HasDefaultValue(0)
            .IsRequired();
        builder
            .Property(x => x.BreakTime)
            .IsRequired();
        builder
            .Property(x => x.CostPershift)
            .IsRequired();
        builder
            .Property(x => x.EmployeeCategoryId)
            .IsRequired();
        builder
            .Property(x => x.JobDescription)
            .IsRequired();
        builder
            .Property(x => x.CancellationReason)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.JobTypeId)
            .IsRequired();
        builder
            .Property(x => x.CostPershiftPerDay)
            .HasDefaultValue(0)
            .IsRequired();
        builder
            .Property(x => x.JobHoursPerDay)
            .HasDefaultValue(0)
            .IsRequired();
        builder
            .Property(x => x.Status)
            .HasDefaultValue(JobStatusEnum.Open)
            .IsRequired();
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
        builder
            .Property(x => x.IsDummy)
            .HasDefaultValue(false)
            .IsRequired();
        #endregion

        #region Global query filter
        builder.HasQueryFilter(x => !x.IsDeleted && x.IsActive);
        #endregion
    }
}

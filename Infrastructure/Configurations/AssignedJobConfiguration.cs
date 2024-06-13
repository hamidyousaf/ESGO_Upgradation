namespace Infrastructure.Configurations;

public class AssignedJobConfiguration : IEntityTypeConfiguration<AssignedJob>
{
    public void Configure(EntityTypeBuilder<AssignedJob> builder)
    {
        #region Entity configuration
        builder
        .HasKey(x => x.Id);
        builder
            .HasOne(x => x.Employee)
            .WithMany()
            .HasForeignKey(b => b.EmployeeId)
            .OnDelete(DeleteBehavior.NoAction)
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
            .Property(x => x.JobStatus)
            .HasDefaultValue(JobStatusEnum.Open)
            .IsRequired();
        builder
            .Property(x => x.IsSelected)
            .HasDefaultValue(true)
            .IsRequired();
        builder
            .Property(x => x.HourWorked)
            .HasDefaultValue(0)
            .IsRequired();
        builder
            .Property(x => x.SelectedDate)
            .IsRequired();
        #endregion
        #region Global query filter
        builder.HasQueryFilter(x => !x.IsDeleted && x.IsActive);
        #endregion
    }
}

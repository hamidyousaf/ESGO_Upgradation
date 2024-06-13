namespace Infrastructure.Configurations;

internal class ShiftConfiguration : IEntityTypeConfiguration<Shift>
{
    public void Configure(EntityTypeBuilder<Shift> builder)
    {
        #region Entity configuration
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Name)
            .HasMaxLength(255)
            .IsRequired()
            ;
        builder
            .Property(x => x.StartTime)
            .HasConversion<TimeOnlyConverter>()
            .IsRequired();
        builder
            .Property(x => x.EndTime)
            .HasConversion<TimeOnlyConverter>()
            .IsRequired();
        builder
            .HasOne(x => x.Employer)
            .WithMany()
            .HasForeignKey(b => b.EmployerId)
            .OnDelete(DeleteBehavior.NoAction)
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
        #endregion

        #region Global query filter
        builder.HasQueryFilter(x => !x.IsDeleted && x.IsActive);
        #endregion
    }
}
namespace Infrastructure.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        #region Entity configuration
        builder
            .HasKey(x => x.Id);
        builder
            .HasOne(x => x.Employer)
            .WithMany()
            .HasForeignKey(b => b.EmployerId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        builder
            .Property(x => x.Details)
            .HasMaxLength(1024)
            .IsRequired();
        builder
            .Property(x => x.Date)
            .IsRequired();
        builder
            .Property(x => x.Status)
            .HasDefaultValue(BookingStatusEnum.Unknown)
            .IsRequired();
        builder
            .Property(x => x.DocumentUrl)
            .HasMaxLength(255)
            .IsRequired();
        builder
            .Property(x => x.ReasonForRejection)
            .HasMaxLength(1024)
            .HasDefaultValue("")
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

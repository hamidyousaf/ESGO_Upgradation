
namespace Infrastructure.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        #region Entity configuration
        builder
        .HasKey(x => x.Id);
        builder
            .Property(x => x.Date)
            .IsRequired();
        builder
            .Property(x => x.Type)
            .HasDefaultValue(NotificationTypeEnum.UnKnown)
            .IsRequired();
        builder
            .Property(x => x.Content)
            .HasDefaultValue(string.Empty)
            .IsRequired();
        builder
            .Property(x => x.EmployeeId)
            .IsRequired(false);
        builder
            .Property(x => x.EmployerId)
            .IsRequired(false);
        builder
            .Property(x => x.IsRead)
            .HasDefaultValue(false)
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

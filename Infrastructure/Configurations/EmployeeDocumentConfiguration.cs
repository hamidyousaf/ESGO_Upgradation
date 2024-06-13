namespace Infrastructure.Configurations;

public class EmployeeDocumentConfiguration : IEntityTypeConfiguration<EmployeeDocument>
{
    public void Configure(EntityTypeBuilder<EmployeeDocument> builder)
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
            .HasOne(x => x.Document)
            .WithMany()
            .HasForeignKey(b => b.DocumentId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
        builder
            .Property(x => x.DocumentUrl)
            .HasMaxLength(255)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.Status)
            .HasDefaultValue(DocumentStatusEnum.Unknown)
            .IsRequired();
        builder
            .Property(x => x.Reason)
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

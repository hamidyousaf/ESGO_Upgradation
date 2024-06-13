namespace Infrastructure.Configurations;

public class DbsDocumentConfiguration : IEntityTypeConfiguration<DbsDocument>
{
    public void Configure(EntityTypeBuilder<DbsDocument> builder)
    {
        #region Entity configuration
        builder
        .HasKey(x => x.Id);
        builder
            .Property(x => x.Url)
            .HasMaxLength(255)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .HasOne(x => x.Employee)
            .WithMany()
            .HasForeignKey(b => b.EmployeeId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
        builder
            .HasOne(x => x.DocumentType)
            .WithMany()
            .HasForeignKey(b => b.DocumentTypeId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
        builder
            .Property(x => x.DocumentNumber)
            .IsRequired();
        builder
            .Property(x => x.Status)
            .HasDefaultValue(DbsDocumentStatusEnum.Unknown)
            .IsRequired();
        builder
            .Property(x => x.RejectionReason)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.IsActive)
            .HasDefaultValue(false)
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

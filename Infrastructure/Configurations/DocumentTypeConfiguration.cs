namespace Infrastructure.Configurations;

public class DocumentTypeConfiguration : IEntityTypeConfiguration<DocumentType>
{
    public void Configure(EntityTypeBuilder<DocumentType> builder)
    {
        #region Entity configuration
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();
        builder
            .Property(x => x.GroupNo)
            .HasMaxLength(10)
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

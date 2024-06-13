namespace Infrastructure.Configurations;

internal class EmployementTypeConfiguration : IEntityTypeConfiguration<EmployementType>
{
    public void Configure(EntityTypeBuilder<EmployementType> builder)
    {
        #region Entity configuration
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Name)
            .HasMaxLength(255)
            .IsRequired();
        builder
            .Property(x => x.Description)
            .HasDefaultValue("")
            .HasMaxLength(255)
            .IsRequired();
        builder
            .Property(x => x.Order)
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

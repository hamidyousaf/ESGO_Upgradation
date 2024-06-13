namespace Infrastructure.Configurations;

internal class EmployeeTypeConfiguration : IEntityTypeConfiguration<EmployeeType>
{
    public void Configure(EntityTypeBuilder<EmployeeType> builder)
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
            .Property(x => x.MinRate)
            .HasDefaultValue(0.0)
            .IsRequired();
        builder
            .Property(x => x.ParentId)
            .HasDefaultValue(0)
            .IsRequired();
        builder
            .Property(x => x.ParentId)
            .HasDefaultValue(0)
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

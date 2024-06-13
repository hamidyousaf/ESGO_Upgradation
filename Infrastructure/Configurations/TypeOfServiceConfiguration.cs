namespace Infrastructure.Configurations;

public class TypeOfServiceConfiguration : IEntityTypeConfiguration<TypeOfService>
{
    public void Configure(EntityTypeBuilder<TypeOfService> builder)
    {
        #region Entity configuration
        builder
            .HasKey(x => x.Id);
        builder
            .HasOne(x => x.Employer)
            .WithMany(x => x.TypeOfServices)
            .HasForeignKey(b => b.EmployerId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
        builder
            .Property(x => x.TypeOfServiceId)
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

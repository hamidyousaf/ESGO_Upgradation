namespace Infrastructure.Configurations;

public class StarterFormQuestionConfiguration : IEntityTypeConfiguration<StarterFormQuestion>
{
    public void Configure(EntityTypeBuilder<StarterFormQuestion> builder)
    {
        #region Entity configuration
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Question)
            .HasMaxLength(1024)
            .IsRequired();
        builder
            .Property(x => x.Order)
            .IsRequired();
        builder
            .Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();
        #endregion

        #region Global query filter
        builder.HasQueryFilter(x => !x.IsDeleted);
        #endregion
    }
}

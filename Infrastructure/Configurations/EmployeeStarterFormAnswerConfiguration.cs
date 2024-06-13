namespace Infrastructure.Configurations;

public class EmployeeStarterFormAnswerConfiguration : IEntityTypeConfiguration<EmployeeStarterFormAnswer>
{
    public void Configure(EntityTypeBuilder<EmployeeStarterFormAnswer> builder)
    {
        #region Entity configuration
        builder
            .HasKey(x => x.Id);
        builder
            .HasOne(x => x.Question)
            .WithMany()
            .HasForeignKey(x => x.QuestionId)
            .IsRequired();
        builder
            .HasOne(x => x.Employee)
            .WithMany()
            .HasForeignKey(x => x.EmployeeId)
            .IsRequired();
        builder
            .Property(x => x.YesOrNo)
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

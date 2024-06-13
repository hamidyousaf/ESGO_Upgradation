
namespace Infrastructure.Configurations;

public class EmployerFavouriteConfiguration : IEntityTypeConfiguration<EmployerFavourite>
{
    public void Configure(EntityTypeBuilder<EmployerFavourite> builder)
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
            .HasOne(x => x.Employer)
            .WithMany()
            .HasForeignKey(b => b.EmployerId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
        #endregion
    }
}

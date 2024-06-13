namespace Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User, Roles, string> (options)
{
    public DbSet<Book> Books { get; set; }
    public DbSet<EmployeeType> EmployeeTypes { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployementType> EmployementTypes { get; set; }
    public DbSet<Qualification> Qualifications { get; set; }
    public DbSet<Employement> Employements { get; set; }
    public DbSet<Reference> References { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<EmployeeDocument> EmployeeDocuments { get; set; }
    public DbSet<Employer> Employers { get; set; }
    public DbSet<EmployerContactDetail> EmployerContactDetails { get; set; }
    public DbSet<TypeOfService> TypeOfServices { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Shift> Shifts { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<Timesheet> Timesheets { get; set; }
    public DbSet<StarterFormQuestion> StarterFormQuestions { get; set; }
    public DbSet<EmployeeStarterFormAnswer> EmployeeStarterFormAnswers  { get; set; }
    public DbSet<ShadowShift> ShadowShifts { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<MonthlySupervisionReport> MonthlySupervisionReports { get; set; }
    public DbSet<AssignedJob> AssignedJobs { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<DocumentType> DocumentTypes { get; set; }
    public DbSet<DbsDocument> DbsDocuments { get; set; }
    public DbSet<EmployeeFavourite> EmployeeFavourites { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<EmployerFavourite> EmployerFavourites { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region Apply entity configuration
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        #endregion
    }
}

namespace Infrastructure.Repositories.UnitOfWork;

internal class UnitOfWork(ApplicationDbContext _dbContext, IMapper _mapper) : IUnitOfWork
{
    public IBookRepository BookRepository => new BookRepository(_dbContext);
    public IEmployeeRepository EmployeeRepository => new EmployeeRepository(_mapper, _dbContext);
    public IEmployementTypeRepository EmployementTypeRepository => new EmployementTypeRepository(_mapper, _dbContext);
    public IEmployeeTypeRepository EmployeeTypeRepository =>  new EmployeeTypeRepository(_mapper, _dbContext);
    public IQualificationRepository QualificationRepository => new QualificationRepository(_dbContext, _mapper);
    public IEmployementRepository EmployementRepository => new EmployementRepository(_dbContext, _mapper);
    public IReferenceRepository ReferenceRepository => new ReferenceRepository(_dbContext, _mapper);
    public IDocumentRepository DocumentRepository => new DocumentRepository(_dbContext, _mapper);
    public IEmployeeDocumentRepository EmployeeDocumentRepository => new EmployeeDocumentRepository(_dbContext, _mapper);
    public IEmployerRepository EmployerRepository => new EmployerRepository(_dbContext, _mapper);
    public IEmployerContactDetailRepository EmployerContactDetailRepository =>  new EmployerContactDetailRepository(_dbContext);
    public ITypeOfServiceRepository TypeOfServiceRepository => new TypeOfServiceRepository(_dbContext);
    public IBookingRepository BookingRepository => new BookingRepository(_dbContext, _mapper);
    public IShiftRepository ShiftRepository => new ShiftRepository(_dbContext, _mapper);
    public IJobRepository JobRepository => new JobRepository(_dbContext, _mapper);
    public ITimesheetRepository TimesheetRepository => new TimesheetRepository(_dbContext, _mapper);
    public IStarterFormQuestionRepository StarterFormQuestionRepository => new StarterFormQuestionRepository(_dbContext);
    public IEmployeeStarterFormAnswerRepository EmployeeStarterFormAnswerRepository =>  new EmployeeStarterFormAnswerRepository(_dbContext);
    public IShadowShiftRepository ShadowShiftRepository => new ShadowShiftRepository(_dbContext, _mapper);
    public IFeedbackRepository FeedbackRepository => new FeedbackRepository(_dbContext, _mapper);
    public IMonthlySupervisionReportRepository MonthlySupervisionReportRepository => new MonthlySupervisionReportRepository(_dbContext, _mapper);
    public IAssignedJobRepository AssignedJobRepository => new AssignedJobRepository(_dbContext, _mapper);
    public IInvoiceRepository InvoiceRepository =>  new InvoiceRepository(_dbContext);
    public IDocumentTypeRepository DocumentTypeRepository => new DocumentTypeRepository(_dbContext, _mapper);
    public IDbsDocumentRepository DbsDocumentRepository => new DbsDocumentRepository(_dbContext, _mapper);
    public IEmployeeFavouriteRepository EmployeeFavouriteRepository => new EmployeeFavouriteRepository(_dbContext);
    public INotificationRepository NotificationsRepository => new NotificationRepository(_dbContext, _mapper);
    public IEmployerFavouriteRepository EmployerFavouriteRepository =>  new EmployerFavouriteRepository(_dbContext);

    public bool HasChanges()
    {
        return _dbContext.ChangeTracker.HasChanges();
    }

    public async Task<bool> IsCompleted()
    {
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public IDbTransaction BeginTransaction()
    {
        var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);
        return transaction.GetDbTransaction();
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}

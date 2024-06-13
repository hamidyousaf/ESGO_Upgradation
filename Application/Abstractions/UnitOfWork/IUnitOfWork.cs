namespace Domain.Abstractions.UnitOfWork;

public interface IUnitOfWork
{
    #region Add repositories here
    IBookRepository BookRepository { get; }
    IEmployeeRepository EmployeeRepository { get; }
    IEmployementTypeRepository EmployementTypeRepository { get; }
    IEmployeeTypeRepository EmployeeTypeRepository { get; }
    IQualificationRepository QualificationRepository { get; }
    IEmployementRepository EmployementRepository { get; }
    IReferenceRepository ReferenceRepository { get; }
    IDocumentRepository DocumentRepository { get; }
    IEmployeeDocumentRepository EmployeeDocumentRepository { get; }
    IEmployerRepository EmployerRepository { get; }
    IEmployerContactDetailRepository EmployerContactDetailRepository { get; }
    ITypeOfServiceRepository TypeOfServiceRepository { get; }
    IBookingRepository BookingRepository { get; }
    IShiftRepository ShiftRepository { get; }
    IJobRepository JobRepository { get; }
    ITimesheetRepository TimesheetRepository { get; }
    IStarterFormQuestionRepository StarterFormQuestionRepository { get; }
    IEmployeeStarterFormAnswerRepository EmployeeStarterFormAnswerRepository { get; }
    IShadowShiftRepository ShadowShiftRepository { get; }
    IMonthlySupervisionReportRepository MonthlySupervisionReportRepository{ get; }
    IFeedbackRepository FeedbackRepository { get; }
    IAssignedJobRepository AssignedJobRepository { get; }
    IInvoiceRepository InvoiceRepository { get; }
    IDocumentTypeRepository DocumentTypeRepository { get; }
    IDbsDocumentRepository DbsDocumentRepository { get; }
    IEmployeeFavouriteRepository EmployeeFavouriteRepository { get; }
    IEmployerFavouriteRepository EmployerFavouriteRepository { get; }
    INotificationRepository NotificationsRepository { get; }
    #endregion

    Task<bool> IsCompleted();
    bool HasChanges();
    IDbTransaction BeginTransaction();
    Task SaveChangesAsync();
}

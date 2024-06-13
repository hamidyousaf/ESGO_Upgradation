using Application.Abstractions;
using Application.Abstractions.Services;
using Domain.Entities;
using Humanizer;
using Wkhtmltopdf.NetCore;

namespace Domain.CQRS.Admins;

public sealed class GetAllEmployeesByStatusHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetAllEmployeesByStatusQuery, Result<PaginationModel<GetAllEmployeesByStatusResponse>>>
{
    public async Task<Result<PaginationModel<GetAllEmployeesByStatusResponse>>> Handle(GetAllEmployeesByStatusQuery request, CancellationToken cancellationToken)
    {
        var isExist = Enum.IsDefined(typeof(EmployeeAccountStatusEnum), request.Status);
        if (!isExist)
            return Result<PaginationModel<GetAllEmployeesByStatusResponse>>.Fail("Invalid status code.");

        var employees = _unitOfWork.EmployeeRepository.GetAllPendingEmployee(request.Status);

        // Add pagination
        PaginationModel<GetAllEmployeesByStatusResponse> model = new PaginationModel<GetAllEmployeesByStatusResponse>();
        if (request.PageSize > 0)
        {
            var result = new PagedList<GetAllEmployeesByStatusResponse>(employees, request.PageIndex, request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }

        return Result<PaginationModel<GetAllEmployeesByStatusResponse>>.Success(model, "Employee list.");
    }
}

public sealed class ChangeEmployeeStatusHandler(IUnitOfWork _unitOfWork, IMailService _mailService, IConfiguration _configuration) : IRequestHandler<ChangeEmployeeStatusQuery, Result<bool>>
{
    public async Task<Result<bool>> Handle(ChangeEmployeeStatusQuery request, CancellationToken cancellationToken)
    {
        // check is status code valid.
        var isExist = Enum.IsDefined(typeof(EmployeeAccountStatusEnum), request.Request.Status);
        if (!isExist)
            return Result<bool>.Fail("Invalid status code.");

        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }
        byte employeeStatus = employee.AccountStatus;

        employee.AccountStatus = request.Request.Status;
        employee.AccountStatusChangeReason = request.Request.Reason;
        await _unitOfWork.SaveChangesAsync();

        string subject = string.Empty;
        string content = string.Empty;
        string username = string.Empty;
        if (request.Request.Status == (byte)EmployeeAccountStatusEnum.Activated && employeeStatus == (byte)EmployeeAccountStatusEnum.Pending)
        {
            subject = "Account Approval:ESGO";
            content = "Welcome to Esgo healthcare. Your application has now been approved!     We are excited to have you on board with us.    You are now able to log into your account and start applying for your first shift!  <p style=\"color:#000;font-weight:100;text-align: center;font-size:20px;padding:10px 10px 0px 10px;\">  <a href=\"Url\" style=\"background:  #192a77 0% 0% no-repeat padding-box;border-radius: 6px;opacity: 1;color: #fff;padding: 10px 30px;width:100px;margin:0px auto;text-decoration: none;\"> Login</a></p>  </p>   <p style=\"color: #000; font-weight: 400; font-size: 18px; padding: 0px 10px 0px 10px;\">  Download our mobile app and immerse yourself in an enhanced, connective experience where you can manage your own flexible shift patterns and book shifts from  anywhere at any time.    <p style=\"color:#000;font-weight:100;text-align: center;font-size:20px;padding:10px 10px 0px 10px;\">  <a href=\"https://apps.apple.com/us/app/tesso/id1569340487\" target=\"_blank\">  <img src=\"https://tesso.co.uk/assets/home/app-store1.png\"  width=\"180\" height=\"50\" alt=\"Appstore\" href=\"#\"/> </a>  <a href=\"https://play.google.com/store/apps/details?id=com.antilia.tesso.employee\" target=\"_blank\">  <img src=\"https://tesso.co.uk/assets/home/g-play.png\"  width=\"180\" height=\"50\" alt=\"Playstore\" href=\"#\"/> </a>   </p>  </p>   <p style=\"color: #000; font-weight: 400; font-size: 18px; padding: 0px 10px 0px 10px;\">   <b>Please note:</b> Take care to double-check the location of the care home before you apply.     </p>   <p style=\"color: #000; font-weight: 400; font-size: 18px; padding: 0px 10px 0px 10px;\">   If your application is successful you will receive an email to confirm your shift.  <p>If you require any assistance,our support team is on hand to help.</p>              <p>Just drop us an email at <a href=\"mailto:Info@esgo.co.uk\" target=\"_blank\">Info@esgo.co.uk</a> <br> or                  call us                  on <a href=\"tel:020-3838-2056\" target=\"_blank\">020-3838-2056</a> </p>  ";
            username = $"{employee.FirstName} {employee.LastName}";
            content = content.Replace("Url", $"{_configuration["WebsiteSetting:Url"]}/login");
        }
        else if (request.Request.Status == (byte)EmployeeAccountStatusEnum.InActivated && employeeStatus == (byte)EmployeeAccountStatusEnum.Activated)
        {
            subject = "Account deactivated:ESGO";
            content = "Your account has been deactivated and all outstanding shifts and applied jobs have been cancelled.   Please contact ESGO team if this was not you or you think a mistake has been made .  ";
            username = $"{employee.FirstName} {employee.LastName}";
        }
        else if (request.Request.Status == (byte)EmployeeAccountStatusEnum.Activated && employeeStatus == (byte)EmployeeAccountStatusEnum.InActivated)
        {
            subject = "Account activated:ESGO";
            content = "Thank you for choosing ESGO ,  your account has now been activated and you are now free to send in your required shifts through the portal . Remember our team is always in hand to help you. Alternatively get in touch on 020 3838 2056/Bookings@esgo.co.uk ";
            username = $"{employee.FirstName} {employee.LastName}";
        }

        if (!string.IsNullOrEmpty(subject))
        {
            // send email.
            _ = Task.Run(() =>
            {
                _mailService.Send(employee.Email, subject, content, username);
            });
        }

        return Result<bool>.Success(true, "Employee status changes successfully.");
    }
}

public sealed class GetEmployeeAllDetailsByIdHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetEmployeeAllDetailsByIdQuery, Result<GetEmployeeAllDetailsByIdResponce>>
{
    public async Task<Result<GetEmployeeAllDetailsByIdResponce>> Handle(GetEmployeeAllDetailsByIdQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetEmployeeAllDetailsById(request.EmployeeId).FirstOrDefaultAsync(cancellationToken);
        if (employee is null)
        {
            return Result<GetEmployeeAllDetailsByIdResponce>.Fail("Employee doesn't exist.");
        }
        employee.PersonalReference = await _unitOfWork.ReferenceRepository.GetPersonalReferenceByEmployeeId(request.EmployeeId).FirstOrDefaultAsync(cancellationToken);
        employee.ProfessionalReference = await _unitOfWork.ReferenceRepository.GetProfessionalReferenceByEmployeeId(request.EmployeeId).FirstOrDefaultAsync(cancellationToken);
        employee.Qualifications = await _unitOfWork.QualificationRepository.GetQualificationsByEmployeeId(request.EmployeeId).ToListAsync(cancellationToken);
        employee.Employements = await _unitOfWork.EmployementRepository.GetEmployementsByEmployeeId(request.EmployeeId).ToListAsync(cancellationToken);
        employee.TrainingCertificates = await _unitOfWork.EmployeeDocumentRepository.GetDocumentsByEmployee(request.EmployeeId).ToListAsync(cancellationToken);

        var query = from questions in _unitOfWork.StarterFormQuestionRepository.GetAllReadOnly()
                    join answers in _unitOfWork.EmployeeStarterFormAnswerRepository.GetAllReadOnly()
                    on questions.Id equals answers.QuestionId into Details
                    from m in Details.DefaultIfEmpty()
                    where m.EmployeeId == request.EmployeeId
                    select new StarterFormQuestionsResponce
                    {
                        Id = questions.Id,
                        Question = questions.Question,
                        YesOrNo = m.YesOrNo
                    };
        employee.StarterFormQuestions = await query.ToListAsync(cancellationToken);
        employee.DbsDocuments = await _unitOfWork.DbsDocumentRepository.GetDbsDocumentsByEmployeeId(request.EmployeeId).ToListAsync(cancellationToken);

        return Result<GetEmployeeAllDetailsByIdResponce>.Success(employee, "Employee collected successfully.");
    }
}

public sealed class GetEmployeeShortDetailsByIdHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetEmployeeShortDetailsByIdQuery, Result<GetEmployeeShortDetailsByIdResponce>>
{
    public async Task<Result<GetEmployeeShortDetailsByIdResponce>> Handle(GetEmployeeShortDetailsByIdQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetEmployeeShortDetailsById(request.EmployeeId).FirstOrDefaultAsync(cancellationToken);
        if (employee is null)
        {
            return Result<GetEmployeeShortDetailsByIdResponce>.Fail("Employee doesn't exist.");
        }

        return Result<GetEmployeeShortDetailsByIdResponce>.Success(employee, "Employee collected successfully.");
    }
}


public sealed class DeleteEmployeesByIdsHandler(UserManager<User> _userManager, IUnitOfWork _unitOfWork)
    : IRequestHandler<DeleteEmployeesByIdsCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteEmployeesByIdsCommand request, CancellationToken cancellationToken)
    {
        // we only delete pending, outofportal and esgolead employee.
        var employees = await _unitOfWork
            .EmployeeRepository
            .GetAll()
            .Where(employee => request.Request.EmployeeIds.Contains(employee.Id)
                        && (employee.AccountStatus == (byte)EmployeeAccountStatusEnum.Pending
                            || employee.AccountStatus == (byte)EmployeeAccountStatusEnum.OutOfPortal
                            || employee.AccountStatus == (byte)EmployeeAccountStatusEnum.ESGOLeads))
            .ToListAsync(cancellationToken);

        employees.ForEach(emp =>
        {
            emp.IsDeleted = true;
            emp.IsActive = false;
        });
        _unitOfWork.EmployeeRepository.ChangeRange(employees);

        // also delete in aspnetuser table.
        var users = await _userManager
            .Users
            .Where(user => employees.Select(x => x.UserId.ToString().ToLower()).Contains(user.Id.ToLower()))
            .ToListAsync();

        foreach (var user in users)
        {
            await _userManager.DeleteAsync(user);
        }

        await _unitOfWork.SaveChangesAsync();
        return Result<bool>.Success(true, "Employee deleted.");
    }
}

public sealed class DeleteEmployersByIdsHandler(UserManager<User> _userManager, IUnitOfWork _unitOfWork)
    : IRequestHandler<DeleteEmployersByIdsCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteEmployersByIdsCommand request, CancellationToken cancellationToken)
    {
        // we only delete pending, outofportal and esgolead employee.
        var employers = await _unitOfWork
            .EmployerRepository
            .GetAll()
            .Where(employer => request.Request.EmployerIds.Contains(employer.Id)
                        && employer.AccountStatus == (byte)EmployerAccountStatusEnum.Pending)
            .ToListAsync(cancellationToken);

        employers.ForEach(emp =>
        {
            emp.IsDeleted = true;
            emp.IsActive = false;
        });

        _unitOfWork.EmployerRepository.ChangeRange(employers);
        // also delete in aspnetuser table.
        var users = await _userManager
            .Users
            .Where(user => employers.Select(x => x.UserId.ToString().ToLower()).Contains(user.Id.ToLower()))
            .ToListAsync();

        foreach (var user in users)
        {
            await _userManager.DeleteAsync(user);
        }

        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Employer deleted.");
    }
}

public sealed class GetEmployerAllDetailsByIdHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetEmployerAllDetailsByIdQuery, Result<GetEmployerAllDetailsByIdResponce>>
{
    public async Task<Result<GetEmployerAllDetailsByIdResponce>> Handle(GetEmployerAllDetailsByIdQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employer = await _unitOfWork.EmployerRepository.GetEmployerAllDetailsById(request.EmployerId).FirstOrDefaultAsync(cancellationToken);
        if (employer is null)
        {
            return Result<GetEmployerAllDetailsByIdResponce>.Fail("Employer doesn't exist.");
        }
        var x = await _unitOfWork
             .TypeOfServiceRepository
             .GetAllReadOnly()
             .Where(type => type.EmployerId == request.EmployerId)
             .Select(type => type.TypeOfServiceId)
             .ToArrayAsync(cancellationToken);
        employer.TypeOfService = x.Select(value => Enum.GetName(typeof(TypeOfServiceEnum), value)).ToArray();

        employer.ContactDetails = await _unitOfWork
            .EmployerContactDetailRepository
            .GetAllReadOnly()
            .Where(details => details.EmployerId == request.EmployerId)
            .Select(details => new ContactDetailDto
            {
                ContactName = details.ContactName,
                Email = details.Email,
                PhoneNumber = details.PhoneNumber,
            })
            .ToListAsync(cancellationToken);

        return Result<GetEmployerAllDetailsByIdResponce>.Success(employer, "Employer collected successfully.");
    }
}

public sealed class GetAllDocumentsByEmployeeHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetAllDocumentsByEmployeeQuery, Result<ResultForGetDocumentsByEmployeeResponse>>
{
    public async Task<Result<ResultForGetDocumentsByEmployeeResponse>> Handle(GetAllDocumentsByEmployeeQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.EmployeeId);
        if (employee is null)
        {
            return Result<ResultForGetDocumentsByEmployeeResponse>.Fail("Employee doesn't exist.");
        }

        ResultForGetDocumentsByEmployeeResponse result = new();
        // get employee documents by employee.
        result.Documents = await _unitOfWork.EmployeeDocumentRepository.GetDocumentsByEmployee(request.EmployeeId).ToListAsync(cancellationToken);
        result.TotalDocuments = await _unitOfWork.DocumentRepository.GetDocumentByCategoryId(employee.EmployeeTypeId).CountAsync(cancellationToken);
        result.TotalUploadedDocuments = result.Documents.Count();

        return Result<ResultForGetDocumentsByEmployeeResponse>.Success(result, "Employee dosuments collected successfully.");
    }
}

public sealed class AddShadowShiftHandler(IUnitOfWork _unitOfWork, IFileHelper _fileHelper) : IRequestHandler<AddShadowShiftCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddShadowShiftCommand request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }
        // save file.
        var url = await _fileHelper.UploadFile("Shadow Shifts", request.Request.File);

        // add shadow shift.
        var shift = new ShadowShift()
        {
            EmployeeId = request.Request.EmployeeId,
            Url = url
        };

        await _unitOfWork.ShadowShiftRepository.Add(shift);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Shadow shift added.");
    }
}

public sealed class AddInterviewHandler(IUnitOfWork _unitOfWork, IFileHelper _fileHelper) : IRequestHandler<AddInterviewCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddInterviewCommand request, CancellationToken cancellationToken)
    {
        // check is status code valid.
        var isExist = Enum.IsDefined(typeof(InterviewStatusEnum), request.Request.Status);
        if (!isExist)
            return Result<bool>.Fail("Invalid status code.");

        // check is status code valid.
        if (request.Request.Status == (byte)InterviewStatusEnum.Unknown)
        {
            return Result<bool>.Fail("Invalid status code.");
        }

        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }
        // save file.
        var url = await _fileHelper.UploadFile("Interview files", request.Request.File);

        // add interview details.
        employee.InterviewStatus = request.Request.Status;
        employee.InterviewRemarks = request.Request.Remarks;
        employee.InterviewFileUrl = url;

        _unitOfWork.EmployeeRepository.Change(employee);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Interview details added successfully.");
    }
}

public sealed class AddMonthlySupervisionReportHandler(IUnitOfWork _unitOfWork, IFileHelper _fileHelper)
    : IRequestHandler<AddMonthlySupervisionReportCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddMonthlySupervisionReportCommand request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }
        // save file.
        var url = await _fileHelper.UploadFile("Monthly Supervision Reports", request.Request.File);

        // add shadow shift.
        var report = new MonthlySupervisionReport()
        {
            EmployeeId = request.Request.EmployeeId,
            Url = url,
            Date = request.Request.Date
        };

        await _unitOfWork.MonthlySupervisionReportRepository.Add(report);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Report added.");
    }
}

public sealed class ChangeInterviewStatusHandler(IUnitOfWork _unitOfWork, IFileHelper _fileHelper)
    : IRequestHandler<ChangeInterviewStatusCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ChangeInterviewStatusCommand request, CancellationToken cancellationToken)
    {
        var isExist = Enum.IsDefined(typeof(InterviewStatusEnum), request.Request.Status);
        if (!isExist)
            return Result<bool>.Fail("Invalid status code.");

        // check is status code valid.
        if (request.Request.Status == (byte)InterviewStatusEnum.Unknown)
        {
            return Result<bool>.Fail("Invalid status code.");
        }

        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }
        // save file.
        var url = await _fileHelper.UploadFile("Interview Files", request.Request.File);

        employee.InterviewStatus = request.Request.Status;
        employee.InterviewRemarks = request.Request.Remarks;
        employee.InterviewFileUrl = url;

        _unitOfWork.EmployeeRepository.Change(employee);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Interview status changed.");
    }
}

public sealed class AddFeedbackHandler(IUnitOfWork _unitOfWork) : IRequestHandler<AddFeedbackCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddFeedbackCommand request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        // add shadow shift.
        var feedback = new Feedback()
        {
            EmployeeId = request.Request.EmployeeId,
            Description = request.Request.Description
        };

        await _unitOfWork.FeedbackRepository.Add(feedback);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Feedback added.");
    }
}

public sealed class GetShadowShiftsHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetShadowShiftsQuery, Result<List<GetShadowShiftsResponse>>>
{
    public async Task<Result<List<GetShadowShiftsResponse>>> Handle(GetShadowShiftsQuery request, CancellationToken cancellationToken)
    {
        // get shadow shifts.
        var shifts = await _unitOfWork
            .ShadowShiftRepository
            .GetShadowShiftsByEmployeeId(request.EmplyeeId)
            .ToListAsync(cancellationToken);

        return Result<List<GetShadowShiftsResponse>>.Success(shifts, "Shadow shift collected.");
    }
}

public sealed class GetMonthlySupervisionReportsHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetMonthlySupervisionReportsQuery, Result<List<GetMonthlySupervisionReportsResponse>>>
{
    public async Task<Result<List<GetMonthlySupervisionReportsResponse>>> Handle(GetMonthlySupervisionReportsQuery request, CancellationToken cancellationToken)
    {
        // get shadow shifts.
        var reports = await _unitOfWork
            .MonthlySupervisionReportRepository
            .GetMonthlySupervisionReportsByEmployeeId(request.EmplyeeId)
            .ToListAsync(cancellationToken);

        return Result<List<GetMonthlySupervisionReportsResponse>>.Success(reports, "Reports collected.");
    }
}

public sealed class GetAllBookingsHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetAllBookingsQuery, Result<PaginationModel<GetAllBookingsResponce>>>
{
    public async Task<Result<PaginationModel<GetAllBookingsResponce>>> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
    {
        var bookings = _unitOfWork.BookingRepository.GetAllBookings();

        // Add pagination
        PaginationModel<GetAllBookingsResponce> model = new PaginationModel<GetAllBookingsResponce>();
        if (request.PageSize > 0)
        {
            var result = new PagedList<GetAllBookingsResponce>(bookings, request.PageIndex, request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }

        return Result<PaginationModel<GetAllBookingsResponce>>.Success(model, "Booking list.");
    }
}

public sealed class GetFeedbacksHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetFeedbacksQuery, Result<List<GetFeedBacksResponse>>>
{
    public async Task<Result<List<GetFeedBacksResponse>>> Handle(GetFeedbacksQuery request, CancellationToken cancellationToken)
    {
        // get feedbacks.
        var feedbacks = await _unitOfWork.FeedbackRepository.GetFeedbacksByEmployeeId(request.EmplyeeId).ToListAsync(cancellationToken);

        return Result<List<GetFeedBacksResponse>>.Success(feedbacks, "Feedbacks collected.");
    }
}

public sealed class ChangeEmployeeDocumentStatusHandler(IUnitOfWork _unitOfWork) : IRequestHandler<ChangeEmployeeDocumentStatusQuery, Result<bool>>
{
    public async Task<Result<bool>> Handle(ChangeEmployeeDocumentStatusQuery request, CancellationToken cancellationToken)
    {
        // check is status code valid.
        var isExist = Enum.IsDefined(typeof(DocumentStatusEnum), request.Request.Status);
        if (!isExist)
            return Result<bool>.Fail("Invalid status code.");

        // check is status code valid.
        if (request.Request.Status == (byte)DocumentStatusEnum.Unknown || request.Request.Status == (byte)DocumentStatusEnum.VerificationUnderProcess)
        {
            return Result<bool>.Fail("Invalid status code.");
        }

        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        // get employee documents by employee.
        var employeeDocument = await _unitOfWork.EmployeeDocumentRepository.GetById(request.Request.EmployeeDocumentId);
        if (employeeDocument is null)
        {
            return Result<bool>.Fail("Employee document doesn't exist.");
        }

        // check the document is of employee.
        if (employeeDocument.EmployeeId != request.Request.EmployeeId)
        {
            return Result<bool>.Fail("You are unable to delete documents.");
        }

        // update the status of documents.
        employeeDocument.Status = request.Request.Status;
        employeeDocument.Reason = request.Request.Reason;

        await _unitOfWork.IsCompleted();

        return Result<bool>.Success(true, "Employee document status changed successfully.");
    }
}

public sealed class GetAllEmployersByStatusHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetAllEmployersByStatusQuery, Result<PaginationModel<GetAllEmployersByStatusResponse>>>
{
    public async Task<Result<PaginationModel<GetAllEmployersByStatusResponse>>> Handle(GetAllEmployersByStatusQuery request, CancellationToken cancellationToken)
    {
        var isExist = Enum.IsDefined(typeof(EmployerAccountStatusEnum), request.Status);
        if (!isExist)
            return Result<PaginationModel<GetAllEmployersByStatusResponse>>.Fail("Invalid status code.");

        var employers = _unitOfWork.EmployerRepository.GetAllPendingEmployers(request.Status);

        // Add pagination
        PaginationModel<GetAllEmployersByStatusResponse> model = new PaginationModel<GetAllEmployersByStatusResponse>();
        if (request.PageSize > 0)
        {
            var result = new PagedList<GetAllEmployersByStatusResponse>(employers, request.PageIndex, request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }

        return Result<PaginationModel<GetAllEmployersByStatusResponse>>.Success(model, "Employer list.");
    }
}

public sealed class GetDbsExpiredEmployeesHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetDbsExpiredEmployeesQuery, Result<PaginationModel<GetDbsExpiredEmployeesResponse>>>
{
    public async Task<Result<PaginationModel<GetDbsExpiredEmployeesResponse>>> Handle(GetDbsExpiredEmployeesQuery request, CancellationToken cancellationToken)
    {
        var employees = _unitOfWork.EmployeeRepository.GetDbsExpiredEmployees();

        // Add pagination
        PaginationModel<GetDbsExpiredEmployeesResponse> model = new PaginationModel<GetDbsExpiredEmployeesResponse>();
        if (request.PageSize > 0)
        {
            var result = new PagedList<GetDbsExpiredEmployeesResponse>(employees, request.PageIndex, request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }

        return Result<PaginationModel<GetDbsExpiredEmployeesResponse>>.Success(model, "Employee list.");
    }
}

public sealed class GetTrainingCertificateExpiredEmployeesHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetTrainingCertificateExpiredEmployeesQuery, Result<PaginationModel<GetTrainingCertificateExpiredEmployeesResponse>>>
{
    public async Task<Result<PaginationModel<GetTrainingCertificateExpiredEmployeesResponse>>> Handle(GetTrainingCertificateExpiredEmployeesQuery request, CancellationToken cancellationToken)
    {
        var employeesIds = await _unitOfWork
            .EmployeeDocumentRepository
            .GetAllReadOnly()
            .Where(x => x.ExpiryDate.Date < DateTime.UtcNow.Date).Select(x => x.EmployeeId)
            .ToListAsync(cancellationToken);

        var employees = _unitOfWork.EmployeeRepository.GetEmployeesByIds(employeesIds);

        // Add pagination
        PaginationModel<GetTrainingCertificateExpiredEmployeesResponse> model = new PaginationModel<GetTrainingCertificateExpiredEmployeesResponse>();
        if (request.PageSize > 0)
        {
            var result = new PagedList<GetTrainingCertificateExpiredEmployeesResponse>(employees, request.PageIndex, request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }

        return Result<PaginationModel<GetTrainingCertificateExpiredEmployeesResponse>>.Success(model, "Employee list.");
    }
}

public sealed class ChangeEmployerStatusHandler(IUnitOfWork _unitOfWork, IMailService _mailService) : IRequestHandler<ChangeEmployerStatusQuery, Result<bool>>
{
    public async Task<Result<bool>> Handle(ChangeEmployerStatusQuery request, CancellationToken cancellationToken)
    {
        // check is status code valid.
        var isExist = Enum.IsDefined(typeof(EmployerAccountStatusEnum), request.Request.Status);
        if (!isExist)
            return Result<bool>.Fail("Invalid status code.");

        // check is status code valid.
        if (request.Request.Status == (byte)EmployerAccountStatusEnum.Pending)
        {
            return Result<bool>.Fail("Invalid status code.");
        }

        // check is employer exists.
        var employer = await _unitOfWork.EmployerRepository.GetById(request.Request.EmployerId);
        if (employer is null)
        {
            return Result<bool>.Fail("Employer doesn't exist.");
        }

        byte employerStatus = employer.AccountStatus;
        employer.AccountStatus = request.Request.Status;
        await _unitOfWork.SaveChangesAsync();

        string subject = string.Empty;
        string content = string.Empty;
        string username = string.Empty;
        if (request.Request.Status == (byte)EmployerAccountStatusEnum.Active && employerStatus == (byte)EmployerAccountStatusEnum.Pending)
        {
            subject = $"Account Approval Confirmation for {employer.CompanyName} on ESGO";
            content = "<p>Dear {CareHomeName},</p>             <p>We are thrilled to confirm that your registration as an employer with ESGO has been successfully received and approved. Welcome aboard!                 Your dedication to providing exceptional care aligns perfectly with our mission at ESGO, and we are excited to support your efforts in managing your care home efficiently and provide the care you want to.</p>             <p>Your approved account grants you access to our comprehensive platform, enabling you to:</p>             <ol>                 <li>Post and manage job listings efficiently.</li>                 <li>Connect with experienced candidates.</li>                 <li>Streamline your recruitment process through our user-friendly interface.</li>                 <li>Access a network of professionals dedicated to enhancing the care home industry.                 </li>             </ol>             <p>To get started, please log in to your account using the credentials you provided during registration. You'll find a dashboard tailored to your needs, offering seamless navigation and powerful tools to assist you in finding the right candidates for your care home.</p>             <p>Thank you for choosing ESGO as your healthcare agency. We look forward to supporting your endeavours and contributing to the success of {CareHomeName}.</p>             <p>Best Regards,</p>";
            content = content.Replace("{CareHomeName}", employer.CompanyName);
            username = $"{employer.Name}";
        }
        else if (request.Request.Status == (byte)EmployerAccountStatusEnum.InActive && employerStatus == (byte)EmployerAccountStatusEnum.Active)
        {
            subject = "Account deactivated:ESGO";
            content = "Your account has been deactivated and all outstanding shifts have been cancelled.     Please contact a member of the ESGO team if this was not you or you think a mistake has been made.";
            username = $"{employer.Name}";
        }
        else if (request.Request.Status == (byte)EmployerAccountStatusEnum.Active && employerStatus == (byte)EmployerAccountStatusEnum.InActive)
        {
            subject = "Your account has been activated:ESGO";
            content = "Thank you for choosing ESGO ,  your account has now been activated and you are now free to send in your required shifts through the portal . Remember our team is always in hand to help you. Alternatively get in touch on 020 3838 2056/Bookings@esgo.co.uk ";
            username = $"{employer.Name}";
        }

        string[] emails = await _unitOfWork.EmployerContactDetailRepository.GetAllReadOnly()
            .Where(contact => contact.EmployerId == employer.Id)
            .Select(contact => contact.Email)
            .ToArrayAsync(cancellationToken);

        if (!string.IsNullOrEmpty(subject))
        {
            // send email to employer.
            _ = Task.Run(() =>
            {
                _mailService.Send(employer.Email, subject, content, username);
            });

            // send email to contact emails.
            emails.Select(email => Task.Run(() =>
            {
                _mailService.Send(email, subject, content, username);
            })).ToList();
        }

        return Result<bool>.Success(true, "Employer status changes successfully.");
    }
}

public sealed class AddJobCommissionHandler(IUnitOfWork _unitOfWork) : IRequestHandler<AddJobCommissionCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddJobCommissionCommand request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployerRepository.GetById(request.Request.EmployerId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employer doesn't exist.");
        }

        employee.SelfCommission = request.Request.SelfCommission;
        employee.LimitedCommission = request.Request.LimitedCommission;
        employee.PayrollCommission = request.Request.PayrollCommission;
        await _unitOfWork.IsCompleted();

        return Result<bool>.Success(true, "Commissions added successfully.");
    }
}

public sealed class ChangeBookingStatusHandler(IUnitOfWork _unitOfWork, IMailService _mailService) : IRequestHandler<ChangeBookingStatusQuery, Result<bool>>
{
    public async Task<Result<bool>> Handle(ChangeBookingStatusQuery request, CancellationToken cancellationToken)
    {
        // check is status code valid.
        var isExist = Enum.IsDefined(typeof(BookingStatusEnum), request.Request.Status);
        if (!isExist)
            return Result<bool>.Fail("Invalid status code.");

        // check is status code valid.
        if (request.Request.Status == (byte)BookingStatusEnum.Unknown || request.Request.Status == (byte)BookingStatusEnum.New)
        {
            return Result<bool>.Fail("Invalid status code.");
        }

        if (request.Request.Status == (byte)BookingStatusEnum.Rejected && string.IsNullOrEmpty(request.Request.Reason))
        {
            return Result<bool>.Fail("You must add reason to rejection.");
        }

        // check is booking exists.
        var booking = await _unitOfWork.BookingRepository.GetById(request.Request.BookingId);
        if (booking is null)
        {
            return Result<bool>.Fail("Booking doesn't exist.");
        }

        // check employer exist.
        var employer = await _unitOfWork.EmployerRepository.GetById(booking.EmployerId);
        if (employer is null)
        {
            return Result<bool>.Fail("Employer doesn't exist.");
        }

        booking.Status = request.Request.Status;
        booking.ReasonForRejection = request.Request.Reason;
        await _unitOfWork.SaveChangesAsync();


        string subject = string.Empty;
        string content = string.Empty;
        string username = string.Empty;
        if (request.Request.Status == (byte)BookingStatusEnum.Inprogress)
        {
            subject = $"Your booking has been moved to inprogress :ESGO";
            content = "Your request with booking Id BookingId is currently being processed. We will update you once it's done.";
            content = content.Replace("BookingId", booking.Id.ToString());
        }
        else if (request.Request.Status == (byte)BookingStatusEnum.Done)
        {
            subject = "Your booking has been completed :ESGO";
            content = "The booking with Id BookingId  has been completed.";
            content = content.Replace("BookingId", booking.Id.ToString());
        }

        if (!string.IsNullOrEmpty(subject))
        {
            // send email to employer.
            _ = Task.Run(() =>
            {
                _mailService.Send(employer.Email, subject, content, username);
            });
        }

        return Result<bool>.Success(true, "Booking status changes successfully.");
    }
}

public sealed class GetEmployeeTypesHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetEmployeeTypesQuery, Result<List<EmployeeTypeResponse>>>
{
    public async Task<Result<List<EmployeeTypeResponse>>> Handle(GetEmployeeTypesQuery request, CancellationToken cancellationToken)
    {
        // get all employee types.
        var types = await _unitOfWork
            .EmployeeTypeRepository
            .GetEmployeeTypes()
            .ToListAsync(cancellationToken);


        return Result<List<EmployeeTypeResponse>>.Success(types, "Employee type collected successfully.");
    }
}

public sealed class GetEmployeeTypeByIdHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetEmployeeTypeByIdQuery, Result<GetEmployeeTypeByIdResponse>>
{
    public async Task<Result<GetEmployeeTypeByIdResponse>> Handle(GetEmployeeTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var type = await _unitOfWork.EmployeeTypeRepository.GetEmployeeTypeById(request.EmplyeeTypeId).FirstOrDefaultAsync(cancellationToken);
        if (type is null)
        {
            return Result<GetEmployeeTypeByIdResponse>.Fail("Employee type doesn't exist.");
        }

        return Result<GetEmployeeTypeByIdResponse>.Success(type, "Employee type collected successfully.");
    }
}

public sealed class GetEmployeesByEmployeeTypeIdHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetEmployeesByEmployeeTypeIdQuery, Result<List<GetEmployeesByEmployeeTypeIdResponse>>>
{
    public async Task<Result<List<GetEmployeesByEmployeeTypeIdResponse>>> Handle(GetEmployeesByEmployeeTypeIdQuery request, CancellationToken cancellationToken)
    {
        var employees = await _unitOfWork.EmployeeRepository
            .GetEmployeesByEmployeeTypeId(request.EmplyeeTypeId)
            .ToListAsync(cancellationToken);

        return Result<List<GetEmployeesByEmployeeTypeIdResponse>>.Success(employees, "Employees collected successfully.");
    }
}

public sealed class GetShiftsByEmployerIdHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetShiftsByEmployerIdQuery, Result<List<GetShiftsResponse>>>
{
    public async Task<Result<List<GetShiftsResponse>>> Handle(GetShiftsByEmployerIdQuery request, CancellationToken cancellationToken)
    {
        var shifts = await _unitOfWork.ShiftRepository
            .GetShiftsByEmployerId(request.EmployerId)
            .ToListAsync(cancellationToken);

        return Result<List<GetShiftsResponse>>.Success(shifts, "Shifts collected successfully.");
    }
}

public sealed class AddJobHandler(IUnitOfWork _unitOfWork, IMediator _mediator, IConfiguration _configuration, IMailService _mailService) : IRequestHandler<AddJobCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddJobCommand request, CancellationToken cancellationToken)
    {
        // begin the transaction here.
        var transaction = _unitOfWork.BeginTransaction();

        // check employer exist.
        var employer = await _unitOfWork.EmployerRepository.GetById(request.Request.EmployerId);
        if (employer is null)
        {
            return Result<bool>.Fail("Employer doesn't exist.");
        }

        // validate the job entity.
        var jobResult = await _mediator.Send(new JobCommand(request.Request));
        if (!jobResult.Succeeded)
        {
            return Result<bool>.Fail(jobResult.Message);
        }

        // get employees.
        var employees = await _unitOfWork.EmployeeRepository.GetAllReadOnly()
            .Where(employee => request.Request.EmployeeIds.Contains(employee.Id))
            .ToListAsync(cancellationToken);

        // add multiple jobs accourding to number of days.
        var getTotalJobDays = DateHelper.GetTotalDays(request.Request.StartDate, request.Request.EndDate);

        foreach (var dayOffset in Enumerable.Range(0, getTotalJobDays))
        {
            var newJob = new Job(); // Create a new instance of Job for each iteration
            newJob.EmployerId = jobResult.Data.EmployerId;
            newJob.JobTypeId = jobResult.Data.JobTypeId;
            newJob.EmployeeTypeId = jobResult.Data.EmployeeTypeId;
            newJob.BookingId = jobResult.Data.BookingId;
            newJob.Date = jobResult.Data.Date.AddDays(dayOffset);
            newJob.ShiftId = jobResult.Data.ShiftId;
            newJob.ShiftStartTime = jobResult.Data.ShiftStartTime;
            newJob.ShiftEndTime = jobResult.Data.ShiftEndTime;
            newJob.BreakTime = jobResult.Data.BreakTime;
            newJob.CostPershift = jobResult.Data.CostPershift;
            newJob.JobDescription = jobResult.Data.JobDescription;
            newJob.CostPershiftPerDay = jobResult.Data.CostPershiftPerDay;
            newJob.JobHoursPerDay = jobResult.Data.JobHoursPerDay;
            newJob.IsDummy = jobResult.Data.IsDummy;
            newJob.Status = (byte)JobStatusEnum.Open;
            newJob.EmployeeCategoryId = jobResult.Data.EmployeeCategoryId;
            newJob.IsFixedRate = jobResult.Data.IsFixedRate;
            
            // calculate rates
            newJob.HourlyRate = jobResult.Data.HourlyRate;

            newJob.SelfCommission = employer.SelfCommission;
            newJob.HourlyRateAfterSelfCommission = jobResult.Data.HourlyRate - (jobResult.Data.HourlyRate * employer.SelfCommission/100);

            newJob.PayrollCommission = employer.PayrollCommission;
            newJob.HourlyRateAfterPayrollCommission = jobResult.Data.HourlyRate - (jobResult.Data.HourlyRate * employer.PayrollCommission / 100);

            newJob.LimitedCommission = employer.LimitedCommission;
            newJob.HourlyRateAfterLimitedCommission = jobResult.Data.HourlyRate - (jobResult.Data.HourlyRate * employer.LimitedCommission / 100);


            if (newJob.IsFixedRate)
            {
                newJob.FixedRate = jobResult.Data.FixedRate;
                var commssion = Convert.ToDecimal(_configuration["JobSettings:FixedRateCommission"] ?? "0");
                newJob.FixedRateAfterCommission = newJob.FixedRate - (newJob.FixedRate * commssion / 100);
            }

            await _unitOfWork.JobRepository.Add(newJob);
            await _unitOfWork.SaveChangesAsync();

            // asign job to employee.
            foreach (var employee in employees)
            {
                var assignedJob = new AssignedJob()
                {
                    JobId = newJob.Id,
                    EmployeeId = employee.Id,
                    EmployerId = newJob.EmployerId,
                    AppliedDate = DateTime.UtcNow.Date,
                    HourWorked = newJob.JobHoursPerDay,
                    JobStatus = (byte)JobStatusEnum.Open,
                    IsSelected = true,
                    SelectedDate = DateTime.UtcNow
                };

                await _unitOfWork.AssignedJobRepository.Add(assignedJob);
                await _unitOfWork.SaveChangesAsync();

                // send email to employee for job confirmation.
                string subject = $"Confirm your shift at {employer.CompanyName}:ESGO";
                string content = "You have been assigned the shift you have applied for  on JobDate from StartTime to EndTime Please confirm you are still available by clicking the link to avoid missing out .    </p><p style=\"color: #000; font-weight: 100; font-size: 20px; padding: 20px 10px 0px 10px;\"> Click here to confirm </p>   <p style=\"color:#000;font-weight:600;text-align: center;font-size:20px;padding:20px 10px 0px 10px;\">  <a href=\"Url\" style=\"background: #ED1C24 0% 0% no-repeat padding-box;border-radius: 6px;opacity: 1;color: #fff;padding: 10px 20px;width:100px;margin:0px auto;text-decoration: none;\"> Proceed </a></p>  ";
                content = content.Replace("OrganizationName", employer.CompanyName);
                content = content.Replace("JobDate", newJob.Date.ToString("yyyy-MM-dd"));
                content = content.Replace("StartTime", newJob.ShiftStartTime.ToString("hh:mm tt"));
                content = content.Replace("EndTime", newJob.ShiftEndTime.ToString("hh:mm tt"));
                content = content.Replace("Url", $"{_configuration["WebsiteSetting:Url"]}/jobconfirmation/{Encryption.Crypt(assignedJob.Id.ToString())}");
                string username = employer.Name;

                _ = Task.Run(() =>
                {
                    _mailService.Send(employee.Email, subject, content, username);
                });
            }

            if (jobResult.Data.BookingId.HasValue && jobResult.Data.BookingId != 0)
            {
                // send email to to employer like Booking in progress.
                string subject1 = $"Your booking is in progress :ESGO";
                string content1 = $"Your request with booking Id BookingId is currently being processed. We will update you once it's done.";
                content1 = content1.Replace("BookingId", newJob.BookingId.ToString());
                string username1 = employer.Name;

                _ = Task.Run(() =>
                {
                    _mailService.Send(employer.Email, subject1, content1, username1);
                });
            }

        }

        // end the transaction here.
        transaction.Commit();

        return Result<bool>.Success(true, "Jobs added successfully.");
    }
}

public sealed class AddMultilpleJobHandler(IUnitOfWork _unitOfWork, IMediator _mediator, IConfiguration _configuration, IMailService _mailService) : IRequestHandler<AddMultilpleJobCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddMultilpleJobCommand request, CancellationToken cancellationToken)
    {
        // begin the transaction here.
        var transaction = _unitOfWork.BeginTransaction();

        foreach (var item in request.Request)
        {
            // check employer exist.
            var employer = await _unitOfWork.EmployerRepository.GetById(item.EmployerId);
            if (employer is null)
            {
                return Result<bool>.Fail("Employer doesn't exist.");
            }

            // validate the job entity.
            var jobResult = await _mediator.Send(new JobCommand(item));
            if (!jobResult.Succeeded)
            {
                return Result<bool>.Fail(jobResult.Message);
            }

            // get employees.
            var employees = await _unitOfWork.EmployeeRepository.GetAllReadOnly()
                .Where(employee => item.EmployeeIds.Contains(employee.Id))
                .ToListAsync(cancellationToken);

            // add multiple jobs accourding to number of days.
            var getTotalJobDays = DateHelper.GetTotalDays(item.StartDate, item.EndDate);

            foreach (var dayOffset in Enumerable.Range(0, getTotalJobDays))
            {
                var newJob = new Job(); // Create a new instance of Job for each iteration
                newJob.EmployerId = jobResult.Data.EmployerId;
                newJob.JobTypeId = jobResult.Data.JobTypeId;
                newJob.EmployeeTypeId = jobResult.Data.EmployeeTypeId;
                newJob.BookingId = jobResult.Data.BookingId;
                newJob.Date = jobResult.Data.Date;
                newJob.ShiftId = jobResult.Data.ShiftId;
                newJob.ShiftStartTime = jobResult.Data.ShiftStartTime;
                newJob.ShiftEndTime = jobResult.Data.ShiftEndTime;
                newJob.BreakTime = jobResult.Data.BreakTime;
                newJob.CostPershift = jobResult.Data.CostPershift;
                newJob.JobDescription = jobResult.Data.JobDescription;
                newJob.CostPershiftPerDay = jobResult.Data.CostPershiftPerDay;
                newJob.JobHoursPerDay = jobResult.Data.JobHoursPerDay;
                newJob.IsDummy = jobResult.Data.IsDummy;
                newJob.Status = (byte)JobStatusEnum.Open;
                newJob.EmployeeCategoryId = jobResult.Data.EmployeeCategoryId;
                newJob.IsFixedRate = jobResult.Data.IsFixedRate;

                // calculate rates
                newJob.HourlyRate = jobResult.Data.HourlyRate;

                newJob.SelfCommission = employer.SelfCommission;
                newJob.HourlyRateAfterSelfCommission = jobResult.Data.HourlyRate - (jobResult.Data.HourlyRate * employer.SelfCommission / 100);

                newJob.PayrollCommission = employer.PayrollCommission;
                newJob.HourlyRateAfterPayrollCommission = jobResult.Data.HourlyRate - (jobResult.Data.HourlyRate * employer.PayrollCommission / 100);

                newJob.LimitedCommission = employer.LimitedCommission;
                newJob.HourlyRateAfterLimitedCommission = jobResult.Data.HourlyRate - (jobResult.Data.HourlyRate * employer.LimitedCommission / 100);

                if (newJob.IsFixedRate)
                {
                    newJob.FixedRate = jobResult.Data.FixedRate;
                    var commssion = Convert.ToDecimal(_configuration["JobSettings:FixedRateCommission"] ?? "0");
                    newJob.FixedRateAfterCommission = newJob.FixedRate - (newJob.FixedRate * commssion / 100);
                }

                await _unitOfWork.JobRepository.Add(newJob);
                await _unitOfWork.SaveChangesAsync();

                // asign job to employee.
                foreach (var employee in employees)
                {
                    var assignedJob = new AssignedJob()
                    {
                        JobId = newJob.Id,
                        EmployeeId = employee.Id,
                        EmployerId = newJob.EmployerId,
                        AppliedDate = DateTime.UtcNow.Date,
                        HourWorked = newJob.JobHoursPerDay,
                        JobStatus = (byte)JobStatusEnum.Open,
                        IsSelected = true,
                        SelectedDate = DateTime.UtcNow
                    };

                    await _unitOfWork.AssignedJobRepository.Add(assignedJob);
                    await _unitOfWork.SaveChangesAsync();

                    // send email to employee for job confirmation.
                    string subject = $"Confirm your shift at {employer.CompanyName}:ESGO";
                    string content = "You have been assigned the shift you have applied for  on JobDate from StartTime to EndTime Please confirm you are still available by clicking the link to avoid missing out .    </p><p style=\"color: #000; font-weight: 100; font-size: 20px; padding: 20px 10px 0px 10px;\"> Click here to confirm </p>   <p style=\"color:#000;font-weight:600;text-align: center;font-size:20px;padding:20px 10px 0px 10px;\">  <a href=\"Url\" style=\"background: #ED1C24 0% 0% no-repeat padding-box;border-radius: 6px;opacity: 1;color: #fff;padding: 10px 20px;width:100px;margin:0px auto;text-decoration: none;\"> Proceed </a></p>  ";
                    content = content.Replace("OrganizationName", employer.CompanyName);
                    content = content.Replace("JobDate", newJob.Date.ToString("yyyy-MM-dd"));
                    content = content.Replace("StartTime", newJob.ShiftStartTime.ToString("hh:mm tt"));
                    content = content.Replace("EndTime", newJob.ShiftEndTime.ToString("hh:mm tt"));
                    content = content.Replace("Url", $"{_configuration["WebsiteSetting:Url"]}/jobconfirmation/{Encryption.Crypt(assignedJob.Id.ToString())}");
                    string username = employer.Name;

                    _ = Task.Run(() =>
                    {
                        _mailService.Send(employee.Email, subject, content, username);
                    });
                }

                if (jobResult.Data.BookingId.HasValue && jobResult.Data.BookingId != 0)
                {
                    // send email to to employer like Booking in progress.
                    string subject1 = $"Your booking is in progress :ESGO";
                    string content1 = $"Your request with booking Id BookingId is currently being processed. We will update you once it's done.";
                    content1 = content1.Replace("BookingId", newJob.BookingId.ToString());
                    string username1 = employer.Name;

                    _ = Task.Run(() =>
                    {
                        _mailService.Send(employer.Email, subject1, content1, username1);
                    });
                }
            }
        }

        // end the transaction here.
        transaction.Commit();

        return Result<bool>.Success(true, "Jobs added successfully.");
    }
}

public sealed class JobHandler(IUnitOfWork _unitOfWork) : IRequestHandler<JobCommand, Result<Job>>
{
    public async Task<Result<Job>> Handle(JobCommand request, CancellationToken cancellationToken)
    {
        // check is status code valid.
        var isExist = Enum.IsDefined(typeof(EmployeeCategoryEnum), request.Request.EmployeeCategoryId);
        if (!isExist)
            return Result<Job>.Fail("Invalid status code.");

        // check Booking exist.
        if (request.Request.BookingId is not null)
        {
            var isBookingExist = await _unitOfWork
                .BookingRepository
                .GetAllReadOnly()
                .FirstOrDefaultAsync(booking => booking.Id == request.Request.BookingId);
            if (isBookingExist is null)
            {
                return Result<Job>.Fail("Booking doesn't exist.");
            }
            // change booking status to inprogress.
            isBookingExist.Status = (byte)BookingStatusEnum.Inprogress;
        }
        else
        {
            request.Request.BookingId = null;
        }

        // check employee type exist.
        var isEmployeeTypeExist = await _unitOfWork.EmployeeTypeRepository.GetById(request.Request.EmployeeTypeId);
        if (isEmployeeTypeExist is null)
        {
            return Result<Job>.Fail("Employee type doesn't exist.");
        }

        // check is job type valid.
        var isJobTypeExist = Enum.IsDefined(typeof(JobTypeEnum), request.Request.JobTypeId);
        if (!isJobTypeExist)
            return Result<Job>.Fail("Invalid status code.");


        // convert string to timeonly.
        TimeOnly.TryParse(request.Request.ShiftStartTime, out TimeOnly startTime);
        TimeOnly.TryParse(request.Request.ShiftEndTime, out TimeOnly endTime);

        // check starttime is in correct format.
        if (startTime.Ticks is 0)
        {
            return Result<Job>.Fail("Start time isn't in correct format.");
        }

        // check endtime is in correct format.
        if (endTime.Ticks is 0)
        {
            return Result<Job>.Fail("End time isn't in correct format.");
        }

        // check shift exist.
        TimeOnly shiftStartTime = startTime;
        TimeOnly shiftEndTime = endTime;
        var isShiftExist = await _unitOfWork.ShiftRepository.GetById(request.Request.ShiftId);
        if (isShiftExist is not null)
        {
            shiftStartTime = isShiftExist.StartTime;
            shiftEndTime = isShiftExist.EndTime;
        }

        // check hourly rate is valid.
        if (isEmployeeTypeExist.MinRate > request.Request.HourlyRate)
        {
            return Result<Job>.Fail($"Rate should be greater than {isEmployeeTypeExist.MinRate}.");
        }

        // check break time is valid.
        var isBreakTimeValid = Enum.IsDefined(typeof(BreakTimeEnum), request.Request.BreakTime);
        if (!isBreakTimeValid)
            return Result<Job>.Fail("Invalid break time.");

        // check start date is valid.
        if (request.Request.StartDate.Date < DateTime.UtcNow.Date)
        {
            return Result<Job>.Fail("Invalid start date.");
        }

        // check end date is valid.
        if (request.Request.EndDate.Date < DateTime.UtcNow.Date && request.Request.EndDate.Date < request.Request.StartDate.Date)
        {
            return Result<Job>.Fail("Invalid end date.");
        }

        /// calculate Cost per Shift. ----------------------

        var getTotalJobDays = DateHelper.GetTotalDays(request.Request.StartDate, request.Request.EndDate);
        var getJobMinutesPerDay = DateHelper.GetTotalMinutes(startTime, endTime);
        var getJobHoursPerDay = DateHelper.GetTotalHours(startTime, endTime);
        var breakMinutes = (double)request.Request.BreakTime * getTotalJobDays;
        var ratePerMinute = (double)(request.Request.HourlyRate / 60);

        var CostPerShift = ((getTotalJobDays * getJobMinutesPerDay) - breakMinutes) * ratePerMinute;

        /// ------------------------------------------------
        var job = new Job()
        {
            EmployerId = request.Request.EmployerId,
            JobTypeId = request.Request.JobTypeId,
            EmployeeTypeId = request.Request.EmployeeTypeId,
            BookingId = request.Request.BookingId,
            Date = request.Request.StartDate,
            ShiftId = request.Request.ShiftId,
            ShiftStartTime = shiftStartTime,
            ShiftEndTime = shiftEndTime,
            HourlyRate = request.Request.HourlyRate,
            BreakTime = request.Request.BreakTime,
            CostPershift = (decimal)CostPerShift,
            JobDescription = request.Request.JobDescription,
            CostPershiftPerDay = (decimal)CostPerShift / getTotalJobDays,
            JobHoursPerDay = (decimal)getJobHoursPerDay,
            EmployeeCategoryId = request.Request.EmployeeCategoryId,
            IsFixedRate = request.Request.IsFixedRate,
            FixedRate = request.Request.FixedRate
        };

        return Result<Job>.Success(job, "Job is validated successfully.");
    }
}

public sealed class UpdateTimesheetByIdHandler(IUnitOfWork _unitOfWork) : IRequestHandler<UpdateTimesheetByIdCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateTimesheetByIdCommand request, CancellationToken cancellationToken)
    {
        // check break time is valid.
        var isBreakTimeValid = Enum.IsDefined(typeof(BreakTimeEnum), request.Request.BreakTime);
        if (!isBreakTimeValid)
        {
            return Result<bool>.Fail("Invalid break time.");
        }

        // convert string to timeonly.
        TimeOnly.TryParse(request.Request.StartTime, out TimeOnly startTime);
        TimeOnly.TryParse(request.Request.EndTime, out TimeOnly endTime);

        // check starttime is in correct format.
        if (startTime.Ticks is 0)
        {
            return Result<bool>.Fail("Start time isn't in correct format.");
        }

        // check endtime is in correct format.
        if (endTime.Ticks is 0)
        {
            return Result<bool>.Fail("End time isn't in correct format.");
        }

        // check timesheet exist.
        var timesheet = await _unitOfWork
            .TimesheetRepository
            .GetWithCondition(timesheet => timesheet.Id == request.Request.TimesheetId)
                .Include(timesheet => timesheet.Job)
                    .ThenInclude(timesheet => timesheet.EmployeeType)
            .FirstOrDefaultAsync(cancellationToken);
        if (timesheet is null)
        {
            return Result<bool>.Fail("Timeshhet is not exist.");
        }

        // check hourly rate is valid.
        if (timesheet.Job.EmployeeType.MinRate > request.Request.HourlyRate)
        {
            return Result<bool>.Fail($"Rate should be greater than {timesheet.Job.EmployeeType.MinRate}.");
        }

        // calculate billable hours
        var totalMinutes = DateHelper.GetTotalMinutes(startTime, endTime);
        var time = TimeSpan.FromMinutes(totalMinutes);
        var totalHours = new TimeOnly((int)time.TotalHours, (int)time.Minutes);
        var billableHours = totalHours.AddMinutes(-request.Request.BreakTime);
        var billableHourInDecimal = DateHelper.ConvertToDecimal(billableHours);

        timesheet.Date = request.Request.Date;
        timesheet.HourlyRate = request.Request.HourlyRate;
        timesheet.BreakTime = request.Request.BreakTime;
        timesheet.Notes = request.Request.Notes;
        timesheet.StartTime = startTime;
        timesheet.EndTime = endTime;
        timesheet.UpdatedDate = DateTime.UtcNow;
        timesheet.BillableHours = billableHours;
        timesheet.BillableHourInDecimal = billableHourInDecimal;
        timesheet.TotalHours = totalHours;
        timesheet.TotalAmount = billableHourInDecimal * request.Request.HourlyRate;
        timesheet.OriginalHourlyRate = request.Request.HourlyRate;
        timesheet.OrginalTotalAmount = billableHourInDecimal * request.Request.HourlyRate;
        timesheet.TotalHolidayAmount = billableHourInDecimal * timesheet.Job.HolidayPayRate;

        _unitOfWork.TimesheetRepository.Change(timesheet);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Time sheet updated.");
    }
}

public sealed class GetJobsByStatusHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetJobsByStatusQuery, Result<PaginationModel<GetJobsByStatusResponse>>>
{
    public async Task<Result<PaginationModel<GetJobsByStatusResponse>>> Handle(GetJobsByStatusQuery request, CancellationToken cancellationToken)
    {
        // check is status valid.
        var isJobStatusValid = Enum.IsDefined(typeof(JobStatusEnum), request.Status);
        if (!isJobStatusValid)
        {
            return Result<PaginationModel<GetJobsByStatusResponse>>.Fail("Invalid status.");
        }

        // get jobs by status.
        var jobs = _unitOfWork.AssignedJobRepository.GetJobsByStatus(request.Status);

        // get timesheets by status
        var query = from job in jobs
                    join shift in _unitOfWork.ShiftRepository.GetAllReadOnly()
                    on job.ShiftId equals shift.Id into Details
                    from m in Details.DefaultIfEmpty()
                    select new GetJobsByStatusResponse
                    {
                        AssignedJobId = job.AssignedJobId,
                        OrganisationName = job.OrganisationName,
                        EmployeeTypeId = job.EmployeeTypeId,
                        JobDate = job.JobDate,
                        JobCreatedDate = job.JobCreatedDate,
                        JobId = job.JobId,
                        StartDate = job.StartDate,
                        Location = job.Location,
                        ShiftStartTime = job.ShiftStartTime,
                        ShiftEndTime = job.ShiftEndTime,
                        EmployeeType = job.EmployeeType,
                        ShiftId = job.ShiftId,
                        Status = job.Status,
                        EmployeeName = job.EmployeeName,
                        EmployeeId = job.EmployeeId,
                        ShiftDescription = job.ShiftId == 0 ? "Custom" : m.Name
                    };
        // Add pagination
        PaginationModel<GetJobsByStatusResponse> model = new PaginationModel<GetJobsByStatusResponse>();
        if (request.PageSize > 0)
        {
            var result = new PagedList<GetJobsByStatusResponse>(query, request.PageIndex, request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }

        return Result<PaginationModel<GetJobsByStatusResponse>>.Success(model, "Jobs list collected.");
    }
}

public sealed class GetUnsuccessfulJobsHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetUnsuccessfulJobsQuery, Result<PaginationModel<GetUnsuccessfulJobsResponse>>>
{
    public async Task<Result<PaginationModel<GetUnsuccessfulJobsResponse>>> Handle(GetUnsuccessfulJobsQuery request, CancellationToken cancellationToken)
    {
        // get all Unsuccessful bookings.
        var jobs = _unitOfWork.AssignedJobRepository.GetUnsuccessfulJobs();

        // Add pagination
        PaginationModel<GetUnsuccessfulJobsResponse> model = new PaginationModel<GetUnsuccessfulJobsResponse>();
        if (request.PageSize > 0)
        {
            var result = new PagedList<GetUnsuccessfulJobsResponse>(jobs, request.PageIndex, request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }

        return Result<PaginationModel<GetUnsuccessfulJobsResponse>>.Success(model, "Unsuccessful jobs list.");
    }
}

public sealed class GetConfirmedJobsHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetConfirmedJobsForAdminQuery, Result<PaginationModel<GetConfirmedJobsForAdminResponse>>>
{
    public async Task<Result<PaginationModel<GetConfirmedJobsForAdminResponse>>> Handle(GetConfirmedJobsForAdminQuery request, CancellationToken cancellationToken)
    {
        // get all Unsuccessful bookings.
        var jobs = _unitOfWork.AssignedJobRepository.GetConfirmedJobs();

        // Add pagination
        PaginationModel<GetConfirmedJobsForAdminResponse> model = new PaginationModel<GetConfirmedJobsForAdminResponse>();
        if (request.PageSize > 0)
        {
            var result = new PagedList<GetConfirmedJobsForAdminResponse>(jobs, request.PageIndex, request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }

        return Result<PaginationModel<GetConfirmedJobsForAdminResponse>>.Success(model, "Confirmed jobs list.");
    }
}

public sealed class GetCompletedJobsHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetCompletedJobsQuery, Result<PaginationModel<GetCompletedJobsResponse>>>
{
    public async Task<Result<PaginationModel<GetCompletedJobsResponse>>> Handle(GetCompletedJobsQuery request, CancellationToken cancellationToken)
    {
        // get all Unsuccessful bookings.
        var jobs = _unitOfWork.AssignedJobRepository.GetCompletedJobs();

        // Add pagination
        PaginationModel<GetCompletedJobsResponse> model = new PaginationModel<GetCompletedJobsResponse>();
        if (request.PageSize > 0)
        {
            var result = new PagedList<GetCompletedJobsResponse>(jobs, request.PageIndex, request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }

        return Result<PaginationModel<GetCompletedJobsResponse>>.Success(model, "Completed jobs list.");
    }
}

public sealed class GetAssignedJobEmployeeByIdForAdminHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetAssignedJobEmployeeByIdForAdminQuery, Result<GetAssignedJobEmployeeByIdForAdminResponse>>
{
    public async Task<Result<GetAssignedJobEmployeeByIdForAdminResponse>> Handle(GetAssignedJobEmployeeByIdForAdminQuery request, CancellationToken cancellationToken)
    {
        // get employee.
        var employee = await _unitOfWork.AssignedJobRepository.GetAssignedJobById(request.AssignedJobId).FirstOrDefaultAsync(cancellationToken);
        if (employee is null)
        {
            return Result<GetAssignedJobEmployeeByIdForAdminResponse>.Fail("Not found.");
        }

        return Result<GetAssignedJobEmployeeByIdForAdminResponse>.Success(employee, "Data collected.");
    }
}

public sealed class GetAssignedJobByIdForAdminHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetAssignedJobByIdForAdminQuery, Result<GetAssignedJobByIdForAdminResponce>>
{
    public async Task<Result<GetAssignedJobByIdForAdminResponce>> Handle(GetAssignedJobByIdForAdminQuery request, CancellationToken cancellationToken)
    {
        // get assigned job.
        var assignedJob = await _unitOfWork
            .AssignedJobRepository
            .GetAllReadOnly()
                .Include(assignedJob => assignedJob.Employer)
                .Include(assignedJob => assignedJob.Job)
                .Include(assignedJob => assignedJob.Employee)
                    .ThenInclude(assignedJob => assignedJob.EmployeeType)
            .Where(assignedJob => assignedJob.Id == request.AssignedJobId)
            .Select(assignedJob => new GetAssignedJobByIdForAdminResponce
            {
                AssignedJobId = assignedJob.Id,
                OrganisationName = assignedJob.Employer.CompanyName,
                HourlyRate = assignedJob.Job.HourlyRate,
                EmployeeType = assignedJob.Employee.EmployeeType.Name,
                JobCreatedDate = assignedJob.CreatedDate.Date,
                JobDate = assignedJob.Job.Date.Date,
                StartTime = assignedJob.Job.Date.Date.Add(assignedJob.Job.ShiftStartTime.ToTimeSpan()),
                EndTime = assignedJob.Job.Date.Date.Add(assignedJob.Job.ShiftEndTime.ToTimeSpan()),
                JobDescription = assignedJob.Job.JobDescription
            })
            .FirstOrDefaultAsync(cancellationToken);
        if (assignedJob is null)
        {
            return Result<GetAssignedJobByIdForAdminResponce>.Fail("Not found.");
        }

        return Result<GetAssignedJobByIdForAdminResponce>.Success(assignedJob, "Data collected.");
    }
}

public sealed class ChangeUTRNumberStatusHandler(IUnitOfWork _unitOfWork) : IRequestHandler<ChangeUTRNumberStatusCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ChangeUTRNumberStatusCommand request, CancellationToken cancellationToken)
    {
        // check is status code valid.
        var isExist = Enum.IsDefined(typeof(UTRNumberStatusEnum), request.Request.Status);
        if (!isExist)
            return Result<bool>.Fail("Invalid status code.");

        // check is status code valid.
        if (request.Request.Status == (byte)UTRNumberStatusEnum.Unknown || request.Request.Status == (byte)UTRNumberStatusEnum.Pending)
        {
            return Result<bool>.Fail("Invalid status code.");
        }

        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("employee doesn't exist.");
        }

        if (request.Request.Status == (byte)UTRNumberStatusEnum.Reject && string.IsNullOrEmpty(request.Request.Reason))
        {
            return Result<bool>.Fail("You must add reason to rejection.");
        }

        employee.UTRNumberStatus = request.Request.Status;
        employee.UTRNumberRejectionReason = request.Request.Reason;
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Utn number status changes successfully.");
    }
}

public sealed class ChangeCompanyNumberStatusHandler(IUnitOfWork _unitOfWork) : IRequestHandler<ChangeCompanyNumberStatusCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ChangeCompanyNumberStatusCommand request, CancellationToken cancellationToken)
    {
        // check is status code valid.
        var isExist = Enum.IsDefined(typeof(CompanyNumberStatusEnum), request.Request.Status);
        if (!isExist)
            return Result<bool>.Fail("Invalid status code.");

        // check is status code valid.
        if (request.Request.Status == (byte)CompanyNumberStatusEnum.Unknown || request.Request.Status == (byte)CompanyNumberStatusEnum.Pending)
        {
            return Result<bool>.Fail("Invalid status code.");
        }

        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("employee doesn't exist.");
        }

        if (request.Request.Status == (byte)CompanyNumberStatusEnum.Reject && string.IsNullOrEmpty(request.Request.Reason))
        {
            return Result<bool>.Fail("You must add reason to rejection.");
        }

        employee.CompanyNumberStatus = request.Request.Status;
        employee.CompanyNumberRejectionReason = request.Request.Reason;
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Company number status changes successfully.");
    }
}

public sealed class ChangeNMCRegistrationStatusHandler(IUnitOfWork _unitOfWork) : IRequestHandler<ChangeNMCRegistrationStatusCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ChangeNMCRegistrationStatusCommand request, CancellationToken cancellationToken)
    {
        // check is status code valid.
        var isExist = Enum.IsDefined(typeof(NMCPinStatusEnum), request.Request.Status);
        if (!isExist)
            return Result<bool>.Fail("Invalid status code.");

        // check is status code valid.
        if (request.Request.Status == (byte)NMCPinStatusEnum.Unknown || request.Request.Status == (byte)NMCPinStatusEnum.Pending)
        {
            return Result<bool>.Fail("Invalid status code.");
        }

        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("employee doesn't exist.");
        }

        if (request.Request.Status == (byte)NMCPinStatusEnum.Reject && string.IsNullOrEmpty(request.Request.Reason))
        {
            return Result<bool>.Fail("You must add reason to rejection.");
        }

        employee.NMCPinStatus = request.Request.Status;
        employee.NMCPinRejectionReason = request.Request.Reason;
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "NMC Pin status changes successfully.");
    }
}

public sealed class ChangeNationalInsuranceNumberStatusHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<ChangeNationalInsuranceNumberStatusCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ChangeNationalInsuranceNumberStatusCommand request, CancellationToken cancellationToken)
    {
        // check is status code valid.
        var isExist = Enum.IsDefined(typeof(NationalInsuranceNumberStatusEnum), request.Request.Status);
        if (!isExist)
            return Result<bool>.Fail("Invalid status code.");

        // check is status code valid.
        if (request.Request.Status == (byte)NationalInsuranceNumberStatusEnum.Unknown || request.Request.Status == (byte)NationalInsuranceNumberStatusEnum.Pending)
        {
            return Result<bool>.Fail("Invalid status code.");
        }

        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("employee doesn't exist.");
        }

        if (request.Request.Status == (byte)NationalInsuranceNumberStatusEnum.Reject && string.IsNullOrEmpty(request.Request.Reason))
        {
            return Result<bool>.Fail("You must add reason to rejection.");
        }

        employee.NationalInsuranceNumberStatus = request.Request.Status;
        employee.NationalInsuranceNumberRejectionReason = request.Request.Reason;
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "NMC Pin status changes successfully.");
    }
}

public sealed class ChangePersonalReferenceStatusHandler(IUnitOfWork _unitOfWork) : IRequestHandler<ChangePersonalReferenceStatusCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ChangePersonalReferenceStatusCommand request, CancellationToken cancellationToken)
    {
        // check is status code valid.
        var isExist = Enum.IsDefined(typeof(PersonalReferenceStatusEnum), request.Request.Status);
        if (!isExist)
            return Result<bool>.Fail("Invalid status code.");

        // check is status code valid.
        if (request.Request.Status == (byte)PersonalReferenceStatusEnum.Unknown || request.Request.Status == (byte)PersonalReferenceStatusEnum.Pending)
        {
            return Result<bool>.Fail("Invalid status code.");
        }

        // check is personal reference exists.
        var reference = await _unitOfWork
            .ReferenceRepository
            .GetAll()
            .FirstOrDefaultAsync(reference => reference.EmployeeId == request.Request.EmployeeId
            && reference.ReferenceTypeId == (byte)ReferenceTypeEnum.Personal, cancellationToken);
        if (reference is null)
        {
            return Result<bool>.Fail("Reference doesn't exist.");
        }

        if (request.Request.Status == (byte)PersonalReferenceStatusEnum.Reject && string.IsNullOrEmpty(request.Request.Reason))
        {
            return Result<bool>.Fail("You must add reason to rejection.");
        }

        reference.Status = request.Request.Status;
        reference.RejectionReason = request.Request.Reason;
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Reference status changes successfully.");
    }
}

public sealed class ChangeProfessionalReferenceStatusHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<ChangeProfessionalReferenceStatusCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ChangeProfessionalReferenceStatusCommand request, CancellationToken cancellationToken)
    {
        // check is status code valid.
        var isExist = Enum.IsDefined(typeof(ProfessionalReferenceStatusEnum), request.Request.Status);
        if (!isExist)
            return Result<bool>.Fail("Invalid status code.");

        // check is status code valid.
        if (request.Request.Status == (byte)ProfessionalReferenceStatusEnum.Unknown || request.Request.Status == (byte)ProfessionalReferenceStatusEnum.Pending)
        {
            return Result<bool>.Fail("Invalid status code.");
        }

        // check is personal reference exists.
        var reference = await _unitOfWork
            .ReferenceRepository
            .GetAll()
            .FirstOrDefaultAsync(employee => employee.EmployeeId == request.Request.EmployeeId
            && employee.ReferenceTypeId == (byte)ReferenceTypeEnum.Professional, cancellationToken);
        if (reference is null)
        {
            return Result<bool>.Fail("Reference doesn't exist.");
        }

        if (request.Request.Status == (byte)ProfessionalReferenceStatusEnum.Reject && string.IsNullOrEmpty(request.Request.Reason))
        {
            return Result<bool>.Fail("You must add reason to rejection.");
        }

        reference.Status = request.Request.Status;
        reference.RejectionReason = request.Request.Reason;
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Reference status changes successfully.");
    }
}

public sealed class ChangeBiometricResidenceCardStatusHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<ChangeBiometricResidenceCardStatusCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ChangeBiometricResidenceCardStatusCommand request, CancellationToken cancellationToken)
    {
        // check is status code valid.
        var isExist = Enum.IsDefined(typeof(BiometricResidenceCardStatusEnum), request.Request.Status);
        if (!isExist)
            return Result<bool>.Fail("Invalid status code.");

        // check is status code valid.
        if (request.Request.Status == (byte)BiometricResidenceCardStatusEnum.Unknown || request.Request.Status == (byte)BiometricResidenceCardStatusEnum.Pending)
        {
            return Result<bool>.Fail("Invalid status code.");
        }

        // check is personal reference exists.
        var employee = await _unitOfWork
            .EmployeeRepository
            .GetAll()
            .FirstOrDefaultAsync(employee => employee.Id == request.Request.EmployeeId, cancellationToken);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        if (request.Request.Status == (byte)BiometricResidenceCardStatusEnum.Reject && string.IsNullOrEmpty(request.Request.Reason))
        {
            return Result<bool>.Fail("You must add reason to rejection.");
        }

        employee.BiometricResidenceCardStatus = request.Request.Status;
        employee.BiometricResidenceCardRejectionReason = request.Request.Reason;
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Biometric residence card status changes successfully.");
    }
}

public sealed class ChangePassportStatusHandler(IUnitOfWork _unitOfWork) : IRequestHandler<ChangePassportStatusCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ChangePassportStatusCommand request, CancellationToken cancellationToken)
    {
        // check is status code valid.
        var isExist = Enum.IsDefined(typeof(PassportStatusEnum), request.Request.Status);
        if (!isExist)
            return Result<bool>.Fail("Invalid status code.");

        // check is status code valid.
        if (request.Request.Status == (byte)PassportStatusEnum.Unknown || request.Request.Status == (byte)PassportStatusEnum.Pending)
        {
            return Result<bool>.Fail("Invalid status code.");
        }

        // check is exists.
        var employee = await _unitOfWork
            .EmployeeRepository
            .GetAll()
            .FirstOrDefaultAsync(employee => employee.Id == request.Request.EmployeeId, cancellationToken);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        if (request.Request.Status == (byte)PassportStatusEnum.Reject && string.IsNullOrEmpty(request.Request.Reason))
        {
            return Result<bool>.Fail("You must add reason to rejection.");
        }

        employee.PassportStatus = request.Request.Status;
        employee.PassportRejectionReason = request.Request.Reason;
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Passport status changed successfully.");
    }
}

public sealed class ChangeDbsDocumentStatusHandler(IUnitOfWork _unitOfWork) : IRequestHandler<ChangeDbsDocumentStatusCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ChangeDbsDocumentStatusCommand request, CancellationToken cancellationToken)
    {
        // check is status code valid.
        var isExist = Enum.IsDefined(typeof(DbsDocumentStatusEnum), request.Request.Status);
        if (!isExist)
            return Result<bool>.Fail("Invalid status code.");

        // check is status code valid.
        if (request.Request.Status == (byte)DbsDocumentStatusEnum.Unknown || request.Request.Status == (byte)DbsDocumentStatusEnum.Pending)
        {
            return Result<bool>.Fail("Invalid status code.");
        }

        // check is exists.
        var document = await _unitOfWork
            .DbsDocumentRepository
            .GetAll()
            .FirstOrDefaultAsync(document => document.Id == request.Request.DocumentId && document.EmployeeId == request.Request.EmployeeId, cancellationToken);
        if (document is null)
        {
            return Result<bool>.Fail("Document doesn't exist.");
        }

        if (request.Request.Status == (byte)DbsDocumentStatusEnum.Reject && string.IsNullOrEmpty(request.Request.Reason))
        {
            return Result<bool>.Fail("You must add reason to rejection.");
        }

        document.Status = request.Request.Status;
        document.RejectionReason = request.Request.Reason;

        _unitOfWork.DbsDocumentRepository.Change(document);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "DbsDocument status changed successfully.");
    }
}

public sealed class ChangeDbsNumebrStatusHandler(IUnitOfWork _unitOfWork) : IRequestHandler<ChangeDbsNumebrStatusCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ChangeDbsNumebrStatusCommand request, CancellationToken cancellationToken)
    {
        // check is status code valid.
        var isExist = Enum.IsDefined(typeof(DbsNumebrStatusEnum), request.Request.Status);
        if (!isExist)
            return Result<bool>.Fail("Invalid status code.");

        // check is status code valid.
        if (request.Request.Status == (byte)DbsNumebrStatusEnum.Unknown || request.Request.Status == (byte)DbsNumebrStatusEnum.Pending)
        {
            return Result<bool>.Fail("Invalid status code.");
        }

        // check is exists.
        var employee = await _unitOfWork
            .EmployeeRepository
            .GetAll()
            .FirstOrDefaultAsync(employee => employee.Id == request.Request.EmployeeId, cancellationToken);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        if (request.Request.Status == (byte)DbsNumebrStatusEnum.Reject && string.IsNullOrEmpty(request.Request.Reason))
        {
            return Result<bool>.Fail("You must add reason to rejection.");
        }

        employee.DbsNumberStatus = request.Request.Status;
        employee.DbsNumberRejectionReason = request.Request.Reason;
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Dbs Numebr status changed successfully.");
    }
}

public sealed class ChangeAccessNIStatusHandler(IUnitOfWork _unitOfWork) : IRequestHandler<ChangeAccessNIStatusCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ChangeAccessNIStatusCommand request, CancellationToken cancellationToken)
    {
        // check is status code valid.
        var isExist = Enum.IsDefined(typeof(AccessNIStatusEnum), request.Request.Status);
        if (!isExist)
            return Result<bool>.Fail("Invalid status code.");

        // check is status code valid.
        if (request.Request.Status == (byte)AccessNIStatusEnum.Unknown || request.Request.Status == (byte)AccessNIStatusEnum.Pending)
        {
            return Result<bool>.Fail("Invalid status code.");
        }

        // check is exists.
        var employee = await _unitOfWork
            .EmployeeRepository
            .GetAll()
            .FirstOrDefaultAsync(employee => employee.Id == request.Request.EmployeeId, cancellationToken);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        if (request.Request.Status == (byte)AccessNIStatusEnum.Reject && string.IsNullOrEmpty(request.Request.Reason))
        {
            return Result<bool>.Fail("You must add reason to rejection.");
        }

        employee.AccessNIStatus = request.Request.Status;
        employee.AccessNIRejectionReason = request.Request.Reason;
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Access NI status changed successfully.");
    }
}

public sealed class ChangeNationalInsuranceStatusHandler(IUnitOfWork _unitOfWork) : IRequestHandler<ChangeNationalInsuranceStatusCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ChangeNationalInsuranceStatusCommand request, CancellationToken cancellationToken)
    {
        // check is status code valid.
        var isExist = Enum.IsDefined(typeof(NationalInsuranceStatusEnum), request.Request.Status);
        if (!isExist)
            return Result<bool>.Fail("Invalid status code.");

        // check is status code valid.
        if (request.Request.Status == (byte)NationalInsuranceStatusEnum.Unknown || request.Request.Status == (byte)NationalInsuranceStatusEnum.Pending)
        {
            return Result<bool>.Fail("Invalid status code.");
        }

        // check is exists.
        var employee = await _unitOfWork
            .EmployeeRepository
            .GetAll()
            .FirstOrDefaultAsync(employee => employee.Id == request.Request.EmployeeId, cancellationToken);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        if (request.Request.Status == (byte)NationalInsuranceStatusEnum.Reject && string.IsNullOrEmpty(request.Request.Reason))
        {
            return Result<bool>.Fail("You must add reason to rejection.");
        }

        employee.NationalInsuranceStatus = request.Request.Status;
        employee.NationalInsuranceRejectionReason = request.Request.Reason;
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "National Insurance status changed successfully.");
    }
}

public sealed class ChangeDbsCertificateStatusHandler(IUnitOfWork _unitOfWork) : IRequestHandler<ChangeDbsCertificateStatusCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ChangeDbsCertificateStatusCommand request, CancellationToken cancellationToken)
    {
        // check is status code valid.
        var isExist = Enum.IsDefined(typeof(DbsCertificateStatusEnum), request.Request.Status);
        if (!isExist)
            return Result<bool>.Fail("Invalid status code.");

        // check is status code valid.
        if (request.Request.Status == (byte)DbsCertificateStatusEnum.Unknown || request.Request.Status == (byte)DbsCertificateStatusEnum.Pending)
        {
            return Result<bool>.Fail("Invalid status code.");
        }

        // check is exists.
        var employee = await _unitOfWork
            .EmployeeRepository
            .GetAll()
            .FirstOrDefaultAsync(employee => employee.Id == request.Request.EmployeeId, cancellationToken);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        if (request.Request.Status == (byte)DbsCertificateStatusEnum.Reject && string.IsNullOrEmpty(request.Request.Reason))
        {
            return Result<bool>.Fail("You must add reason to rejection.");
        }

        employee.DbsCertificateStatus = request.Request.Status;
        employee.DbsCertificateRejectionReason = request.Request.Reason;
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Dbs Certificate status changed successfully.");
    }
}

public sealed class DeletePersonalReferenceHandler(IUnitOfWork _unitOfWork) : IRequestHandler<DeletePersonalReferenceCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeletePersonalReferenceCommand request, CancellationToken cancellationToken)
    {
        // check is personal reference exists.
        var reference = await _unitOfWork
            .ReferenceRepository
            .GetAll()
            .FirstOrDefaultAsync(reference => reference.EmployeeId == request.EmployeeId
            && reference.ReferenceTypeId == (byte)ReferenceTypeEnum.Personal
            && reference.Status == (byte)PersonalReferenceStatusEnum.Reject, cancellationToken);
        if (reference is null)
        {
            return Result<bool>.Fail("Reference doesn't exist.");
        }
        _unitOfWork.ReferenceRepository.DeleteByEntity(reference);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Reference deleted successfully.");
    }
}

public sealed class DeleteProfessionalReferenceHandler(IUnitOfWork _unitOfWork) : IRequestHandler<DeleteProfessionalReferenceCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteProfessionalReferenceCommand request, CancellationToken cancellationToken)
    {
        // check is personal reference exists.
        var reference = await _unitOfWork
            .ReferenceRepository
            .GetAll()
            .FirstOrDefaultAsync(reference => reference.EmployeeId == request.EmployeeId
            && reference.ReferenceTypeId == (byte)ReferenceTypeEnum.Professional
            && reference.Status == (byte)ProfessionalReferenceStatusEnum.Reject, cancellationToken);
        if (reference is null)
        {
            return Result<bool>.Fail("Reference doesn't exist.");
        }
        _unitOfWork.ReferenceRepository.DeleteByEntity(reference);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Reference deleted successfully.");
    }
}

public sealed class GetEmployersForInvoiceHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetEmployersForInvoiceQuery, Result<List<GetEmployersForInvoiceResponse>>>
{
    public async Task<Result<List<GetEmployersForInvoiceResponse>>> Handle(GetEmployersForInvoiceQuery request, CancellationToken cancellationToken)
    {
        // get employers.
        var employers = await _unitOfWork
            .EmployerRepository
            .GetAll()
            .Where(employer => employer.AccountStatus == (byte)EmployerAccountStatusEnum.Active)
            .Select(employer => new GetEmployersForInvoiceResponse()
            {
                Id = employer.Id,
                Name = $"{employer.CompanyName}",
                CompanyName = employer.CompanyName
            })
            .ToListAsync(cancellationToken);

        return Result<List<GetEmployersForInvoiceResponse>>.Success(employers, "Employers collected successfully.");
    }
}

public sealed class GetTotalAmountOfEmployerHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetTotalAmountOfEmployerQuery, Result<GetTotalAmountOfEmployerResponse>>
{
    public async Task<Result<GetTotalAmountOfEmployerResponse>> Handle(GetTotalAmountOfEmployerQuery request, CancellationToken cancellationToken)
    {
        // get sum amount
        var timesheetAmount = await _unitOfWork
            .TimesheetRepository
            .GetAllReadOnly()
            .Include(timesheet => timesheet.AssignedJob)
            .Where(timesheet => timesheet.EmployerId == request.Request.EmployerId
                            && timesheet.Date.Date >= request.Request.StartDate.Date
                            && timesheet.Date.Date <= request.Request.EndDate.Date
                            && timesheet.AssignedJob.JobStatus == (byte)JobStatusEnum.Completed)
            .SumAsync(timesheet => timesheet.TotalAmount);

        var result = new GetTotalAmountOfEmployerResponse()
        {
            Amount = timesheetAmount
        };

        return Result<GetTotalAmountOfEmployerResponse>.Success(result, "Amount collected successfully.");
    }
}

public sealed class AddInvoiceHandler(IUnitOfWork _unitOfWork) : IRequestHandler<AddInvoiceCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddInvoiceCommand request, CancellationToken cancellationToken)
    {
        // check employer exists.
        var employer = await _unitOfWork
            .EmployerRepository
            .GetAllReadOnly()
            .AnyAsync(employer => employer.Id == request.Request.EmployerId);
        if (!employer)
        {
            return Result<bool>.Fail("Employer isn't exist.");
        }

        // check from date and to date is valid.
        if (request.Request.From.Date > request.Request.To.Date)
        {
            return Result<bool>.Fail("From date should be before To date.");
        }

        // check amount is valid.
        var timesheetAmount = await _unitOfWork
            .TimesheetRepository
            .GetAllReadOnly()
            .Include(timesheet => timesheet.AssignedJob)
            .Where(timesheet => timesheet.EmployerId == request.Request.EmployerId
                            && timesheet.Date.Date >= request.Request.From.Date
                            && timesheet.Date.Date <= request.Request.To.Date
                            && timesheet.AssignedJob.JobStatus == (byte)JobStatusEnum.Completed)
            .SumAsync(timesheet => timesheet.TotalAmount);
        if (timesheetAmount != request.Request.Amount)
        {
            return Result<bool>.Fail("Invalid amount.");
        }

        var invoice = new Invoice()
        {
            EmployerId = request.Request.EmployerId,
            Date = request.Request.Date,
            From = request.Request.From,
            To = request.Request.To,
            Amount = request.Request.Amount,
            Remarks = request.Request.Remarks
        };

        await _unitOfWork.InvoiceRepository.Add(invoice);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Invoice added successfully.");
    }
}

public sealed class GetNextInvoiceNumberHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetNextInvoiceNumberQuery, Result<GetNextInvoiceNumberResponse>>
{
    public async Task<Result<GetNextInvoiceNumberResponse>> Handle(GetNextInvoiceNumberQuery request, CancellationToken cancellationToken)
    {
        // get next invoice number.
        var invoiceNumber = await _unitOfWork
            .InvoiceRepository
            .GetAllReadOnly()
            .MaxAsync(invoice => invoice.Id) + 1;
        var result = new GetNextInvoiceNumberResponse()
        {
            NextInvoiceNumber = invoiceNumber
        };

        return Result<GetNextInvoiceNumberResponse>.Success(result, "Next invoice number collected successfully.");
    }
}

public sealed class GetEmployeeProfileAsPdfHandler(IUnitOfWork _unitOfWork, IHostingEnvironment _hostingEnvironment, IGeneratePdf _generatePdf)
    : IRequestHandler<GetEmployeeProfileAsPdfQuery, Result<string>>
{
    public async Task<Result<string>> Handle(GetEmployeeProfileAsPdfQuery request, CancellationToken cancellationToken)
    {
        // check employee is exist.
        var employee = await _unitOfWork
            .EmployeeRepository
            .GetAllReadOnly()
                .Include(employee => employee.EmployeeType)
            .Select(employee => new EmployeeDto()
            {
                Id = employee.Id,
                ProfileImageUrl = employee.ProfileImageUrl,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                EmployeeTypeName = employee.EmployeeType.Name,
                EmployeeTypeId = employee.EmployeeTypeId,
                YearsOfExperience = employee.YearsOfExperience ?? 0,
                NMCPin = employee.NMCPin,
                NMCPinStatus = employee.NMCPinStatus,
                DbsNumber = employee.DbsNumber,
                DbsNumberStatus = employee.DbsNumberStatus,
                VaccinationCertificateUrl = employee.VaccinationCertificateUrl,
            })
            .FirstOrDefaultAsync(employee => employee.Id == request.EmployeeId, cancellationToken);
        if (employee is null)
        {
            return Result<string>.Fail("Employee isn't exist.");
        }

        // get total worked hours.
        var jobs = await _unitOfWork
                .AssignedJobRepository
                .GetAllReadOnly()
                    .Include(assignedJob => assignedJob.Job)
                .Where(assignedJob => assignedJob.EmployeeId == request.EmployeeId)
                .ToListAsync(cancellationToken);
        employee.TotalHours = jobs.Where(assignedJob => assignedJob.JobStatus == (byte)JobStatusEnum.Completed).Select(job => job.Job.JobHoursPerDay).Sum();

        // get logos.
        // Create a list to hold the image objects
        var webRootPath = $"{_hostingEnvironment.ContentRootPath}/wwwroot/";
        string verifiedUrl = webRootPath + "/Verified/verifiedImage.png";
        string notverifiedUrl = webRootPath + "/NotVerified/notVerified.png";
        // Convert image to byte array  
        byte[] byteDataVerified = System.IO.File.ReadAllBytes(verifiedUrl);
        byte[] byteDataNotVerified = System.IO.File.ReadAllBytes(notverifiedUrl);
        //Convert byte arry to base64string
        string imgBase64DataVerified = Convert.ToBase64String(byteDataVerified);
        string imgBase64DataNotVerified = Convert.ToBase64String(byteDataNotVerified);
        employee.VerifiedLogo = string.Format("data:image/jpeg;base64,{0}", imgBase64DataVerified);
        employee.NotVerifiedLogo = string.Format("data:image/jpeg;base64,{0}", imgBase64DataNotVerified);

        // get personal reference.
        employee.PersonalReference = await _unitOfWork.ReferenceRepository.GetAllReadOnly().Where(reference => reference.EmployeeId == request.EmployeeId && reference.ReferenceTypeId == (byte)ReferenceTypeEnum.Personal).ToListAsync(cancellationToken);
        employee.ProfessionalReference = await _unitOfWork.ReferenceRepository.GetAllReadOnly().Where(reference => reference.EmployeeId == request.EmployeeId && reference.ReferenceTypeId == (byte)ReferenceTypeEnum.Professional).ToListAsync(cancellationToken);

        // get employements
        employee.Employements = await _unitOfWork.EmployementRepository.GetAllReadOnly().Where(employement => employement.EmployeeId == request.EmployeeId).ToListAsync(cancellationToken);


        var employeeInvoiceList = await _generatePdf.GetByteArray("Template/EmployeeProfilePdf.cshtml", employee);

        string foldername = "EmployeeProfiles";
        string fileNameCombo = $"{employee.FirstName}_{employee.LastName}_profile_with_certificates.pdf";
        if (employeeInvoiceList is not null)
        {
            string file = $"{employee.FirstName} {employee.LastName}".ToLower().Replace(" ", "_") + '_' + "profile.pdf";

            List<string> paths = new List<string>();
            // directory.
            string newPath = Path.Combine(webRootPath, foldername);
            if (!Directory.Exists(newPath))
            {
                System.IO.Directory.CreateDirectory(newPath);
            }
            string filePath = Path.Combine(newPath, file);
            paths.Add(filePath);

            byte[] fileBytes = employeeInvoiceList;
            System.IO.File.WriteAllBytes(filePath, fileBytes);
            // new pdf file name.
            // image to pdf temp files.
            string newProfileImageUrl = "";
            string newDbsCertificateUrl = "";
            string newPassportUrl = "";
            string newBiometricResidenceCardUrl = "";

            // generate pdf for profile.
            if (!string.IsNullOrEmpty(employee.ProfileImageUrl))
            {
                Aspose.Imaging.Image image = Aspose.Imaging.Image.Load(Path.Combine(webRootPath, employee.ProfileImageUrl));
                string fileName = employee.ProfileImageUrl.Replace("Profiles", "").Split(".")[0];
                newProfileImageUrl = Path.Combine(newPath, fileName + ".pdf");

                // Check if the image was loaded successfully.
                if (image != null)
                {
                    // Save the image as a PDF.
                    image.Save(newProfileImageUrl, new Aspose.Imaging.ImageOptions.PdfOptions());

                    // Dispose of the image object to release resources.
                    image.Dispose();

                    paths.Add(Path.Combine(webRootPath, newProfileImageUrl));
                }
            }

            // generate pdf for Dbs.
            if (!string.IsNullOrEmpty(employee.DbsCertificateUrl))
            {
                Aspose.Imaging.Image image = Aspose.Imaging.Image.Load(Path.Combine(webRootPath, employee.DbsCertificateUrl));
                string fileName = employee.DbsCertificateUrl.Replace("DbsCertificates", "").Split(".")[0];
                newDbsCertificateUrl = Path.Combine(newPath, fileName + ".pdf");

                // Check if the image was loaded successfully.
                if (image != null)
                {
                    // Save the image as a PDF.
                    image.Save(newDbsCertificateUrl, new Aspose.Imaging.ImageOptions.PdfOptions());

                    // Dispose of the image object to release resources.
                    image.Dispose();

                    paths.Add(Path.Combine(webRootPath, newDbsCertificateUrl));
                }
            }

            // generate pdf for passport.
            if (!string.IsNullOrEmpty(employee.PassportUrl))
            {
                Aspose.Imaging.Image image = Aspose.Imaging.Image.Load(Path.Combine(webRootPath, employee.PassportUrl));
                string fileName = employee.PassportUrl.Replace("Passports", "").Split(".")[0];
                newPassportUrl = Path.Combine(newPath, fileName + ".pdf");

                // Check if the image was loaded successfully.
                if (image != null)
                {
                    // Save the image as a PDF.
                    image.Save(newPassportUrl, new Aspose.Imaging.ImageOptions.PdfOptions());

                    // Dispose of the image object to release resources.
                    image.Dispose();

                    paths.Add(Path.Combine(webRootPath, newPassportUrl));
                }
            }

            // generate pdf for biometric.
            if (!string.IsNullOrEmpty(employee.BiometricResidenceCardUrl))
            {
                Aspose.Imaging.Image image = Aspose.Imaging.Image.Load(Path.Combine(webRootPath, employee.BiometricResidenceCardUrl));
                string fileName = employee.BiometricResidenceCardUrl.Replace("Biometric Residence Cards", "").Split(".")[0];
                newBiometricResidenceCardUrl = Path.Combine(newPath, fileName + ".pdf");

                // Check if the image was loaded successfully.
                if (image != null)
                {
                    // Save the image as a PDF.
                    image.Save(newBiometricResidenceCardUrl, new Aspose.Imaging.ImageOptions.PdfOptions());

                    // Dispose of the image object to release resources.
                    image.Dispose();

                    paths.Add(Path.Combine(webRootPath, newBiometricResidenceCardUrl));
                }
            }

            // merge all the pdfs.
            MargeMultiplePDF(paths, Path.Combine(newPath, fileNameCombo));

            // Deleted temp files 
            if (File.Exists(newProfileImageUrl))
            {
                File.Delete(newProfileImageUrl);
            }
            if (File.Exists(newDbsCertificateUrl))
            {
                File.Delete(newDbsCertificateUrl);
            }
            if (File.Exists(newPassportUrl))
            {
                File.Delete(newPassportUrl);
            }
            if (File.Exists(newBiometricResidenceCardUrl))
            {
                File.Delete(newBiometricResidenceCardUrl);
            }
        }

        var result = foldername + @"\" + fileNameCombo;
        return Result<string>.Success(result, "Pdf generated successfully.");
    }

    public static void MargeMultiplePDF(List<string> PDFfileNames, string OutputFile)
    {
        PdfReader reader = null;
        iTextSharp.text.Document sourceDocument = null;
        PdfCopy pdfCopyProvider = null;
        PdfImportedPage importedPage;
        string outputPdfPath = OutputFile;


        using (var fileStream = new System.IO.FileStream(outputPdfPath, System.IO.FileMode.Create))
        {
            sourceDocument = new iTextSharp.text.Document();
            pdfCopyProvider = new PdfCopy(sourceDocument, fileStream);

            // Output file Open  
            sourceDocument.Open();

            // Files list wise Loop  
            foreach (var item in PDFfileNames)
            {
                int pages = TotalPageCount(item);

                // Skip processing if TotalPageCount returns 0
                if (pages == 0)
                {
                    Console.WriteLine("Skipping file: " + item + " - It has no pages.");
                    continue;
                }

                reader = new PdfReader(item);
                // Add pages in new file  
                for (int i = 1; i <= pages; i++)
                {
                    importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                    pdfCopyProvider.AddPage(importedPage);
                }

                reader.Close();
            }

            // Save the output file  
            sourceDocument.Close();
        }
    }
    private static int TotalPageCount(string file)
    {
        using (StreamReader sr = new StreamReader(System.IO.File.OpenRead(file)))
        {
            Regex regex = new Regex(@"/Type\s*/Page[^s]");
            MatchCollection matches = regex.Matches(sr.ReadToEnd());

            return matches.Count;
        }
    }
}

public sealed class UpdateEmployeeDetailsHandler(IUnitOfWork _unitOfWork) : IRequestHandler<UpdateEmployeeDetailsCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateEmployeeDetailsCommand request, CancellationToken cancellationToken)
    {
        // check employer exists.
        var employee = await _unitOfWork
            .EmployeeRepository
            .GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee isn't exist.");
        }

        if (employee.HaveDbsNumber)
        {
            if (string.IsNullOrEmpty(request.Request.DbsNumber))
            {
                return Result<bool>.Fail("DbsNumber is required.");
            }
            employee.DbsNumber = request.Request.DbsNumber;
        }
        else
        {
            if (string.IsNullOrEmpty(request.Request.NationalInsuranceNumber))
            {
                return Result<bool>.Fail("NationalInsuranceNumber is required.");
            }
            employee.NationalInsuranceNumber = request.Request.NationalInsuranceNumber;
        }

        employee.FirstName = request.Request.FirstName;
        employee.LastName = request.Request.LastName;
        employee.EmployementTypeId = request.Request.EmployementTypeId;
        employee.EmployeeTypeId = request.Request.EmployeeTypeId;
        employee.Address = request.Request.Address;
        employee.Address2 = request.Request.Address2;
        employee.City = request.Request.City;
        employee.PinCode = request.Request.PinCode;
        employee.DateOfBirth = request.Request.DateOfBirth;
        employee.Nationality = request.Request.Nationality;
        employee.UTRNumber = request.Request.UTRNumber;
        employee.Latitude = request.Request.Latitude;
        employee.Longitude = request.Request.Longitude;

        _unitOfWork.EmployeeRepository.Change(employee);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Employee updated successfully.");
    }
}

public sealed class GetAPIsForJobHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetAPIsForJobQuery, Result<GetAPIsForJobResponce>>
{
    public async Task<Result<GetAPIsForJobResponce>> Handle(GetAPIsForJobQuery request, CancellationToken cancellationToken)
    {
        // check booking exists.
        var booking = await _unitOfWork
            .BookingRepository
            .GetDetailsForJobById(request.BookingId)
            .FirstOrDefaultAsync(cancellationToken);
        if (booking is null)
        {
            return Result<GetAPIsForJobResponce>.Fail("Booking isn't exist.");
        }

        // get employee types.
        booking.EmployeeTypes = await _unitOfWork
            .EmployeeTypeRepository
            .GetAllReadOnly()
            .Select(type => new EmployeeTypeDto()
            {
                Id = type.Id,
                Name = type.Name,
                Description = type.Description,
                MinRate = type.MinRate
            }).ToListAsync(cancellationToken);

        // get shifts.
        booking.Shifts = await _unitOfWork
            .ShiftRepository
            .GetAllReadOnly()
            .Where(shift => shift.EmployerId == booking.EmployerId)
            .Select(shift => new ShiftDto()
            {
                Id = shift.Id,
                Name = shift.Name,
                StartTime = DateTime.Now.Date.Add(shift.StartTime.ToTimeSpan()),
                EndTime = DateTime.Now.Date.Add(shift.EndTime.ToTimeSpan())
            }).ToListAsync(cancellationToken);

        // get job types from enum.
        booking.JobTypes = Enum.GetValues(typeof(JobTypeEnum))
                           .Cast<JobTypeEnum>()
                           .Where(type => type != JobTypeEnum.UnlistingJob)
                           .Select(e => new JobTypeDto() { Id = (byte)e, Name = GetEnumDescription(e) })
                           .ToList();

        // get employee categories from enum.
        booking.EmployeeCategories = Enum.GetValues(typeof(EmployeeCategoryEnum))
                           .Cast<EmployeeCategoryEnum>()
                           .Select(e => new EmployeeCategoryDto() { Id = (byte)e, Name = GetEnumDescription(e) })
                           .ToList();

        // get break time from enum.
        booking.BreakTime = Enum.GetValues(typeof(BreakTimeEnum))
                           .Cast<BreakTimeEnum>()
                           .Select(e => (byte)e)
                           .ToList();

        return Result<GetAPIsForJobResponce>.Success(booking, "Data collected successfully.");
    }
    static string GetEnumDescription(Enum value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());
        var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));

        return attribute == null ? value.ToString() : attribute.Description;
    }
}

public sealed class GetEmployeesByEmployeeTypeIdAndEmployeeCategoryIdHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetEmployeesByEmployeeTypeIdAndEmployeeCategoryIdQuery, Result<List<GetEmployeesByEmployeeTypeIdAndEmployeeCategoryIdResponce>>>
{
    public async Task<Result<List<GetEmployeesByEmployeeTypeIdAndEmployeeCategoryIdResponce>>> Handle(GetEmployeesByEmployeeTypeIdAndEmployeeCategoryIdQuery request, CancellationToken cancellationToken)
    {
        // check is status code valid.
        var isExist = Enum.IsDefined(typeof(EmployeeCategoryEnum), request.EmployeeCategoryId);
        if (!isExist)
            return Result<List<GetEmployeesByEmployeeTypeIdAndEmployeeCategoryIdResponce>>.Fail("Invalid status code.");

        // Get employee by type.
        var query = _unitOfWork.EmployeeRepository.GetEmployeesByEmployeeTypeId(request.EmployeeTypeId);
        switch (request.EmployeeCategoryId)
        {
            case (byte)EmployeeCategoryEnum.OutOfPortalEmployee: query = query.Where(employee => employee.AccountStatus == (byte)EmployeeAccountStatusEnum.OutOfPortal); break;
            case (byte)EmployeeCategoryEnum.ESGOEmployee: query = query.Where(timesheet => timesheet.AccountStatus == (byte)EmployeeAccountStatusEnum.Activated); break;
        }

        var result = await query.Select(employee => new GetEmployeesByEmployeeTypeIdAndEmployeeCategoryIdResponce
        {
            Id = employee.Id,
            Name = employee.FullName,
        }).ToListAsync(cancellationToken);

        return Result<List<GetEmployeesByEmployeeTypeIdAndEmployeeCategoryIdResponce>>.Success(result, "Employees collected successfully.");
    }
}

public sealed class GetTimesheetsByStatusForAdminHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetTimesheetsByStatusForAdminQuery, Result<PaginationModel<GetTimesheetsByStatusForAdminResponce>>>
{
    public async Task<Result<PaginationModel<GetTimesheetsByStatusForAdminResponce>>> Handle(GetTimesheetsByStatusForAdminQuery request, CancellationToken cancellationToken)
    {
        // check nurse type exists.
        var isExist = Enum.IsDefined(typeof(TimeSheetStatusEnum), request.Status);
        if (!isExist)
            return Result<PaginationModel<GetTimesheetsByStatusForAdminResponce>>.Fail("Invalid status type.");

        // get timesheets by status
        var timesheets = _unitOfWork.TimesheetRepository.GetTimesheetsByStatus(request.Status);
        var query = from timesheet in timesheets
                    join shift in _unitOfWork.ShiftRepository.GetAllReadOnly()
                    on timesheet.ShiftId equals shift.Id into Details
                    from m in Details.DefaultIfEmpty()
                    select new GetTimesheetsByStatusForAdminResponce
                    {
                        Id = timesheet.Id,
                        EmployerId = timesheet.EmployerId,
                        JobId = timesheet.JobId,
                        EmployeeId = timesheet.EmployeeId,
                        ShiftId = timesheet.ShiftId,
                        EmployeeType = timesheet.EmployeeType,
                        BillableHours = timesheet.BillableHours,
                        Date = timesheet.Date,
                        StartTime = timesheet.StartTime,
                        EndTime = timesheet.EndTime,
                        EmployeeName = timesheet.EmployeeName,
                        EmployerName = timesheet.EmployerName,
                        BreakTime = timesheet.BreakTime,
                        TotalHours = timesheet.TotalHours,
                        HourlyRate = timesheet.HourlyRate,
                        ReviewedBy = timesheet.ReviewedBy,
                        Notes = timesheet.Notes,
                        Status = timesheet.Status,
                        ShiftDescription = timesheet.ShiftId == 0 ? "Custom" : m.Name
                    };
        switch (request.Status)
        {
            case (byte)TimeSheetStatusEnum.Pending: query = query.Where(x => x.Status == (byte)TimeSheetStatusEnum.Pending); break;
            case (byte)TimeSheetStatusEnum.Approved: query = query.Where(x => x.Status == (byte)TimeSheetStatusEnum.Approved); break;
            case (byte)TimeSheetStatusEnum.Rejected: query = query.Where(x => x.Status == (byte)TimeSheetStatusEnum.Rejected); break;
        }

        // Add pagination
        PaginationModel<GetTimesheetsByStatusForAdminResponce> model = new PaginationModel<GetTimesheetsByStatusForAdminResponce>();
        if (request.PageSize > 0)
        {
            var result = new PagedList<GetTimesheetsByStatusForAdminResponce>(query, request.PageIndex, request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }

        return Result<PaginationModel<GetTimesheetsByStatusForAdminResponce>>.Success(model, "Timesheet list.");
    }
}

public sealed class GetTimesheetByIdForAdminHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetTimesheetByIdForAdminQuery, Result<GetTimesheetByIdForAdminResponce>>
{
    public async Task<Result<GetTimesheetByIdForAdminResponce>> Handle(GetTimesheetByIdForAdminQuery request, CancellationToken cancellationToken)
    {
        // get timesheet by id.
        var timesheet = await _unitOfWork.TimesheetRepository.GetAllReadOnly()
            .Include(timesheet => timesheet.Employee)
            .Include(timesheet => timesheet.Employer)
            .Include(timesheet => timesheet.AssignedJob)
            .Select(timesheet => new GetTimesheetByIdForAdminResponce()
            {
                Id = timesheet.Id,
                BreakTime = timesheet.BreakTime,
                OrganisationName = timesheet.Employer.CompanyName,
                EmployeeName = $"{timesheet.Employee.FirstName} {timesheet.Employee.LastName}",
                PhoneNumber = timesheet.Employee.PhoneNumber,
                TimesheetDate = timesheet.Date.Date,
                JobDate = timesheet.AssignedJob.CreatedDate.Date,
                StartTime = timesheet.Date.Date.Add(timesheet.StartTime.ToTimeSpan()),
                EndTime = timesheet.Date.Date.Add(timesheet.EndTime.ToTimeSpan()),
                TotalAmount = timesheet.TotalAmount,
                TotalHours = timesheet.TotalHours,
                HourlyRate = timesheet.HourlyRate,
                Status = timesheet.Status,
                ApprovalDate = timesheet.ApprovalDate.HasValue ? timesheet.ApprovalDate.Value.Date : null,
                RejectionDate = timesheet.RejectionDate.HasValue ? timesheet.RejectionDate.Value.Date : null,
                CreatedDate = timesheet.CreatedDate.Date,
                BillableHours = timesheet.BillableHours,
                Reason = timesheet.Reason
            })
            .FirstOrDefaultAsync(timesheet => timesheet.Id == request.TimesheetId);
        if (timesheet is null)
        {
            return Result<GetTimesheetByIdForAdminResponce>.Fail("Timesheet is not exist.");
        }

        return Result<GetTimesheetByIdForAdminResponce>.Success(timesheet, "Timesheet collected.");
    }
}

public sealed class GetEmployeesByJobIdHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetEmployeesByJobIdQuery, Result<List<GetEmployeesByJobIdResponce>>>
{
    public async Task<Result<List<GetEmployeesByJobIdResponce>>> Handle(GetEmployeesByJobIdQuery request, CancellationToken cancellationToken)
    {
        // get employee by job id.
        var employees = await _unitOfWork
            .AssignedJobRepository
            .GetAllReadOnly()
                .Include(assignedJob => assignedJob.Employee)
            .Where(assignedJob => assignedJob.JobId == request.JobId)
            .GroupBy(assignedJob => assignedJob.EmployeeId)
            .Select(assignedJob => new GetEmployeesByJobIdResponce()
            {
                AssignedJobId = assignedJob.Select(x => x.Id).FirstOrDefault(),
                JobId = assignedJob.Select(x => x.JobId).FirstOrDefault(),
                EmployeeId = assignedJob.Select(x => x.EmployeeId).FirstOrDefault(),
                EmployeeName = assignedJob.Select(x => $"{x.Employee.FirstName} {x.Employee.LastName}").First(),
                AppliedDate = assignedJob.Select(x => x.AppliedDate.Date).First(),
                ConfirmationDate = assignedJob.Select(x => x.ConfirmationDate.Date).First(),
                Status = assignedJob.Select(x => x.JobStatus).First()
            })
            .ToListAsync(cancellationToken);

        return Result<List<GetEmployeesByJobIdResponce>>.Success(employees, "Employees collected.");
    }
}

public sealed class GetEmployeeByAssignedJobIdHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetEmployeeByAssignedJobIdQuery, Result<GetEmployeeByAssignedJobIdResponce>>
{
    public async Task<Result<GetEmployeeByAssignedJobIdResponce>> Handle(GetEmployeeByAssignedJobIdQuery request, CancellationToken cancellationToken)
    {
        // get employee by job id.
        var employee = await _unitOfWork
            .AssignedJobRepository
            .GetAllReadOnly()
                .Include(assignedJob => assignedJob.Employee)
            .Where(assignedJob => assignedJob.Id == request.AssignedJobId)
            .Select(assignedJob => new GetEmployeeByAssignedJobIdResponce()
            {
                AssignedJobId = assignedJob.Id,
                JobId = assignedJob.JobId,
                EmployeeName = $"{assignedJob.Employee.FirstName} {assignedJob.Employee.LastName}",
                PhoneNumber = assignedJob.Employee.PhoneNumber,
                Email = assignedJob.Employee.Email,
                Address = assignedJob.Employee.Address,
                City = assignedJob.Employee.City,
                AppliedDate = assignedJob.AppliedDate.Date,
                JobDate = assignedJob.Job.Date.Date,
            })
            .FirstOrDefaultAsync(cancellationToken);
        if (employee is null)
        {
            return Result<GetEmployeeByAssignedJobIdResponce>.Fail("Employee isn't exist.");
        }

        return Result<GetEmployeeByAssignedJobIdResponce>.Success(employee, "Employee collected.");
    }
}

public sealed class GetEmployeeJobDetailsByIdHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetEmployeeJobDetailsByIdQuery, Result<GetEmployeeJobDetailsByIdResponce>>
{
    public async Task<Result<GetEmployeeJobDetailsByIdResponce>> Handle(GetEmployeeJobDetailsByIdQuery request, CancellationToken cancellationToken)
    {
        // get job id.
        var assignedJob = _unitOfWork
            .AssignedJobRepository
            .GetAllReadOnly()
                .Include(assignedJob => assignedJob.Job)
                .Include(assignedJob => assignedJob.Employer)
            .Where(assignedJob => assignedJob.Id == request.AssignedJobId)
            .Select(assignedJob => new GetEmployeeJobDetailsByIdResponce()
            {
                AssignedJobId = assignedJob.Id,
                JobDate = assignedJob.Job.Date.Date,
                OrganisationName = assignedJob.Employer.CompanyName,
                HourlyRate = assignedJob.Job.HourlyRate,
                CompletionDate = assignedJob.CompletionDate.Date,
                ConfirmationDate = assignedJob.ConfirmationDate.Date,
                SelectionDate = assignedJob.ConfirmationDate.Date,
                ShiftId = assignedJob.Job.ShiftId,
            });
        var query = from job in assignedJob
                    join timesheet in _unitOfWork.TimesheetRepository.GetAllReadOnly()
                    on job.AssignedJobId equals timesheet.AssignedJobId into Details
                    from m in Details.DefaultIfEmpty()
                    select new GetEmployeeJobDetailsByIdResponce
                    {
                        AssignedJobId = job.AssignedJobId,
                        JobDate = job.JobDate,
                        OrganisationName = job.OrganisationName,
                        HourlyRate = job.HourlyRate,
                        ShiftId = job.ShiftId,
                        CompletionDate = job.CompletionDate,
                        ConfirmationDate = job.ConfirmationDate,
                        SelectionDate = job.SelectionDate,
                        TimesheetDate = m != null ? m.Date.Date : null
                    };
        var result = await query.FirstOrDefaultAsync(cancellationToken);
        if (result is null)
        {
            return Result<GetEmployeeJobDetailsByIdResponce>.Fail("Data isn't exist.");
        }

        return Result<GetEmployeeJobDetailsByIdResponce>.Success(result, "Employee collected.");
    }
}

public sealed class GetDashboardDetailsHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetDashboardDetailsQuery, Result<GetDashboardDetailsResponce>>
{
    public async Task<Result<GetDashboardDetailsResponce>> Handle(GetDashboardDetailsQuery request, CancellationToken cancellationToken)
    {
        GetDashboardDetailsResponce result = new();

        // get employees.
        var employees = await _unitOfWork.EmployeeRepository.GetAllReadOnly()
            .GroupBy(employee => employee.AccountStatus)
            .Select(employee => new
            {
                Status = employee.Key,
                Total = employee.Count()
            })
            .ToListAsync(cancellationToken);
        result.TotalEmployees = employees.Sum(employee => employee.Total);
        result.TotalActiveEmployees = employees.Where(employee => employee.Status == (byte)EmployeeAccountStatusEnum.Activated).Sum(employee => employee.Total);
        result.TotalInActiveEmployees = employees.Where(employee => employee.Status == (byte)EmployeeAccountStatusEnum.InActivated).Sum(employee => employee.Total);
        result.TotalPendingEmployees = employees.Where(employee => employee.Status == (byte)EmployeeAccountStatusEnum.Pending).Sum(employee => employee.Total);

        // get employers.
        var employers = await _unitOfWork.EmployerRepository.GetAllReadOnly()
            .GroupBy(employer => employer.AccountStatus)
            .Select(employer => new
            {
                Status = employer.Key,
                Total = employer.Count()
            })
            .ToListAsync(cancellationToken);
        result.TotalEmployers = employers.Sum(employer => employer.Total);
        result.TotalActiveEmployers = employers.Where(employer => employer.Status == (byte)EmployerAccountStatusEnum.Active).Sum(employer => employer.Total);
        result.TotalInActiveEmployers = employers.Where(employer => employer.Status == (byte)EmployerAccountStatusEnum.InActive).Sum(employer => employer.Total);
        result.TotalPendingEmployers = employers.Where(employer => employer.Status == (byte)EmployerAccountStatusEnum.Pending).Sum(employer => employer.Total);

        // get employement types.
        result.EmployementTypes = await _unitOfWork
            .EmployeeRepository
            .GetAllReadOnly()
                .Include(employee => employee.EmployementType)
            .GroupBy(employee => employee.EmployementTypeId)
            .Select(employee => new EmployementTypeForDashboard
            {
                EmployementTypeId = employee.Key,
                Name = employee.Select(x => x.EmployementType.Name).FirstOrDefault(),
                Total = employee.Count()
            })
            .ToListAsync(cancellationToken);

        // get employee types.
        result.EmployeeTypes = await _unitOfWork
            .EmployeeRepository
            .GetAllReadOnly()
                .Include(employee => employee.EmployeeType)
            .GroupBy(employee => employee.EmployeeTypeId)
            .Select(employee => new EmployeeTypeForDashboard
            {
                EmployeeTypeId = employee.Key,
                Name = employee.Select(x => x.EmployeeType.Name).FirstOrDefault(),
                Total = employee.Count()
            })
            .ToListAsync(cancellationToken);

        // getting top 3 records of each status.
        var assignedJobs = await _unitOfWork
            .AssignedJobRepository
            .GetAllReadOnly()
                .Include(assignedJob => assignedJob.Job)
                .Include(assignedJob => assignedJob.Employer)
            .Select(assignedJob => new JobsForDashboard
            {
                AssignedJobId = assignedJob.Id,
                Date = assignedJob.Job.Date.Date,
                EmployerName = assignedJob.Employer.Name,
                HourlyRate = assignedJob.Job.HourlyRate,
                StartTime = assignedJob.Job.Date.Date.Add(assignedJob.Job.ShiftStartTime.ToTimeSpan()),
                EndTime = assignedJob.Job.Date.Date.Add(assignedJob.Job.ShiftEndTime.ToTimeSpan()),
                ShiftId = assignedJob.Job.ShiftId,
                Status = assignedJob.JobStatus
            })
            .GroupBy(assignedJob => assignedJob.Status)
                .Select(group => group.OrderByDescending(e => e.AssignedJobId).Take(3))
            .ToListAsync(cancellationToken);

        // get timesheets by status
        var query = from job in assignedJobs.SelectMany(assignedJob => assignedJob)
                    join shift in _unitOfWork.ShiftRepository.GetAllReadOnly()
                    on job.ShiftId equals shift.Id into Details
                    from m in Details.DefaultIfEmpty()
                    select new JobsForDashboard
                    {
                        AssignedJobId = job.AssignedJobId,
                        Date = job.Date,
                        EmployerName = job.EmployerName,
                        HourlyRate = job.HourlyRate,
                        ShiftId = job.ShiftId,
                        StartTime = job.StartTime,
                        EndTime = job.EndTime,
                        Status = job.Status,
                        ShiftDescription = job.ShiftId == 0 ? "Custom" : m.Name
                    };
        result.JobsForDashboard = query.ToList();
        return Result<GetDashboardDetailsResponce>.Success(result, "Data collected successfully.");
    }
}

public sealed class GetJobHistoryByEmployeeIdHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetJobHistoryByEmployeeIdQuery, Result<PaginationModel<GetJobHistoryByEmployeeIdResponce>>>
{
    public async Task<Result<PaginationModel<GetJobHistoryByEmployeeIdResponce>>> Handle(GetJobHistoryByEmployeeIdQuery request, CancellationToken cancellationToken)
    {
        // get employee by job id.
        var employees = _unitOfWork
            .AssignedJobRepository
            .GetAllReadOnly()
                .Include(assignedJob => assignedJob.Employer)
                .Include(assignedJob => assignedJob.Job)
            .Where(assignedJob => assignedJob.EmployeeId == request.EmployeeId)
            .Select(assignedJob => new GetJobHistoryByEmployeeIdResponce()
            {
                AssignedJobId = assignedJob.Id,
                Date = assignedJob.Job.Date.Date,
                ShiftId = assignedJob.Job.ShiftId,
                StartTime = assignedJob.Job.Date.Date.Add(assignedJob.Job.ShiftStartTime.ToTimeSpan()),
                EndTime = assignedJob.Job.Date.Date.Add(assignedJob.Job.ShiftEndTime.ToTimeSpan()),
                EmployerName = assignedJob.Employer.Name,
                HourlyRate = assignedJob.Job.HourlyRate,
                Status = assignedJob.JobStatus
            });

        var query = from employee in employees
                    join shift in _unitOfWork.ShiftRepository.GetAllReadOnly()
                    on employee.ShiftId equals shift.Id into Details
                    from m in Details.DefaultIfEmpty()
                    select new GetJobHistoryByEmployeeIdResponce
                    {
                        AssignedJobId = employee.AssignedJobId,
                        Date = employee.Date,
                        ShiftId = employee.ShiftId,
                        StartTime = employee.StartTime,
                        EndTime = employee.EndTime,
                        EmployerName = employee.EmployerName,
                        HourlyRate = employee.HourlyRate,
                        Status = employee.Status,
                        ShiftDescription = employee.ShiftId == 0 ? "Custom" : m.Name
                    };
        // Add pagination
        PaginationModel<GetJobHistoryByEmployeeIdResponce> model = new PaginationModel<GetJobHistoryByEmployeeIdResponce>();
        if (request.PageSize > 0)
        {
            var result = new PagedList<GetJobHistoryByEmployeeIdResponce>(query, request.PageIndex, request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }
        return Result<PaginationModel<GetJobHistoryByEmployeeIdResponce>>.Success(model, "Employee collected.");
    }
}

public sealed class GetEmployerJobsHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetEmployerJobsQuery, Result<PaginationModel<GetEmployerJobsResponce>>>
{
    public async Task<Result<PaginationModel<GetEmployerJobsResponce>>> Handle(GetEmployerJobsQuery request, CancellationToken cancellationToken)
    {
        // get employee by job id.
        var employees = _unitOfWork
            .AssignedJobRepository
            .GetAllReadOnly()
                .Include(assignedJob => assignedJob.Employer)
                .Include(assignedJob => assignedJob.Employee)
                    .ThenInclude(employee => employee.EmployeeType)
                .Include(assignedJob => assignedJob.Job)
            .Where(assignedJob => assignedJob.EmployerId == request.EmployerId)
            .Select(assignedJob => new GetEmployerJobsResponce()
            {
                AssignedJobId = assignedJob.Id,
                Date = assignedJob.Job.Date.Date,
                ShiftId = assignedJob.Job.ShiftId,
                StartTime = assignedJob.Job.Date.Date.Add(assignedJob.Job.ShiftStartTime.ToTimeSpan()),
                EndTime = assignedJob.Job.Date.Date.Add(assignedJob.Job.ShiftEndTime.ToTimeSpan()),
                EmployerName = assignedJob.Employer.Name,
                HourlyRate = assignedJob.Job.HourlyRate,
                Status = assignedJob.JobStatus,
                SelectedSource = 1,
                Count = 1,
                EmployeeType = assignedJob.Employee.EmployeeType.Name
            });

        employees = request.TabName switch
        {
            "today" => employees.Where(employee => employee.Date == DateTime.UtcNow.Date && employee.Status == (byte)JobStatusEnum.Open),
            "upcomming" => employees.Where(employee => employee.Date >= DateTime.UtcNow.Date && employee.Status == (byte)JobStatusEnum.Open),
            "completed" => employees.Where(employee => employee.Status == (byte)JobStatusEnum.Completed),
            "cancelled" => employees.Where(employee => employee.Status == (byte)JobStatusEnum.Cancelled),
            _ => employees,
        };

        var query = from employee in employees
                    join shift in _unitOfWork.ShiftRepository.GetAllReadOnly()
                    on employee.ShiftId equals shift.Id into Details
                    from m in Details.DefaultIfEmpty()
                    select new GetEmployerJobsResponce
                    {
                        AssignedJobId = employee.AssignedJobId,
                        Date = employee.Date,
                        ShiftId = employee.ShiftId,
                        StartTime = employee.StartTime,
                        EndTime = employee.EndTime,
                        EmployerName = employee.EmployerName,
                        HourlyRate = employee.HourlyRate,
                        Status = employee.Status,
                        ShiftDescription = employee.ShiftId == 0 ? "Custom" : m.Name
                    };
        // Add pagination
        PaginationModel<GetEmployerJobsResponce> model = new PaginationModel<GetEmployerJobsResponce>();
        if (request.PageSize > 0)
        {
            var result = new PagedList<GetEmployerJobsResponce>(query, request.PageIndex, request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }
        return Result<PaginationModel<GetEmployerJobsResponce>>.Success(model, "Employee collected.");
    }
}

public sealed class EmployeeLoginCommandHandlers(UserManager<User> _userManger, IConfiguration _configuration, IUnitOfWork _unitOfWork)
    : IRequestHandler<EmployeeLoginCommand, Result<EmployeeLoginResponse>>
{
    public async Task<Result<EmployeeLoginResponse>> Handle(EmployeeLoginCommand request, CancellationToken cancellationToken)
    {
        // login with email.
        var user = await _userManger.FindByEmailAsync(request.Request.Email);
        if (user is null)
        {
            return Result<EmployeeLoginResponse>.Fail("There is no user.");
        }

        // login by admin.
        var adminEmail = _configuration["SystemadminUser:Email"] ?? string.Empty;
        if (adminEmail is null)
        {
            return Result<EmployeeLoginResponse>.Fail("Admin email is not exist.");
        }

        var admin = await _userManger.FindByEmailAsync(adminEmail);
        if (admin is null)
        {
            return Result<EmployeeLoginResponse>.Fail("There is no admin found.");
        }

        // check is password correct.
        var result = await _userManger.CheckPasswordAsync(admin, request.Request.Password);
        if (!result)
        {
            return Result<EmployeeLoginResponse>.Fail("Invalid credentials.");
        }

        // is employee exists.
        var employee = await _unitOfWork
            .EmployeeRepository
            .GetAll()
            .FirstOrDefaultAsync(x => x.UserId.Equals(new Guid(user.Id)), cancellationToken);
        if (employee is null)
        {
            return Result<EmployeeLoginResponse>.Fail("There is no user.");
        }

        // add claims in jwt token.
        var claims = new[] {
                new Claim("Email", request.Request.Email),
                new Claim("EmployeeId", employee.Id.ToString()),
                new Claim("role", nameof(RoleEnum.Employee)),
                new Claim("role", nameof(RoleEnum.SuperAdmin)),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));
        var token = new JwtSecurityToken(
            issuer: _configuration["AuthSettings:Issuer"],
            audience: _configuration["AuthSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

        EmployeeLoginResponse response = new EmployeeLoginResponse()
        {
            ExpireDate = token.ValidTo,
            Token = tokenAsString,
            Country = employee.Country ?? string.Empty,
            EmployeeTypeId = employee.EmployeeTypeId,
            EmployementTypeId = employee.EmployementTypeId,
            AccountStatus = employee.AccountStatus
        };

        // return response.
        return Result<EmployeeLoginResponse>.Success(response, "User logged in successfully!");
    }
}

public sealed class EmployerLoginForAdminHandlers(UserManager<User> _userManger, IConfiguration _configuration, IUnitOfWork _unitOfWork)
    : IRequestHandler<EmployerLoginForAdminCommand, Result<EmployerLoginResponse>>
{
    public async Task<Result<EmployerLoginResponse>> Handle(EmployerLoginForAdminCommand request, CancellationToken cancellationToken)
    {
        // login with email.
        var user = await _userManger.FindByEmailAsync(request.Request.Email);
        if (user is null)
        {
            return Result<EmployerLoginResponse>.Fail("There is no user.");
        }

        // login by admin.
        var adminEmail = _configuration["SystemadminUser:Email"] ?? string.Empty;
        if (adminEmail is null)
        {
            return Result<EmployerLoginResponse>.Fail("Admin email is not exist.");
        }

        var admin = await _userManger.FindByEmailAsync(adminEmail);
        if (admin is null)
        {
            return Result<EmployerLoginResponse>.Fail("There is no admin found.");
        }

        // check is password correct.
        var result = await _userManger.CheckPasswordAsync(admin, request.Request.Password);
        if (!result)
        {
            return Result<EmployerLoginResponse>.Fail("Invalid credentials.");
        }

        // is employer exist.
        var employer = await _unitOfWork
            .EmployerRepository
            .GetAllReadOnly()
            .Select(x => new { x.Id, x.UserId, x.CompanyName })
            .FirstOrDefaultAsync(x => x.UserId.Equals(new Guid(user.Id)), cancellationToken);
        if (employer is null)
        {
            return Result<EmployerLoginResponse>.Fail("There is no user.");
        }

        // add claims in jwt token.
        var claims = new[] {
                new Claim("Email", request.Request.Email),
                new Claim("EmployerId", employer.Id.ToString()),
                new Claim("role", nameof(RoleEnum.Employer)),
                new Claim("role", nameof(RoleEnum.SuperAdmin)),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
        var organisationName = employer.CompanyName;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));
        var token = new JwtSecurityToken(
            issuer: _configuration["AuthSettings:Issuer"],
            audience: _configuration["AuthSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

        EmployerLoginResponse response = new EmployerLoginResponse()
        {
            ExpireDate = token.ValidTo,
            Token = tokenAsString,
            Role = nameof(RoleEnum.Employer),
            OrganisationName = organisationName,
            IsAdmin = true
        };

        // return response.
        return Result<EmployerLoginResponse>.Success(response, "User logged in successfully!");
    }
}

public sealed class GetNotificationsHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetNotificationsQuery, Result<GetNotificationsResponce>>
{
    public async Task<Result<GetNotificationsResponce>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
    {
        GetNotificationsResponce result = new ();
        IQueryable<Notification> query = _unitOfWork.NotificationsRepository.GetNotifications();
        if (request.Type == "Unread")
        {
            query = query.Where(x => !x.IsRead);
        }
        query = query.OrderByDescending(x => x.CreatedDate);

        var notifications = await query.Skip(request.Skip).Take(20).ToListAsync(cancellationToken);
        foreach (var notification in notifications)
        {
            DateTime dtNow = DateTime.UtcNow;
            TimeSpan results = dtNow.Subtract(notification.Date);
            int seconds = Convert.ToInt32(results.TotalSeconds);
            notification.TimeAgo = DateTime.Now.AddSeconds(-seconds).Humanize();
            result.Notifications.Add(notification);
        }

        result.TotalUnread = await _unitOfWork.NotificationsRepository.GetAllReadOnly().Where(x => !x.IsRead).CountAsync(cancellationToken);
        return Result<GetNotificationsResponce>.Success(result, "Notifications collected.");
    }
}

public sealed class MarkAsReadNotificationHandler(IUnitOfWork _unitOfWork) : IRequestHandler<MarkAsReadNotificationCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(MarkAsReadNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = await _unitOfWork.NotificationsRepository.GetAll().FirstOrDefaultAsync(x => x.Id == request.NotificationId, cancellationToken);
        if (notification is null)
        {
            return Result<bool>.Fail("notification not found.");
        }

        notification.IsRead = true;
        _unitOfWork.NotificationsRepository.Change(notification);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "successfully changed.");
    }
}

public sealed class MarkAllAsReadNotificationsHandler(IUnitOfWork _unitOfWork) : IRequestHandler<MarkAllAsReadNotificationsCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(MarkAllAsReadNotificationsCommand request, CancellationToken cancellationToken)
    {
        var notifications = _unitOfWork.NotificationsRepository.GetAll()
            .Where(x => !x.IsRead)
                .ExecuteUpdate(setters => setters
                    .SetProperty(b => b.IsRead, true));

        return Result<bool>.Success(true, "successfully changed.");
    }
}
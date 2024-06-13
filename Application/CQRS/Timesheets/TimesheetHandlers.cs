namespace Application.CQRS.Timesheets;

public sealed class GetTimesheetsHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetTimesheetsQuery, Result<PaginationModel<GetTimesheetsResponse>>>
{
    public async Task<Result<PaginationModel<GetTimesheetsResponse>>> Handle(GetTimesheetsQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employer = await _unitOfWork.EmployerRepository.GetById(request.EmployerId);
        if (employer is null)
        {
            return Result<PaginationModel<GetTimesheetsResponse>>.Fail("Employer doesn't exist.");
        }

        var timesheets = _unitOfWork.TimesheetRepository.GetTimesheets(request.EmployerId);

        // Add pagination
        PaginationModel<GetTimesheetsResponse> model = new PaginationModel<GetTimesheetsResponse>();
        if (request.PageSize > 0)
        {
            var result = new PagedList<GetTimesheetsResponse>(timesheets, request.PageIndex, request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }

        return Result<PaginationModel<GetTimesheetsResponse>>.Success(model, "timesheets list.");
    }
}

public sealed class GetTimesheetByJobIdHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetTimesheetByJobIdQuery, Result<GetTimesheetsResponse>>
{
    public async Task<Result<GetTimesheetsResponse>> Handle(GetTimesheetByJobIdQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employer = await _unitOfWork.EmployerRepository.GetById(request.EmployerId);
        if (employer is null)
        {
            return Result<GetTimesheetsResponse>.Fail("Employer doesn't exist.");
        }

        var timesheet = await _unitOfWork
            .TimesheetRepository
            .GetTimesheetByJobId(request.JobId)
            .FirstOrDefaultAsync(cancellationToken);
        if (timesheet is null)
        {
            return Result<GetTimesheetsResponse>.Fail("Timesheet doesn't exist.");
        }

        return Result<GetTimesheetsResponse>.Success(timesheet, "timesheet collected.");
    }
}

public sealed class GetApprovedTimeSheetsByJobIdHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetApprovedTimeSheetsByJobIdQuery, Result<PaginationModel<GetTimesheetsResponse>>>
{
    public async Task<Result<PaginationModel<GetTimesheetsResponse>>> Handle(GetApprovedTimeSheetsByJobIdQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employer = await _unitOfWork.EmployerRepository.GetById(request.EmployerId);
        if (employer is null)
        {
            return Result<PaginationModel<GetTimesheetsResponse>>.Fail("Employer doesn't exist.");
        }

        var timesheets = _unitOfWork.TimesheetRepository.GetApprovedTimeSheetsByJobId(request.JobId);

        // Add pagination
        PaginationModel<GetTimesheetsResponse> model = new PaginationModel<GetTimesheetsResponse>();
        if (request.PageSize > 0)
        {
            var result = new PagedList<GetTimesheetsResponse>(timesheets, request.PageIndex, request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }

        return Result<PaginationModel<GetTimesheetsResponse>>.Success(model, "timesheets list.");
    }
}

public sealed class GetRejectedTimeSheetByJobIdHandler(IUnitOfWork _unitOfWork) 
    : IRequestHandler<GetRejectedTimeSheetByJobIdQuery, Result<PaginationModel<GetTimesheetsResponse>>>
{
    public async Task<Result<PaginationModel<GetTimesheetsResponse>>> Handle(GetRejectedTimeSheetByJobIdQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employer = await _unitOfWork.EmployerRepository.GetById(request.EmployerId);
        if (employer is null)
        {
            return Result<PaginationModel<GetTimesheetsResponse>>.Fail("Employer doesn't exist.");
        }

        var timesheets = _unitOfWork.TimesheetRepository.GetRejectedTimeSheetsByJobId(request.JobId);

        // Add pagination
        PaginationModel<GetTimesheetsResponse> model = new PaginationModel<GetTimesheetsResponse>();
        if (request.PageSize > 0)
        {
            var result = new PagedList<GetTimesheetsResponse>(timesheets, request.PageIndex, request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }

        return Result<PaginationModel<GetTimesheetsResponse>>.Success(model, "timesheets list.");
    }
}

public sealed class GetPendingTimeSheetByJobIdHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetPendingTimeSheetByJobIdQuery, Result<PaginationModel<GetTimesheetsResponse>>>
{
    public async Task<Result<PaginationModel<GetTimesheetsResponse>>> Handle(GetPendingTimeSheetByJobIdQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employer = await _unitOfWork.EmployerRepository.GetById(request.EmployerId);
        if (employer is null)
        {
            return Result<PaginationModel<GetTimesheetsResponse>>.Fail("Employer doesn't exist.");
        }

        var timesheets = _unitOfWork.TimesheetRepository.GetPendingTimeSheetsByJobId(request.JobId);

        // Add pagination
        PaginationModel<GetTimesheetsResponse> model = new PaginationModel<GetTimesheetsResponse>();
        if (request.PageSize > 0)
        {
            var result = new PagedList<GetTimesheetsResponse>(timesheets, request.PageIndex, request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }

        return Result<PaginationModel<GetTimesheetsResponse>>.Success(model, "timesheets list.");
    }
}

public sealed class ChangeTimeSheetVerificationHandler(IUnitOfWork _unitOfWork) : IRequestHandler<ChangeTimeSheetVerificationCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ChangeTimeSheetVerificationCommand request, CancellationToken cancellationToken)
    {
        var timesheet = await _unitOfWork.TimesheetRepository.GetById(request.TimesheetId);
        if (timesheet is null)
        {
            return Result<bool>.Fail("timesheet doesn't exist.");
        }

        var reason = "";

        if (request.Status == (byte)TimeSheetStatusEnum.Approved)
        {
            reason = "Pending";
        } 
        else if(request.Status == (byte)TimeSheetStatusEnum.Rejected)
        {
            reason = "Rejected";
        }

        timesheet.Reason = reason;
        timesheet.Status = request.Status;

        _unitOfWork.TimesheetRepository.Change(timesheet);
        await _unitOfWork.SaveChangesAsync();
        return Result<bool>.Success(true, "timesheets status changed.");
    }
}

public sealed class GetEmployeeListByPaymentStatusHandler(IUnitOfWork _unitOfWork) 
    : IRequestHandler<GetEmployeeListByPaymentStatusQuery, Result<PaginationModel<GetTimesheetsResponse>>>
{
    public async Task<Result<PaginationModel<GetTimesheetsResponse>>> Handle(GetEmployeeListByPaymentStatusQuery request, CancellationToken cancellationToken)
    {
        // get timesheets.
        var timesheets = _unitOfWork.TimesheetRepository.GetEmployeeListByPaymentStatus(request.Request);

        // Add pagination
        PaginationModel<GetTimesheetsResponse> model = new PaginationModel<GetTimesheetsResponse>();
        if (request.PageSize > 0)
        {
            var result = new PagedList<GetTimesheetsResponse>(timesheets, request.PageIndex, request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }

        return Result<PaginationModel<GetTimesheetsResponse>>.Success(model, "timesheets list.");
    }
}
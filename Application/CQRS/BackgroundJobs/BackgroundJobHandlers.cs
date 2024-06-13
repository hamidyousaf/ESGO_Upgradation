namespace Application.CQRS.BackgroundJobs;

public sealed class ConvertJobStatusFromConfirmToCompleteHandler(IUnitOfWork _unitOfWork) 
    : IRequestHandler<ConvertJobStatusFromConfirmToCompleteCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ConvertJobStatusFromConfirmToCompleteCommand request, CancellationToken cancellationToken)
    {
        // update the job older than today to Unsuccessful Status.
        var olderJobs = await _unitOfWork
            .AssignedJobRepository
            .GetAll()
        .Include(x => x.Job)
            .Where(x => x.Job.Date.Date < DateTime.UtcNow.Date && x.JobStatus == (byte)JobStatusEnum.Confirmed)
            .ToListAsync(cancellationToken);

        var timesheets = new List<Timesheet>();
        // add time sheet.
        foreach (var job in olderJobs)
        {
            // calculate billable hours
            var totalMinutes = DateHelper.GetTotalMinutes(job.Job.ShiftStartTime, job.Job.ShiftEndTime);
            var time = TimeSpan.FromMinutes(totalMinutes);
            var totalHours = new TimeOnly((int)time.TotalHours, (int)time.Minutes);
            var billableHours = totalHours.AddMinutes(-job.Job.BreakTime);
            var billableHourInDecimal = DateHelper.ConvertToDecimal(billableHours);

            var timesheet = new Timesheet()
            {
                AssignedJobId = job.Id,
                JobId = job.JobId,
                EmployerId = job.Job.EmployerId,
                EmployeeId = job.EmployeeId,
                Date = DateTime.UtcNow.Date,
                StartTime = job.Job.ShiftStartTime,
                EndTime = job.Job.ShiftEndTime,
                BreakTime = job.Job.BreakTime,
                BillableHours = billableHours,
                BillableHourInDecimal = billableHourInDecimal,
                TotalHours = totalHours,
                HourlyRate = job.Job.HourlyRate,
                TotalAmount = billableHourInDecimal * job.Job.HourlyRate,
                OriginalHourlyRate = job.Job.HourlyRate,
                OrginalTotalAmount = billableHourInDecimal * job.Job.HourlyRate,
                TotalHolidayAmount = billableHourInDecimal * job.Job.HolidayPayRate,
            };
            timesheets.Add(timesheet);
        }
        await _unitOfWork.TimesheetRepository.AddRangeAsync(timesheets);
        // change status of older job to completed.
        olderJobs.ForEach(x =>
        {
            x.JobStatus = (byte)JobStatusEnum.Completed;
            x.CompletionDate = DateTime.UtcNow;
        });
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true,"Job Runs successfully.");
    }
}

public sealed class ConvertJobStatusFromOpenToUnsuccessfulHandler(IUnitOfWork _unitOfWork) 
    : IRequestHandler<ConvertJobStatusFromOpenToUnsuccessfulCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ConvertJobStatusFromOpenToUnsuccessfulCommand request, CancellationToken cancellationToken)
    {
        // update the job older than today to Unsuccessful Status.
        var unsuccessfulJobs = _unitOfWork
            .AssignedJobRepository
            .GetAll()
                .Include(x => x.Job)
            .Where(x => x.Job.Date.Date < DateTime.UtcNow.Date && x.JobStatus == (byte)JobStatusEnum.Open)
            .ExecuteUpdate(setters => setters
        .SetProperty(b => b.JobStatus, (byte)JobStatusEnum.UnSuccessful));

        return Result<bool>.Success(true,"Job Runs successfully.");
    }
}
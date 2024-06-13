namespace Infrastructure.Helpers;

public class ProfileMapper : Profile
{
    public ProfileMapper()
    {
        CreateMap<Employer, GetEmployerByIdResponse>();
        CreateMap<AssignedJob, GetJobsByStatusResponse>()

            .ForMember(dest => dest.IsDummy, opt => opt
                    .MapFrom(src => src.Job.IsDummy))

            .ForMember(dest => dest.IsFixedRate, opt => opt
                    .MapFrom(src => src.Job.IsFixedRate))

            .ForMember(dest => dest.FixedRate, opt => opt
                    .MapFrom(src => src.Job.FixedRate))

            .ForMember(dest => dest.FixedRateAfterCommission, opt => opt
                    .MapFrom(src => src.Job.FixedRateAfterCommission))

            .ForMember(dest => dest.ShiftStartTime, opt => opt
                    .MapFrom(src => src.CreatedDate.Date.Add(src.Job.ShiftStartTime.ToTimeSpan())))
            .ForMember(dest => dest.ShiftEndTime, opt => opt
                    .MapFrom(src => src.CreatedDate.Date.Add(src.Job.ShiftEndTime.ToTimeSpan())))
            .ForMember(dest => dest.JobTypeId, opt => opt
                    .MapFrom(src => src.Job.JobTypeId))
            .ForMember(dest => dest.BreakTime, opt => opt
                    .MapFrom(src => src.Job.BreakTime))
            .ForMember(dest => dest.HourlyRate, opt => opt
                    .MapFrom(src => src.Job.HourlyRate))
            .ForMember(dest => dest.JobCreatedDate, opt => opt
                    .MapFrom(src => src.Job.CreatedDate.Date))
            .ForMember(dest => dest.JobDate, opt => opt
                    .MapFrom(src => src.Job.Date.Date))
            .ForMember(dest => dest.OrganisationName, opt => opt
                    .MapFrom(src => src.Job.Employer.CompanyName))
            .ForMember(dest => dest.ShiftId, opt => opt
                    .MapFrom(src => src.Job.ShiftId))
            .ForMember(dest => dest.AssignedJobId, opt => opt
                    .MapFrom(src => src.Id))
            .ForMember(dest => dest.Location, opt => opt
                    .MapFrom(src => src.Job.Employer.Location))
            .ForMember(dest => dest.StartDate, opt => opt
                    .MapFrom(src => src.Job.Date))
            .ForMember(dest => dest.Status, opt => opt
                    .MapFrom(src => src.JobStatus))
            .ForMember(dest => dest.EmployeeName, opt => opt
                    .MapFrom(src => $"{src.Employee.FirstName} {src.Employee.LastName}"))
            .ForMember(dest => dest.EmployeeType, opt => opt
                    .MapFrom(src => src.Job.EmployeeType.Name))
            .ForMember(dest => dest.EmployeeTypeId, opt => opt
                    .MapFrom(src => src.Job.EmployeeTypeId))
            .ForMember(dest => dest.EmployeeId, opt => opt
                    .MapFrom(src => src.EmployeeId));
        CreateMap<AssignedJob, GetCompletedJobsResponse>()
                        .ForMember(dest => dest.Date, opt => opt
                    .MapFrom(src => src.Job.Date))
                        .ForMember(dest => dest.AssignedJobId, opt => opt
                    .MapFrom(src => src.Id))
            .ForMember(dest => dest.IsUrgent, opt => opt
                    .MapFrom(src => src.Job.JobTypeId == (byte)JobTypeEnum.UrgentJob ? true : false))
            .ForMember(dest => dest.Location, opt => opt
                    .MapFrom(src => src.Job.Employer.Location))
            .ForMember(dest => dest.StartDate, opt => opt
                    .MapFrom(src => src.Job.Date))
            .ForMember(dest => dest.EmployeeType, opt => opt
                    .MapFrom(src => src.Job.EmployeeType.Name))
            .ForMember(dest => dest.ShiftStartTime, opt => opt
                    .MapFrom(src => src.Job.ShiftStartTime))
            .ForMember(dest => dest.ShiftEndTime, opt => opt
                    .MapFrom(src => src.Job.ShiftEndTime))
            .ForMember(dest => dest.PinCode, opt => opt
                    .MapFrom(src => src.Employer.PinCode))
            .ForMember(dest => dest.HourlyRate, opt => opt
                    .MapFrom(src => src.Job.HourlyRate))
            .ForMember(dest => dest.JobStatus, opt => opt
                    .MapFrom(src => src.JobStatus));

        CreateMap<AssignedJob, GetConfirmedJobsForAdminResponse>()
                        .ForMember(dest => dest.AssignedJobId, opt => opt
                    .MapFrom(src => src.Id))
            .ForMember(dest => dest.Location, opt => opt
                    .MapFrom(src => src.Job.Employer.Location))
            .ForMember(dest => dest.StartDate, opt => opt
                    .MapFrom(src => src.Job.Date))
            .ForMember(dest => dest.EmployeeType, opt => opt
                    .MapFrom(src => src.Job.EmployeeType.Name))
            .ForMember(dest => dest.ShiftStartTime, opt => opt
                    .MapFrom(src => src.Job.ShiftStartTime))
            .ForMember(dest => dest.ShiftEndTime, opt => opt
                    .MapFrom(src => src.Job.ShiftEndTime));

        CreateMap<AssignedJob, GetAssignedJobEmployeeByIdForAdminResponse>()
            .ForMember(dest => dest.AssignedJobId, opt => opt
                .MapFrom(src => src.Id))
            .ForMember(dest => dest.EmployeeName, opt => opt
                .MapFrom(src => $"{src.Employee.FirstName} {src.Employee.LastName}"))
            .ForMember(dest => dest.ContactNumber, opt => opt
                .MapFrom(src => src.Employee.PhoneNumber))
            .ForMember(dest => dest.Email, opt => opt
                .MapFrom(src => src.Employee.Email))
            .ForMember(dest => dest.Address, opt => opt
                .MapFrom(src => src.Employee.Address))
            .ForMember(dest => dest.City, opt => opt
                .MapFrom(src => src.Employee.City))
            .ForMember(dest => dest.JobDate, opt => opt
                .MapFrom(src => src.Job.Date))
            .ForMember(dest => dest.AppliedDate, opt => opt
                .MapFrom(src => src.AppliedDate));





        CreateMap<DbsDocument, DbsDocumentDto>()
            .ForMember(dest => dest.Name, opt => opt
                    .MapFrom(src => src.DocumentType.Name))
            .ForMember(dest => dest.GroupNo, opt => opt
                    .MapFrom(src => src.DocumentType.GroupNo));
        CreateMap<DocumentType, DocumentTypeForGetDocumentTypesResponce>();
        CreateMap<AssignedJob, GetUnsuccessfulJobsResponse>()
                        .ForMember(dest => dest.AssignedJobId, opt => opt
                    .MapFrom(src => src.Id))
            .ForMember(dest => dest.Location, opt => opt
                    .MapFrom(src => src.Job.Employer.Location))
            .ForMember(dest => dest.StartDate, opt => opt
                    .MapFrom(src => src.Job.Date))
            .ForMember(dest => dest.EmployeeType, opt => opt
                    .MapFrom(src => src.Job.EmployeeType.Name));

        CreateMap<Timesheet, GetTimesheetsByStatusForEmployerResponse>()
            .ForMember(dest => dest.JobId, opt => opt
                    .MapFrom(src => src.AssignedJobId))
            .ForMember(dest => dest.TimesheetId, opt => opt
                    .MapFrom(src => src.Id))
            .ForMember(dest => dest.TimesheetDate, opt => opt
                    .MapFrom(src => src.Date))
            .ForMember(dest => dest.ShiftId, opt => opt
                    .MapFrom(src => src.Job.ShiftId))
            .ForMember(dest => dest.StartTime, opt => opt
                    .MapFrom(src => src.Date.Date.Add(src.StartTime.ToTimeSpan())))
            .ForMember(dest => dest.EndTime, opt => opt
                    .MapFrom(src => src.Date.Date.Add(src.EndTime.ToTimeSpan())))
                .ForMember(dest => dest.JobDate, opt => opt
                    .MapFrom(src => src.Job.Date))
                .ForMember(dest => dest.JobCreatedDate, opt => opt
                    .MapFrom(src => src.Job.CreatedDate.Date))
                .ForMember(dest => dest.OrganisationName, opt => opt
                    .MapFrom(src => src.Employer.CompanyName))
                .ForMember(dest => dest.EmployeeId, opt => opt
                    .MapFrom(src => src.Employee.Id))
                .ForMember(dest => dest.EmployeeType, opt => opt
                    .MapFrom(src => src.Employee.EmployeeType.Name))
                .ForMember(dest => dest.EmployeeName, opt => opt
                    .MapFrom(src => $"{src.Employee.FirstName} {src.Employee.LastName}"));

        CreateMap<Timesheet, GetTimesheetsByStatusForAdminResponce>()
            .ForMember(dest => dest.StartTime, opt => opt
                    .MapFrom(src => src.Date.Date.Add(src.StartTime.ToTimeSpan())))
            .ForMember(dest => dest.EndTime, opt => opt
                    .MapFrom(src => src.Date.Date.Add(src.EndTime.ToTimeSpan())))
                .ForMember(dest => dest.EmployeeId, opt => opt
                    .MapFrom(src => src.Employee.Id))
                .ForMember(dest => dest.EmployerId, opt => opt
                    .MapFrom(src => src.Employer.Id))
                .ForMember(dest => dest.ShiftId, opt => opt
                    .MapFrom(src => src.Job.ShiftId))
                .ForMember(dest => dest.EmployeeName, opt => opt
                    .MapFrom(src => src.Employer.Name))
                .ForMember(dest => dest.EmployeeType, opt => opt
                    .MapFrom(src => src.Employee.EmployeeType.Name))
                .ForMember(dest => dest.EmployeeName, opt => opt
                    .MapFrom(src => $"{src.Employee.FirstName} {src.Employee.LastName}"));
        CreateMap<Timesheet, GetTimesheetByIdResponce>()
            .ForMember(dest => dest.StartTime, opt => opt
                    .MapFrom(src => src.Date.Date.Add(src.StartTime.ToTimeSpan())))
            .ForMember(dest => dest.EndTime, opt => opt
                    .MapFrom(src => src.Date.Date.Add(src.EndTime.ToTimeSpan())))
            .ForMember(dest => dest.OrganisationName, opt => opt
                    .MapFrom(src => src.Job.Employer.CompanyName));
        CreateMap<Timesheet, GetTimesheetsByStatusResponse>()

            .ForMember(dest => dest.IsFixedRate, opt => opt
                    .MapFrom(src => src.Job.IsFixedRate))

            .ForMember(dest => dest.FixedRate, opt => opt
                    .MapFrom(src => src.Job.FixedRate))

            .ForMember(dest => dest.FixedRateAfterCommission, opt => opt
                    .MapFrom(src => src.Job.FixedRateAfterCommission))
                        .ForMember(dest => dest.IsUrgent, opt => opt
                    .MapFrom(src => src.Job.JobTypeId == (byte)JobTypeEnum.UrgentJob ? true : false))
                        .ForMember(dest => dest.TimesheetId, opt => opt
                    .MapFrom(src => src.Id))
            .ForMember(dest => dest.OrganisationName, opt => opt
                    .MapFrom(src => src.Job.Employer.CompanyName))
            .ForMember(dest => dest.PostalCode, opt => opt
                    .MapFrom(src => src.Employer.PinCode))
            .ForMember(dest => dest.Date, opt => opt
                    .MapFrom(src => src.Job.Date))
            .ForMember(dest => dest.HourlyRate, opt => opt
                    .MapFrom(src => src.Job.HourlyRate))
            .ForMember(dest => dest.ShiftStartTime, opt => opt
                    .MapFrom(src => src.Job.ShiftStartTime))
            .ForMember(dest => dest.ShiftEndTime, opt => opt
                    .MapFrom(src => src.Job.ShiftEndTime))
            .ForMember(dest => dest.Status, opt => opt
                    .MapFrom(src => src.Status));

        CreateMap<Employer, EmployerForGetAssignedJobByIdResponce>();
        CreateMap<Job, JobForGetAssignedJobByIdResponce>();
        CreateMap<AssignedJob, GetAssignedJobByIdResponce>()
            .ForMember(dest => dest.EmployerId, opt => opt
                    .MapFrom(src => src.EmployerId))
            .ForMember(dest => dest.EmployeeTypeDescription, opt => opt
                    .MapFrom(src => src.Employee.EmployeeType.Name))
            .ForMember(dest => dest.AssignedJobId, opt => opt
                    .MapFrom(src => src.Id));        
        
        CreateMap<Job, GetJobByIdResponce>()
            .ForMember(dest => dest.EmployeeTypeId, opt => opt
                    .MapFrom(src => src.EmployeeTypeId))
            .ForMember(dest => dest.EmployerId, opt => opt
                    .MapFrom(src => src.EmployerId))
            .ForMember(dest => dest.JobId, opt => opt
                    .MapFrom(src => src.Id))
            .ForMember(dest => dest.EmployeeTypeDescription, opt => opt
                    .MapFrom(src => src.EmployeeType.Name))
            .ForMember(dest => dest.Title, opt => opt
                    .MapFrom(src => src.Employer.JobTitle))
            .ForMember(dest => dest.Name, opt => opt
                    .MapFrom(src => src.Employer.Name))
            .ForMember(dest => dest.Email, opt => opt
                    .MapFrom(src => src.Employer.Email))
            .ForMember(dest => dest.CompanyName, opt => opt
                    .MapFrom(src => src.Employer.CompanyName))
            .ForMember(dest => dest.CompanyNo, opt => opt
                    .MapFrom(src => src.Employer.CompanyNo))
            .ForMember(dest => dest.PinCode, opt => opt
                    .MapFrom(src => src.Employer.PinCode))
            .ForMember(dest => dest.AboutOrganization, opt => opt
                    .MapFrom(src => src.Employer.AboutOrganization))
            .ForMember(dest => dest.OrganizationImageUrl, opt => opt
                    .MapFrom(src => src.Employer.OrganizationImageUrl))
            .ForMember(dest => dest.Address, opt => opt
                    .MapFrom(src => src.Employer.Address))
            .ForMember(dest => dest.Address2, opt => opt
                    .MapFrom(src => src.Employer.Address2))
            .ForMember(dest => dest.PhoneNumber, opt => opt
                    .MapFrom(src => src.Employer.PhoneNumber))
            .ForMember(dest => dest.Date, opt => opt
                    .MapFrom(src => src.Date.Date))
            .ForMember(dest => dest.ShiftStartTime, opt => opt
                    .MapFrom(src => src.Date.Date.Add(src.ShiftStartTime.ToTimeSpan())))
            .ForMember(dest => dest.ShiftEndTime, opt => opt
                    .MapFrom(src => src.Date.Date.Add(src.ShiftEndTime.ToTimeSpan())))
            .ForMember(dest => dest.HourlyRate, opt => opt
                    .MapFrom(src => src.HourlyRate));

        CreateMap<Employee, GetDbsExpiredEmployeesResponse>()
            .ForMember(dest => dest.FullName, opt => opt
                    .MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.ExpiryDate, opt => opt
                    .MapFrom(src => src.DbsExpiryDate))
            .ForMember(dest => dest.Status, opt => opt
                    .MapFrom(src => (src.DbsExpiryDate < DateTime.UtcNow.Date && src.DbsExpiryDate.HasValue) ? "Expired" : (src.DbsExpiryDate.HasValue && (src.DbsExpiryDate.Value - DateTime.UtcNow.Date).TotalDays > 0) ? $"To be expired in {(src.DbsExpiryDate.Value - DateTime.UtcNow.Date).TotalDays} Days": "To be expired today."));
        CreateMap<Employee, GetTrainingCertificateExpiredEmployeesResponse>()
            .ForMember(dest => dest.FullName, opt => opt
                    .MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.Status, opt => opt
                    .MapFrom(src => $"Expired"));
        CreateMap<AssignedJob, GetAssignedJobsResponse>()


            .ForMember(dest => dest.IsFixedRate, opt => opt
                    .MapFrom(src => src.Job.IsFixedRate))

            .ForMember(dest => dest.FixedRate, opt => opt
                    .MapFrom(src => src.Job.FixedRate))

            .ForMember(dest => dest.FixedRateAfterCommission, opt => opt
                    .MapFrom(src => src.Job.FixedRateAfterCommission))
            .ForMember(dest => dest.EmployeeTypeId, opt => opt
                    .MapFrom(src => src.Job.EmployeeTypeId))
            .ForMember(dest => dest.AssignedJobId, opt => opt
                    .MapFrom(src => src.Id))
            .ForMember(dest => dest.OrganisationName, opt => opt
                    .MapFrom(src => src.Employer.CompanyName))
            .ForMember(dest => dest.IsUrgent, opt => opt
                    .MapFrom(src => src.Job.JobTypeId == (byte)JobTypeEnum.UrgentJob ? true : false))

            .ForMember(dest => dest.PostalCode, opt => opt
                    .MapFrom(src => src.Employer.PinCode))
            .ForMember(dest => dest.Date, opt => opt
                    .MapFrom(src => src.Job.Date))
            .ForMember(dest => dest.ShiftTime, opt => opt
                    .MapFrom(src => $"{src.Job.ShiftStartTime} - {src.Job.ShiftEndTime}"))

            .ForMember(dest => dest.HourlyRate, opt => opt
                    .MapFrom(src => src.Job.HourlyRate))
            .ForMember(dest => dest.SelfCommission, opt => opt
                    .MapFrom(src => src.Job.SelfCommission))
            .ForMember(dest => dest.HourlyRateAfterSelfCommission, opt => opt
                    .MapFrom(src => src.Job.HourlyRateAfterSelfCommission))
            .ForMember(dest => dest.PayrollCommission, opt => opt
                    .MapFrom(src => src.Job.PayrollCommission))
            .ForMember(dest => dest.HourlyRateAfterPayrollCommission, opt => opt
                    .MapFrom(src => src.Job.HourlyRateAfterPayrollCommission))
            .ForMember(dest => dest.LimitedCommission, opt => opt
                    .MapFrom(src => src.Job.LimitedCommission))
            .ForMember(dest => dest.HourlyRateAfterLimitedCommission, opt => opt
                    .MapFrom(src => src.Job.HourlyRateAfterLimitedCommission));
         CreateMap<AssignedJob, GetAppliedJobsResponse>()


            .ForMember(dest => dest.IsFixedRate, opt => opt
                    .MapFrom(src => src.Job.IsFixedRate))

            .ForMember(dest => dest.FixedRate, opt => opt
                    .MapFrom(src => src.Job.FixedRate))

            .ForMember(dest => dest.FixedRateAfterCommission, opt => opt
                    .MapFrom(src => src.Job.FixedRateAfterCommission))
            .ForMember(dest => dest.EmployeeTypeId, opt => opt
                    .MapFrom(src => src.Job.EmployeeTypeId))
            .ForMember(dest => dest.AssignedJobId, opt => opt
                    .MapFrom(src => src.Id))
            .ForMember(dest => dest.OrganisationName, opt => opt
                    .MapFrom(src => src.Employer.CompanyName))
            .ForMember(dest => dest.IsUrgent, opt => opt
                    .MapFrom(src => src.Job.JobTypeId == (byte)JobTypeEnum.UrgentJob ? true : false))

            .ForMember(dest => dest.PostalCode, opt => opt
                    .MapFrom(src => src.Employer.PinCode))
            .ForMember(dest => dest.Date, opt => opt
                    .MapFrom(src => src.Job.Date))
            .ForMember(dest => dest.HourlyRate, opt => opt
                    .MapFrom(src => src.Job.HourlyRate))
            .ForMember(dest => dest.ShiftTime, opt => opt
                    .MapFrom(src => $"{src.Job.ShiftStartTime} - {src.Job.ShiftEndTime}"))
            .ForMember(dest => dest.SelfCommission, opt => opt
                    .MapFrom(src => src.Job.SelfCommission))
            .ForMember(dest => dest.HourlyRateAfterSelfCommission, opt => opt
                    .MapFrom(src => src.Job.HourlyRateAfterSelfCommission))
            .ForMember(dest => dest.PayrollCommission, opt => opt
                    .MapFrom(src => src.Job.PayrollCommission))
            .ForMember(dest => dest.HourlyRateAfterPayrollCommission, opt => opt
                    .MapFrom(src => src.Job.HourlyRateAfterPayrollCommission))
            .ForMember(dest => dest.LimitedCommission, opt => opt
                    .MapFrom(src => src.Job.LimitedCommission))
            .ForMember(dest => dest.HourlyRateAfterLimitedCommission, opt => opt
                    .MapFrom(src => src.Job.HourlyRateAfterLimitedCommission));
        CreateMap<AssignedJob, GetConfirmedJobsResponse>()

            .ForMember(dest => dest.IsFixedRate, opt => opt
                    .MapFrom(src => src.Job.IsFixedRate))

            .ForMember(dest => dest.FixedRate, opt => opt
                    .MapFrom(src => src.Job.FixedRate))

            .ForMember(dest => dest.FixedRateAfterCommission, opt => opt
                    .MapFrom(src => src.Job.FixedRateAfterCommission))
            .ForMember(dest => dest.EmployeeTypeId, opt => opt
                    .MapFrom(src => src.Job.EmployeeTypeId))
           .ForMember(dest => dest.AssignedJobId, opt => opt
                   .MapFrom(src => src.Id))
           .ForMember(dest => dest.OrganisationName, opt => opt
                   .MapFrom(src => src.Employer.CompanyName))
            .ForMember(dest => dest.IsUrgent, opt => opt
                    .MapFrom(src => src.Job.JobTypeId == (byte)JobTypeEnum.UrgentJob ? true : false))
           .ForMember(dest => dest.PostalCode, opt => opt
                   .MapFrom(src => src.Employer.PinCode))
           .ForMember(dest => dest.Date, opt => opt
                   .MapFrom(src => src.Job.Date))
           .ForMember(dest => dest.HourlyRate, opt => opt
                   .MapFrom(src => src.Job.HourlyRate))
           .ForMember(dest => dest.ShiftTime, opt => opt
                   .MapFrom(src => $"{src.Job.ShiftStartTime} - {src.Job.ShiftEndTime}"))

           .ForMember(dest => dest.SelfCommission, opt => opt
                    .MapFrom(src => src.Job.SelfCommission))
            .ForMember(dest => dest.HourlyRateAfterSelfCommission, opt => opt
                    .MapFrom(src => src.Job.HourlyRateAfterSelfCommission))
            .ForMember(dest => dest.PayrollCommission, opt => opt
                    .MapFrom(src => src.Job.PayrollCommission))
            .ForMember(dest => dest.HourlyRateAfterPayrollCommission, opt => opt
                    .MapFrom(src => src.Job.HourlyRateAfterPayrollCommission))
            .ForMember(dest => dest.LimitedCommission, opt => opt
                    .MapFrom(src => src.Job.LimitedCommission))
            .ForMember(dest => dest.HourlyRateAfterLimitedCommission, opt => opt
                    .MapFrom(src => src.Job.HourlyRateAfterLimitedCommission));


        CreateMap<MonthlySupervisionReport, GetMonthlySupervisionReportsResponse>();
        CreateMap<Feedback, GetFeedBacksResponse>();
        CreateMap<ShadowShift, GetShadowShiftsResponse>();
        CreateMap<Reference, GetProfessionalReferenceByEmployeeIdResponce>();
        CreateMap<Reference, GetPersonalReferenceByEmployeeIdResponce>();
        CreateMap<Employee, GetEmployeeAllDetailsByIdResponce>();
        CreateMap<EmployementType, EmployementTypeForGetEmployeeAllDetailsByIdResponce>();
        CreateMap<EmployeeType, EmployeeTypeForGetEmployeeAllDetailsByIdResponce>();
        CreateMap<Employee, GetEmployeeByIdResponse>();
        CreateMap<EmployeeType, EmployeeTypeResponce>();
        CreateMap<Employee, GetEmployeesByEmployeeTypeIdResponse>()
            .ForMember(dest => dest.FullName, opt => opt
                    .MapFrom(src => $"{src.Title} {src.FirstName} {src.LastName}"));
        CreateMap<EmployeeType, GetEmployeeTypeByIdResponse>();
        CreateMap<Shift, GetShiftByIdResponse>()
            .ForMember(dest => dest.StartTime, opt => opt
                    .MapFrom(src => src.CreatedDate.Date.Add(src.StartTime.ToTimeSpan())))
            .ForMember(dest => dest.EndTime, opt => opt
                    .MapFrom(src => src.CreatedDate.Date.Add(src.EndTime.ToTimeSpan())));
        CreateMap<Timesheet, GetTimesheetsResponse>();
        CreateMap<Shift, GetShiftsResponse>();
        CreateMap<EmployeeType, EmployeeTypeResponse>();
        CreateMap<EmployementType, EmployementTypeResponse>();
        CreateMap<Qualification, GetQualificationsByEmployeeResponse>();
        CreateMap<Employement, GetEmployementsByEmployeeResponse>();
        CreateMap<EmployeeDocument, GetDocumentsByEmployeeResponse>()
            .ForMember(dest => dest.DocumentName, opt => opt
                    .MapFrom(src => src.Document.Name));
        CreateMap<Employer, GetAllEmployersByStatusResponse>();
        CreateMap<Booking, GetAPIsForJobResponce>()
            .ForMember(dest => dest.EmployerId, opt => opt
                    .MapFrom(src => src.EmployerId))
            .ForMember(dest => dest.EmployerName, opt => opt
                    .MapFrom(src => src.Employer.Name));
        CreateMap<Booking, GetBookingsResponse>();
        CreateMap<Booking, GetAllBookingsResponce>()
            .ForMember(dest => dest.EmployerName, opt => opt
                    .MapFrom(src => src.Employer.Name));
        CreateMap<Booking, GetBookingResponce>();
        CreateMap<Employee, GetDocumentPolicyInfoResponse>()
            .ForMember(dest => dest.EmployeeId, opt => opt
                    .MapFrom(src => src.Id));
        CreateMap<Document, GetDocumentByCategoryIdResponse>();
        CreateMap<Employee, GetEmployeeBySecretIdResponse>();
        CreateMap<Document, GetEmployeeDocumentsResponse>()
            .ForMember(dest => dest.DocumentId, opt => opt
                    .MapFrom(src => src.Id))
            .ForMember(dest => dest.DocumentName, opt => opt
                    .MapFrom(src => src.Name));


        CreateMap<Employee, GetEmployeeShortDetailsByIdResponce>()
                .ForMember(dest => dest.EmployeeType, opt => opt
                    .MapFrom(src => src.EmployeeType.Name))
                .ForMember(dest => dest.EmployementType, opt => opt
                    .MapFrom(src => src.EmployementType.Name));
        CreateMap<Employer, GetEmployerAllDetailsByIdResponce>();
        CreateMap<Employee, GetAllEmployeesByStatusResponse>()
                .ForMember(dest => dest.EmployeeType, opt => opt
                    .MapFrom(src => src.EmployeeType.Name))
                .ForMember(dest => dest.Name, opt => opt
                    .MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Reason, opt => opt
                    .MapFrom(src => src.AccountStatusChangeReason))
              ;
    }
}

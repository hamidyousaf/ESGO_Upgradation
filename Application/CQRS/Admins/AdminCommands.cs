namespace Domain.CQRS.Admins;

public sealed class AddJobCommissionCommand : IRequest<Result<bool>>
{
    public AddJobCommissionRequest Request { get; set; }

    public AddJobCommissionCommand(AddJobCommissionRequest request)
    {
        Request = request;
    }
}

public sealed class AddJobCommand : IRequest<Result<bool>>
{
    public AddJobRequest Request { get; set; }

    public AddJobCommand(AddJobRequest request)
    {
        Request = request;
    }
}

public sealed class JobCommand : IRequest<Result<Job>>
{
    public AddJobRequest Request { get; set; }

    public JobCommand(AddJobRequest request)
    {
        Request = request;
    }
}

public sealed class DeleteEmployeesByIdsCommand : IRequest<Result<bool>>
{
    public DeleteEmployeesByIdsRequest Request { get; set; }

    public DeleteEmployeesByIdsCommand(DeleteEmployeesByIdsRequest request)
    {
        Request = request;
    }
}

public sealed class DeleteEmployersByIdsCommand : IRequest<Result<bool>>
{
    public DeleteEmployersByIdsRequest Request { get; set; }

    public DeleteEmployersByIdsCommand(DeleteEmployersByIdsRequest request)
    {
        Request = request;
    }
}

public sealed class AddMultilpleJobCommand : IRequest<Result<bool>>
{
    public List<AddJobRequest> Request { get; set; }

    public AddMultilpleJobCommand(List<AddJobRequest> request)
    {
        Request = request;
    }
}

public sealed class ChangeUTRNumberStatusCommand : IRequest<Result<bool>>
{
    public ChangeUTRNumberStatusRequest Request { get; set; }

    public ChangeUTRNumberStatusCommand(ChangeUTRNumberStatusRequest request)
    {
        Request = request;
    }
}

public sealed class ChangeCompanyNumberStatusCommand : IRequest<Result<bool>>
{
    public ChangeCompanyNumberStatusRequest Request { get; set; }

    public ChangeCompanyNumberStatusCommand(ChangeCompanyNumberStatusRequest request)
    {
        Request = request;
    }
}

public sealed class ChangeNMCRegistrationStatusCommand : IRequest<Result<bool>>
{
    public ChangeNMCRegistrationStatusRequest Request { get; set; }

    public ChangeNMCRegistrationStatusCommand(ChangeNMCRegistrationStatusRequest request)
    {
        Request = request;
    }
}

public sealed class ChangeNationalInsuranceNumberStatusCommand : IRequest<Result<bool>>
{
    public ChangeNationalInsuranceNumberStatusRequest Request { get; set; }

    public ChangeNationalInsuranceNumberStatusCommand(ChangeNationalInsuranceNumberStatusRequest request)
    {
        Request = request;
    }
}

public sealed class ChangePersonalReferenceStatusCommand : IRequest<Result<bool>>
{
    public ChangePersonalReferenceStatusRequest Request { get; set; }

    public ChangePersonalReferenceStatusCommand(ChangePersonalReferenceStatusRequest request)
    {
        Request = request;
    }
}

public sealed class ChangeProfessionalReferenceStatusCommand : IRequest<Result<bool>>
{
    public ChangeProfessionalReferenceStatusRequest Request { get; set; }

    public ChangeProfessionalReferenceStatusCommand(ChangeProfessionalReferenceStatusRequest request)
    {
        Request = request;
    }
}

public sealed class ChangeBiometricResidenceCardStatusCommand : IRequest<Result<bool>>
{
    public ChangeBiometricResidenceCardStatusRequest Request { get; set; }

    public ChangeBiometricResidenceCardStatusCommand(ChangeBiometricResidenceCardStatusRequest request)
    {
        Request = request;
    }
}

public sealed class ChangePassportStatusCommand : IRequest<Result<bool>>
{
    public ChangePassportStatusRequest Request { get; set; }

    public ChangePassportStatusCommand(ChangePassportStatusRequest request)
    {
        Request = request;
    }
}

public sealed class ChangeDbsDocumentStatusCommand : IRequest<Result<bool>>
{
    public ChangeDbsDocumentStatusRequest Request { get; set; }

    public ChangeDbsDocumentStatusCommand(ChangeDbsDocumentStatusRequest request)
    {
        Request = request;
    }
}

public sealed class ChangeDbsNumebrStatusCommand : IRequest<Result<bool>>
{
    public ChangeDbsNumebrStatusRequest Request { get; set; }

    public ChangeDbsNumebrStatusCommand(ChangeDbsNumebrStatusRequest request)
    {
        Request = request;
    }
}

public sealed class ChangeDbsCertificateStatusCommand : IRequest<Result<bool>>
{
    public ChangeDbsCertificateStatusRequest Request { get; set; }

    public ChangeDbsCertificateStatusCommand(ChangeDbsCertificateStatusRequest request)
    {
        Request = request;
    }
}

public sealed class ChangeAccessNIStatusCommand : IRequest<Result<bool>>
{
    public ChangeAccessNIStatusRequest Request { get; set; }

    public ChangeAccessNIStatusCommand(ChangeAccessNIStatusRequest request)
    {
        Request = request;
    }
}

public sealed class ChangeNationalInsuranceStatusCommand : IRequest<Result<bool>>
{
    public ChangeNationalInsuranceStatusRequest Request { get; set; }

    public ChangeNationalInsuranceStatusCommand(ChangeNationalInsuranceStatusRequest request)
    {
        Request = request;
    }
}

public sealed class DeletePersonalReferenceCommand : IRequest<Result<bool>>
{
    public int EmployeeId { get; set; }

    public DeletePersonalReferenceCommand(int employeeId)
    {
        EmployeeId = employeeId;
    }
}

public sealed class DeleteProfessionalReferenceCommand : IRequest<Result<bool>>
{
    public int EmployeeId { get; set; }

    public DeleteProfessionalReferenceCommand(int employeeId)
    {
        EmployeeId = employeeId;
    }
}

public sealed class AddShadowShiftCommand : IRequest<Result<bool>>
{
    public AddShadowShiftRequest Request { get; set; }

    public AddShadowShiftCommand(AddShadowShiftRequest request)
    {
        Request = request;
    }
}

public sealed class AddInterviewCommand : IRequest<Result<bool>>
{
    public AddInterviewRequest Request { get; set; }

    public AddInterviewCommand(AddInterviewRequest request)
    {
        Request = request;
    }
}

public sealed class AddMonthlySupervisionReportCommand : IRequest<Result<bool>>
{
    public AddMonthlySupervisionReportRequest Request { get; set; }

    public AddMonthlySupervisionReportCommand(AddMonthlySupervisionReportRequest request)
    {
        Request = request;
    }
}

public sealed class ChangeInterviewStatusCommand : IRequest<Result<bool>>
{
    public ChangeInterviewStatusRequest Request { get; set; }

    public ChangeInterviewStatusCommand(ChangeInterviewStatusRequest request)
    {
        Request = request;
    }
}

public sealed class AddFeedbackCommand : IRequest<Result<bool>>
{
    public AddFeedbackRequest Request { get; set; }

    public AddFeedbackCommand(AddFeedbackRequest request)
    {
        Request = request;
    }
}

public sealed class AddInvoiceCommand : IRequest<Result<bool>>
{
    public AddInvoiceRequest Request { get; set; }

    public AddInvoiceCommand(AddInvoiceRequest request)
    {
        Request = request;
    }
}

public sealed class UpdateEmployeeDetailsCommand : IRequest<Result<bool>>
{
    public UpdateEmployeeDetailsRequest Request { get; set; }

    public UpdateEmployeeDetailsCommand(UpdateEmployeeDetailsRequest request)
    {
        Request = request;
    }
}

public sealed class UpdateTimesheetByIdCommand : IRequest<Result<bool>>
{
    public UpdateTimesheetByIdRequest Request { get; set; }

    public UpdateTimesheetByIdCommand(UpdateTimesheetByIdRequest request)
    {
        Request = request;
    }
}

public sealed class ChangeJobStatusToUnseccessfullCommand : IRequest<Result<bool>> {}

public sealed class EmployeeLoginCommand : IRequest<Result<EmployeeLoginResponse>>
{
    public EmployeeLoginRequest Request { get; }
    public EmployeeLoginCommand(EmployeeLoginRequest request)
    {
        Request = request;
    }
}

public sealed class EmployerLoginForAdminCommand : IRequest<Result<EmployerLoginResponse>>
{
    public EmployerLoginRequest Request { get; }
    public EmployerLoginForAdminCommand(EmployerLoginRequest request)
    {
        Request = request;
    }
}

public sealed class MarkAsReadNotificationCommand : IRequest<Result<bool>>
{
    public int NotificationId { get; set; }
    public MarkAsReadNotificationCommand(int notificationId)
    {
        NotificationId = notificationId;
    }
}

public sealed class MarkAllAsReadNotificationsCommand : IRequest<Result<bool>> {}
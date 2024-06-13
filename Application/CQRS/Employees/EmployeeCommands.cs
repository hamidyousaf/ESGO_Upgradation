namespace Domain.CQRS.Employees;

public sealed class AddEmployeeCommand : IRequest<Result<int>>
{
    public AddEmployeeRequest Employee { get; }
    public AddEmployeeCommand(AddEmployeeRequest employee)
    {
        Employee = employee;
    }
}

public sealed class AddAddressCommand : IRequest<Result<bool>>
{
    public AddAddressRequest Address { get; }
    public AddAddressCommand(AddAddressRequest address)
    {
        Address = address;
    }
}

public sealed class AddNMCRegistrationCommand : IRequest<Result<bool>>
{
    public AddNMCRegistrationRequest Request { get; }
    public AddNMCRegistrationCommand(AddNMCRegistrationRequest request)
    {
        Request = request;
    }
}


public sealed class AddQualificationCommand : IRequest<Result<bool>>
{
    public AddQualificationRequest Request { get; }
    public AddQualificationCommand(AddQualificationRequest request)
    {
        Request = request;
    }
}

public sealed class DeleteQualificationCommand : IRequest<Result<bool>>
{
    public DeleteQualificationRequest Request { get; }
    public DeleteQualificationCommand(DeleteQualificationRequest request)
    {
        Request = request;
    }
}

public sealed class DeleteDbsDocumentCommand : IRequest<Result<bool>>
{
    public DeleteDbsDocumentRequest Request { get; }
    public DeleteDbsDocumentCommand(DeleteDbsDocumentRequest request)
    {
        Request = request;
    }
}


public sealed class AddEmployementCommand : IRequest<Result<bool>>
{
    public AddEmployementRequest Request { get; }
    public AddEmployementCommand(AddEmployementRequest request)
    {
        Request = request;
    }
}


public sealed class DeleteEmployementCommand : IRequest<Result<bool>>
{
    public DeleteEmployementRequest Request { get; }
    public DeleteEmployementCommand(DeleteEmployementRequest request)
    {
        Request = request;
    }
}

public sealed class AddWorkGapReasonCommand : IRequest<Result<bool>>
{
    public AddWorkGapReasonRequest Request { get; }
    public AddWorkGapReasonCommand(AddWorkGapReasonRequest request)
    {
        Request = request;
    }
}

public sealed class GenerateReferenceLinkCommand : IRequest<Result<bool>>
{
    public int EmployeeId { get; }
    public GenerateReferenceLinkCommand(int employeeId)
    {
        EmployeeId = employeeId;
    }
}

public sealed class SendReferenceLinkToMailCommand : IRequest<Result<bool>>
{
    public SendReferenceLinkToMailRequest Request { get; }
    public SendReferenceLinkToMailCommand(SendReferenceLinkToMailRequest request)
    {
        Request = request;
    }
}

public sealed class AddPersonalReferenceCommand : IRequest<Result<bool>>
{
    public AddPersonalReferenceRequest Request { get; }
    public AddPersonalReferenceCommand(AddPersonalReferenceRequest request)
    {
        Request = request;
    }
}

public sealed class AddProfessionalReferenceCommand : IRequest<Result<bool>>
{
    public AddProfessionalReferenceRequest Request { get; }
    public AddProfessionalReferenceCommand(AddProfessionalReferenceRequest request)
    {
        Request = request;
    }
}

public sealed class AddEmployeeDocumentCommand : IRequest<Result<bool>>
{
    public AddEmployeeDocumentRequest Request { get; }
    public AddEmployeeDocumentCommand(AddEmployeeDocumentRequest request)
    {
        Request = request;
    }
}

public sealed class DeleteEmployeeDocumentCommand : IRequest<Result<bool>>
{
    public DeleteEmployeeDocumentRequest Request { get; }
    public DeleteEmployeeDocumentCommand(DeleteEmployeeDocumentRequest request)
    {
        Request = request;
    }
}

public sealed class AddDbsCertificateCommand : IRequest<Result<bool>>
{
    public AddDbsCertificateRequest Request { get; }
    public AddDbsCertificateCommand(AddDbsCertificateRequest request)
    {
        Request = request;
    }
}

public sealed class AddNationalInsuranceCommand : IRequest<Result<bool>>
{
    public AddNationalInsuranceRequest Request { get; }
    public AddNationalInsuranceCommand(AddNationalInsuranceRequest request)
    {
        Request = request;
    }
}

public sealed class AddAccessNICommand : IRequest<Result<bool>>
{
    public AddAccessNIRequest Request { get; }
    public AddAccessNICommand(AddAccessNIRequest request)
    {
        Request = request;
    }
}

public sealed class AddDbsNumberCommand : IRequest<Result<bool>>
{
    public AddDbsNumberRequest Request { get; }
    public AddDbsNumberCommand(AddDbsNumberRequest request)
    {
        Request = request;
    }
}

public sealed class AddBankDetailsCommand : IRequest<Result<bool>>
{
    public AddBankDetailsRequest Request { get; }
    public AddBankDetailsCommand(AddBankDetailsRequest request)
    {
        Request = request;
    }
}

public sealed class AddDocumentPolicyStatusCommand : IRequest<Result<bool>>
{
    public AddDocumentPolicyStatusRequest Request { get; }
    public AddDocumentPolicyStatusCommand(AddDocumentPolicyStatusRequest request)
    {
        Request = request;
    }
}

public sealed class UpdateEmployeeCommand : IRequest<Result<bool>>
{
    public UpdateEmployeeRequest Request { get; }
    public UpdateEmployeeCommand(UpdateEmployeeRequest request)
    {
        Request = request;
    }
}

public sealed class AddProfileImageCommand : IRequest<Result<bool>>
{
    public AddProfileImageRequest Request { get; }
    public AddProfileImageCommand(AddProfileImageRequest request)
    {
        Request = request;
    }
}

public sealed class AddCVCommand : IRequest<Result<bool>>
{
    public AddCVRequest Request { get; }
    public AddCVCommand(AddCVRequest request)
    {
        Request = request;
    }
}

public sealed class AddStarterFormCommand : IRequest<Result<bool>>
{
    public AddStarterFormRequest Request { get; }
    public AddStarterFormCommand(AddStarterFormRequest request)
    {
        Request = request;
    }
}

public sealed class AddP45DocumentCommand : IRequest<Result<bool>>
{
    public AddP45DocumentRequest Request { get; }
    public AddP45DocumentCommand(AddP45DocumentRequest request)
    {
        Request = request;
    }
}

public sealed class AddBiometricResidenceCardCommand : IRequest<Result<bool>>
{
    public AddBiometricResidenceCardRequest Request { get; }
    public AddBiometricResidenceCardCommand(AddBiometricResidenceCardRequest request)
    {
        Request = request;
    }
}

public sealed class AddPassportCommand : IRequest<Result<bool>>
{
    public AddPassportRequest Request { get; }
    public AddPassportCommand(AddPassportRequest request)
    {
        Request = request;
    }
}

public sealed class DeleteBiometricResidenceCardCommand : IRequest<Result<bool>>
{
    public int EmployeeId { get; }
    public DeleteBiometricResidenceCardCommand(int employeeId)
    {
        EmployeeId = employeeId;
    }
}

public sealed class AddRightToWorkCommand : IRequest<Result<bool>>
{
    public AddRightToWorkRequest Request { get; }
    public AddRightToWorkCommand(AddRightToWorkRequest request)
    {
        Request = request;
    }
}

public sealed class DeletePassportCommand : IRequest<Result<bool>>
{
    public int EmployeeId { get; }
    public DeletePassportCommand(int employeeId)
    {
        EmployeeId = employeeId;
    }
}

public sealed class DeleteDbsCertificateCommand : IRequest<Result<bool>>
{
    public int EmployeeId { get; }
    public DeleteDbsCertificateCommand(int employeeId)
    {
        EmployeeId = employeeId;
    }
}

public sealed class DeleteAccessNICommand : IRequest<Result<bool>>
{
    public int EmployeeId { get; }
    public DeleteAccessNICommand(int employeeId)
    {
        EmployeeId = employeeId;
    }
}

public sealed class DeleteNationalInsuranceCommand : IRequest<Result<bool>>
{
    public int EmployeeId { get; }
    public DeleteNationalInsuranceCommand(int employeeId)
    {
        EmployeeId = employeeId;
    }
}

public sealed class SubmitTimesheetCommand : IRequest<Result<bool>>
{
    public SubmitTimesheetRequest Request { get; }
    public SubmitTimesheetCommand(SubmitTimesheetRequest request)
    {
        Request = request;
    }
}

public sealed class AddDbsDocumentsCommand : IRequest<Result<bool>>
{
    public AddDbsDocumentsRequest Request { get; }
    public AddDbsDocumentsCommand(AddDbsDocumentsRequest request)
    {
        Request = request;
    }
}

public sealed class AddToFavouriteCommand : IRequest<Result<bool>>
{
    public AddToFavouriteRequest Request { get; }
    public AddToFavouriteCommand(AddToFavouriteRequest request)
    {
        Request = request;
    }
}

public sealed class SendEmailForResetPasswordCommand : IRequest<Result<bool>>
{
    public string Email { get; }
    public byte UserTypeId { get; }
    public SendEmailForResetPasswordCommand(string email, byte userTypeId)
    {
        Email = email;
        UserTypeId = userTypeId;
    }
}

public sealed class ResetPasswordCommand : IRequest<Result<IEnumerable<string>>>
{
    public string Token { get; }
    public string Password { get; }
    public string ConfirmPassword { get; }
    public ResetPasswordCommand(string token, string password, string confirmPassword)
    {
        Token = token;
        Password = password;
        ConfirmPassword = confirmPassword;
    }
}

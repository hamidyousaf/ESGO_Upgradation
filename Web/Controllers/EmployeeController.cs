namespace Web.Controllers;

[Authorize(Roles =nameof(RoleEnum.Employee))]
public class EmployeeController(IMediator _mediator) : ApiBaseController
{
    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<IActionResult> RegisterAsync([FromForm] RegisterRequest request)
    {
        var result = await _mediator.Send(new RegisterCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
    {
        var result = await _mediator.Send(new LoginCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [AllowAnonymous]
    [HttpPost("Add")]
    public async Task<IActionResult> Add([FromForm] AddEmployeeRequest request)
    {
        var result = await _mediator.Send(new AddEmployeeCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [AllowAnonymous]
    [HttpGet("SignUpAPIs")]
    public async Task<IActionResult> SignUpAPIs()
    {
        var result = await _mediator.Send(new SignUpAPIsQuery());

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
    [HttpPost("AddAddressAsync")]
    public async Task<IActionResult> AddAddressAsync([FromBody] AddAddressRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new AddAddressCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    ///api/Employee/AddVaccination
    [AllowAnonymous]
    [HttpPost("AddVaccinationAsync")] // role employee.
    public async Task<IActionResult> AddVaccinationAsync([FromForm] AddVaccinationRequest request)
    {
        //request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new AddVaccinationQuery(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddShiftsAsync")]
    public async Task<IActionResult> AddShiftsAsync([FromBody] AddShiftsRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new AddShiftsQuery(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddNMCRegistrationAsync")]
    public async Task<IActionResult> AddNMCRegistrationAsync([FromBody] AddNMCRegistrationRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new AddNMCRegistrationCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddQualificationAsync")]
    public async Task<IActionResult> AddQualificationAsync([FromBody] AddQualificationRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new AddQualificationCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpDelete("DeleteQualificationAsync")]
    public async Task<IActionResult> DeleteQualificationAsync([FromQuery] DeleteQualificationRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new DeleteQualificationCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpDelete("DeleteDbsDocumentAsync")]
    public async Task<IActionResult> DeleteDbsDocumentAsync([FromQuery] DeleteDbsDocumentRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new DeleteDbsDocumentCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetQualificationsByEmployeeAsync")]
    public async Task<IActionResult> GetQualificationsByEmployeeAsync()
    {
        int employeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new GetQualificationsByEmployeeQuery(employeeId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddHaveQualificationAsync")]
    public async Task<IActionResult> AddHaveQualificationAsync([FromBody] AddHaveQualificationRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new AddHaveQualificationQuery(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddIsSubjectOfInvestigationAsync")]
    public async Task<IActionResult> AddIsSubjectOfInvestigationAsync([FromBody] AddIsSubjectOfInvestigationRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new AddIsSubjectOfInvestigationQuery(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddEmployementAsync")]
    public async Task<IActionResult> AddEmployementAsync([FromBody] AddEmployementRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new AddEmployementCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetEmployementsByEmployeeAsync")]
    public async Task<IActionResult> GetEmployementsByEmployeeAsync()
    {
        int employeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new GetEmployementsByEmployeeQuery(employeeId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpDelete("DeleteEmployementAsync")]
    public async Task<IActionResult> DeleteEmployementAsync([FromQuery] DeleteEmployementRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new DeleteEmployementCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddWorkGapReasonAsync")]
    public async Task<IActionResult> AddWorkGapReasonAsync([FromQuery] AddWorkGapReasonRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new AddWorkGapReasonCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("SendReferenceLinkToMailAsync")]
    public async Task<IActionResult> SendReferenceLinkToMailAsync([FromBody] SendReferenceLinkToMailRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new SendReferenceLinkToMailCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("CanAddPersonalReferenceAsync/{employeeSecretId}")]
    public async Task<IActionResult> CanAddPersonalReferenceAsync([Required] Guid employeeSecretId)
    {
        var result = await _mediator.Send(new CanAddPersonalReferenceQuery(employeeSecretId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("CanAddProfessionalReferenceAsync/{employeeSecretId}")]
    public async Task<IActionResult> CanAddProfessionalReferenceAsync([Required] Guid employeeSecretId)
    {
        var result = await _mediator.Send(new CanAddProfessionalReferenceQuery(employeeSecretId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [AllowAnonymous, HttpPost("AddPersonalReferenceAsync")]
    public async Task<IActionResult> AddPersonalReferenceAsync([FromBody] AddPersonalReferenceRequest request)
    {
        var result = await _mediator.Send(new AddPersonalReferenceCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [AllowAnonymous, HttpPost("AddProfessionalReferenceAsync")]
    public async Task<IActionResult> AddProfessionalReferenceAsync([FromBody] AddProfessionalReferenceRequest request)
    {
        var result = await _mediator.Send(new AddProfessionalReferenceCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [AllowAnonymous]
    [HttpGet("GetEmployeeForPersonalBySecretIdAsync/{employeeSecretId}")]
    public async Task<IActionResult> GetEmployeeForPersonalBySecretIdAsync([Required] Guid employeeSecretId)
    {
        var result = await _mediator.Send(new GetEmployeeForPersonalBySecretIdQuery(employeeSecretId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [AllowAnonymous]
    [HttpGet("GetEmployeeForProfessionalBySecretIdAsync/{employeeSecretId}")]
    public async Task<IActionResult> GetEmployeeForProfessionalBySecretIdAsync([Required] Guid employeeSecretId)
    {
        var result = await _mediator.Send(new GetEmployeeForProfessionalBySecretIdQuery(employeeSecretId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetEmployeeDocumentsAsync")]
    public async Task<IActionResult> GetEmployeeDocumentsAsync()
    {
        int employeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new GetEmployeeDocumentsQuery(employeeId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddEmployeeDocumentAsync")]
    public async Task<IActionResult> AddEmployeeDocumentAsync([FromForm] AddEmployeeDocumentRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new AddEmployeeDocumentCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpDelete("DeleteEmployeeDocumentAsync")]
    public async Task<IActionResult> DeleteEmployeeDocumentAsync([FromQuery] DeleteEmployeeDocumentRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new DeleteEmployeeDocumentCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddDbsCertificateAsync")]
    public async Task<IActionResult> AddDbsCertificateAsync([FromForm] AddDbsCertificateRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new AddDbsCertificateCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddNationalInsuranceAsync")]
    public async Task<IActionResult> AddNationalInsuranceAsync([FromForm] AddNationalInsuranceRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new AddNationalInsuranceCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddAccessNIAsync")]
    public async Task<IActionResult> AddAccessNIAsync([FromForm] AddAccessNIRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new AddAccessNICommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpDelete("DeleteAccessNIAsync")]
    public async Task<IActionResult> DeleteAccessNIAsync()
    {
        int employeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new DeleteAccessNICommand(employeeId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpDelete("DeleteNationalInsuranceAsync")]
    public async Task<IActionResult> DeleteNationalInsuranceAsync()
    {
        int employeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new DeleteNationalInsuranceCommand(employeeId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddRightToWorkAsync")]
    public async Task<IActionResult> AddRightToWorkAsync([FromForm] AddRightToWorkRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new AddRightToWorkCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddDbsNumberAsync")]
    public async Task<IActionResult> AddDbsNumberAsync([FromForm] AddDbsNumberRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new AddDbsNumberCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpDelete("DeleteDbsCertificateAsync")]
    public async Task<IActionResult> DeleteDbsCertificateAsync()
    {
        int employeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new DeleteDbsCertificateCommand(employeeId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddBankDetailsAsync")]
    public async Task<IActionResult> AddBankDetailsAsync([FromBody] AddBankDetailsRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new AddBankDetailsCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddDocumentPolicyStatusAsync")]
    public async Task<IActionResult> AddDocumentPolicyStatusAsync([FromBody] AddDocumentPolicyStatusRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new AddDocumentPolicyStatusCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetDocumentPolicyInfoAsync")]
    public async Task<IActionResult> GetDocumentPolicyInfoAsync()
    {
        int employeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new GetDocumentPolicyInfoQuery(employeeId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("UpdateEmployeeAsync")]
    public async Task<IActionResult> UpdateEmployeeAsync(UpdateEmployeeRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new UpdateEmployeeCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetEmployeeByIdAsync")]
    public async Task<IActionResult> GetEmployeeByIdAsync()
    {
        int employeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new GetEmployeeByIdQuery(employeeId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddProfileImageAsync")]
    public async Task<IActionResult> AddProfileImageAsync([FromForm] AddProfileImageRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new AddProfileImageCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddCVAsync")]
    public async Task<IActionResult> AddCVAsync([FromForm] AddCVRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new AddCVCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddStarterFormAsync")]
    public async Task<IActionResult> AddStarterFormAsync(AddStarterFormRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new AddStarterFormCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddP45DocumentAsync")]
    public async Task<IActionResult> AddP45DocumentAsync([FromForm] AddP45DocumentRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new AddP45DocumentCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddBiometricResidenceCardAsync")]
    public async Task<IActionResult> AddBiometricResidenceCardAsync([FromForm] AddBiometricResidenceCardRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new AddBiometricResidenceCardCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddPassportAsync")]
    public async Task<IActionResult> AddPassportAsync([FromForm] AddPassportRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new AddPassportCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpDelete("DeleteBiometricResidenceCardAsync")]
    public async Task<IActionResult> DeleteBiometricResidenceCardAsync()
    {
        int employeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new DeleteBiometricResidenceCardCommand(employeeId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpDelete("DeletePassportAsync")]
    public async Task<IActionResult> DeletePassportAsync()
    {
        int employeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new DeletePassportCommand(employeeId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetStarterFormAnswersAsync")]
    public async Task<IActionResult> GetStarterFormAnswersAsync()
    {
        int employeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new GetStarterFormAnswersQuery(employeeId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetAssignedJobsAsync")]
    public async Task<IActionResult> GetAssignedJobsAsync(int pageIndex, int pageSize, DateTime fromDate, decimal minRate = 0, byte radius = 0, string postalCode = "")
    {
        int employeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new GetAssignedJobsQuery(employeeId, pageIndex, pageSize, fromDate, minRate, radius, postalCode));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetConfirmedJobsAsync")]
    public async Task<IActionResult> GetConfirmedJobsAsync(int pageIndex, int pageSize, DateTime fromDate, decimal minRate = 0, byte radius = 0, string postalCode = "")
    {
        int employeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new GetConfirmedJobsQuery(employeeId, pageIndex, pageSize, fromDate, minRate, radius, postalCode));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetCompletedJobsAsync")]
    public async Task<IActionResult> GetCompletedJobsAsync(int pageIndex, int pageSize, DateTime fromDate, decimal minRate = 0, byte radius = 0, string postalCode = "")
    {
        int employeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new GetCompletedJobsByEmployeeQuery(employeeId, pageIndex, pageSize, fromDate, minRate, radius, postalCode));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetTimesheetsByStatusAsync")]
    public async Task<IActionResult> GetTimesheetsByStatusAsync(byte status, int pageIndex, int pageSize)
    {
        int employeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new GetTimesheetsByStatusQuery(employeeId, status, pageIndex, pageSize));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("ChangeJobStatusToConfirmAsync")]
    public async Task<IActionResult> ChangeJobStatusToConfirmAsync([Required] int jobId)
    {
        int employeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new ChangeJobStatusToConfirmQuery(employeeId, jobId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetAssignedJobByIdAsync")]
    public async Task<IActionResult> GetAssignedJobByIdAsync([Required] int assignedJobId)
    {
        int employeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new GetAssignedJobByIdQuery(employeeId, assignedJobId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetJobByIdAsync")]
    public async Task<IActionResult> GetJobByIdAsync([Required] int jobId)
    {
        int employeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new GetJobByIdQuery(employeeId, jobId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [AllowAnonymous, HttpGet("GetJobByIdForHomePageAsync")]
    public async Task<IActionResult> GetJobByIdForHomePageAsync([Required] int jobId)
    {
        var result = await _mediator.Send(new GetJobByIdForHomePageQuery(jobId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetTimesheetByIdAsync")]
    public async Task<IActionResult> GetTimesheetsByIdAsync([Required] int timesheetId)
    {
        int employeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new GetTimesheetByIdQuery(employeeId, timesheetId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("SubmitTimesheetAsync")]
    public async Task<IActionResult> SubmitTimesheetAsync([FromBody] SubmitTimesheetRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new SubmitTimesheetCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetDocumentTypesAsync")]
    public async Task<IActionResult> GetDocumentTypesAsync()
    {
        var result = await _mediator.Send(new GetDocumentTypesQuery());

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddDbsDocumentsAsync")]
    public async Task<IActionResult> AddDbsDocumentsAsync([FromForm] AddDbsDocumentsRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new AddDbsDocumentsCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetPendingJobCountsAsync")]
    public async Task<IActionResult> GetPendingJobCountsAsync()
    {
        int employeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new GetPendingJobCountsQuery(employeeId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetTimesheetCountsAsync")]
    public async Task<IActionResult> GetTimesheetCountsAsync()
    {
        int employeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new GetTimesheetCountsQuery(employeeId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetUrgentJobsAsync")]
    public async Task<IActionResult> GetUrgentJobsAsync(int pageIndex, int pageSize, DateTime fromDate, decimal minRate = 0, byte radius = 0, string postalCode = "")
    {
        int employeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new GetUrgentJobsQuery(employeeId, pageIndex, pageSize, fromDate, minRate, radius, postalCode));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetAllJobsAsync")]
    public async Task<IActionResult> GetAllJobsAsync(int pageIndex, int pageSize, DateTime fromDate, decimal minRate = 0, byte radius = 0, string postalCode = "")
    {
        int employeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new GetAllJobsQuery(employeeId, pageIndex, pageSize, fromDate, minRate, radius, postalCode));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetFavouriteJobsAsync")]
    public async Task<IActionResult> GetFavouriteJobsAsync(int pageIndex, int pageSize, DateTime fromDate, decimal minRate = 0, byte radius = 0, string postalCode = "")
    {
        int employeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new GetFavouriteJobsQuery(employeeId, pageIndex, pageSize, fromDate, minRate, radius, postalCode));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetAppliedJobsAsync")]
    public async Task<IActionResult> GetAppliedJobsAsync(int pageIndex, int pageSize, DateTime fromDate, decimal minRate = 0, byte radius = 0, string postalCode = "")
    {
        int employeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new GetAppliedJobsQuery(employeeId, pageIndex, pageSize, fromDate, minRate, radius, postalCode));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetJobCountsAsync")]
    public async Task<IActionResult> GetJobCountsAsync()
    {
        int employeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new GetJobCountsQuery(employeeId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("ApplyJobAsync")]
    public async Task<IActionResult> ApplyJobAsync([FromBody] ApplyJobRequest request)
    {
        int employeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new ApplyJobQuery(employeeId, request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetEmployeeAllDetailsByIdAsync")]
    public async Task<IActionResult> GetEmployeeAllDetailsByIdAsync()
    {
        int employeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new GetEmployeeAllDetailsByIdForEmployeeQuery(employeeId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("HomeJobSearchAsync")]
    public async Task<IActionResult> HomeJobSearchAsync([FromBody] HomeJobSearchRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new HomeJobSearchQuery(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("PendingJobSearchAsync")]
    public async Task<IActionResult> PendingJobSearchAsync([FromBody] PendingJobSearchRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new PendingJobSearchQuery(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("CompletedJobSearchAsync")]
    public async Task<IActionResult> CompletedJobSearchAsync([FromBody] CompletedJobSearchRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new CompletedJobSearchQuery(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [AllowAnonymous, HttpPost("GetOpenJobsForHomePageAsync")]
    public async Task<IActionResult> GetOpenJobsForHomePageAsync([FromBody] GetOpenJobsForHomePageRequest request)
    {
        var result = await _mediator.Send(new GetOpenJobsForHomePageQuery(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [AllowAnonymous, HttpPost("WebGuestJobSearchAsync")]
    public async Task<IActionResult> WebGuestJobSearchAsync([FromBody] WebGuestJobSearchRequest request)
    {
        var result = await _mediator.Send(new WebGuestJobSearchQuery(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("GetCalenderJobsAsync")]
    public async Task<IActionResult> GetCalenderJobsAsync([FromBody] GetCalenderJobsRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new GetCalenderJobsQuery(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddToFavouriteAsync")]
    public async Task<IActionResult> AddToFavouriteAsync([FromBody] AddToFavouriteRequest request)
    {
        request.EmployeeId = User.GetEmployeeId();
        var result = await _mediator.Send(new AddToFavouriteCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [AllowAnonymous, HttpPost("EmailVerificationAsync")]
    public async Task<IActionResult> EmailVerificationAsync([FromQuery] string token)
    {
        var result = await _mediator.Send(new EmailVerificationQuery(token));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [AllowAnonymous, HttpGet("GetAssignedJobByEncryptIdAsync")]
    public async Task<IActionResult> GetAssignedJobByEncryptIdAsync([FromQuery] string assignedJobId)
    {
        var result = await _mediator.Send(new GetAssignedJobByEncryptIdQuery(assignedJobId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [AllowAnonymous, HttpPost("ChangeJobStatusToConfirmWithEmailAsync")]
    public async Task<IActionResult> ChangeJobStatusToConfirmWithEmailAsync([Required] string assignedJobId)
    {
        var result = await _mediator.Send(new ChangeJobStatusToConfirmWithEmailQuery(assignedJobId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [AllowAnonymous, HttpPost("SendEmailForResetPasswordAsync")]
    public async Task<IActionResult> SendEmailForResetPasswordAsync([Required, EmailAddress] string email)
    {
        var userTypeId = (byte)UserTypeEnum.Employee;
        var result = await _mediator.Send(new SendEmailForResetPasswordCommand(email, userTypeId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [AllowAnonymous, HttpPost("ResetPasswordAsync")]
    public async Task<IActionResult> ResetPasswordAsync(
        [Required] string token, 
        [Required, StringLength(50, MinimumLength = 5)] string password, 
        [Required, StringLength(50, MinimumLength = 5)] string confirmPassword)
    {
        var result = await _mediator.Send(new ResetPasswordCommand(token, password, confirmPassword));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}
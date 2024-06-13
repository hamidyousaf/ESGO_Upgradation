using Aspose.Pdf;

namespace Web.Controllers;

[Authorize(Roles = nameof(RoleEnum.SuperAdmin))]
public class AdminController(IMediator _mediator) : ApiBaseController // primary constructor is mutable mean change able. it is act as parameter of the class. It is not the part of class member.
{
    [HttpGet("GetAllEmployeesByStatusAsync")]
    public async Task<IActionResult> GetAllEmployeesByStatusAsync(byte status, int pageIndex, int pageSize)
    {
        var result = await _mediator.Send(new GetAllEmployeesByStatusQuery(status, pageIndex, pageSize));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("ChangeEmployeeStatusAsync")]
    public async Task<IActionResult> ChangeEmployeeStatusAsync([FromBody] ChangeEmployeeStatusRequest request)
    {
        var result = await _mediator.Send(new ChangeEmployeeStatusQuery(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetEmployeeAllDetailsByIdAsync")]
    public async Task<IActionResult> GetEmployeeAllDetailsByIdAsync(int employeeId)
    {
        var result = await _mediator.Send(new GetEmployeeAllDetailsByIdQuery(employeeId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetEmployeeShortDetailsByIdAsync")]
    public async Task<IActionResult> GetEmployeeShortDetailsByIdAsync(int employeeId)
    {
        var result = await _mediator.Send(new GetEmployeeShortDetailsByIdQuery(employeeId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("DeleteEmployeesByIdsAsync")]
    public async Task<IActionResult> DeleteEmployeesByIdsAsync([FromBody] DeleteEmployeesByIdsRequest request)
    {
        var result = await _mediator.Send(new DeleteEmployeesByIdsCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("DeleteEmployersByIdsAsync")]
    public async Task<IActionResult> DeleteEmployersByIdsAsync([FromBody] DeleteEmployersByIdsRequest request)
    {
        var result = await _mediator.Send(new DeleteEmployersByIdsCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetEmployerAllDetailsByIdAsync")]
    public async Task<IActionResult> GetEmployerAllDetailsByIdAsync(int employerId)
    {
        var result = await _mediator.Send(new GetEmployerAllDetailsByIdQuery(employerId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddShadowShiftAsync")]
    public async Task<IActionResult> AddShadowShiftAsync([FromForm]AddShadowShiftRequest request)
    {
        var result = await _mediator.Send(new AddShadowShiftCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddInterviewAsync")]
    public async Task<IActionResult> AddInterviewAsync([FromForm] AddInterviewRequest request)
    {
        var result = await _mediator.Send(new AddInterviewCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddMonthlySupervisionReportAsync")]
    public async Task<IActionResult> AddMonthlySupervisionReportAsync([FromForm] AddMonthlySupervisionReportRequest request)
    {
        var result = await _mediator.Send(new AddMonthlySupervisionReportCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("ChangeInterviewStatusAsync")]
    public async Task<IActionResult> ChangeInterviewStatusAsync([FromForm] ChangeInterviewStatusRequest request)
    {
        var result = await _mediator.Send(new ChangeInterviewStatusCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddFeedbackAsync")]
    public async Task<IActionResult> AddFeedbackAsync([FromBody] AddFeedbackRequest request)
    {
        var result = await _mediator.Send(new AddFeedbackCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetShadowShiftsAsync")]
    public async Task<IActionResult> GetShadowShiftsAsync(int employeeId)
    {
        var result = await _mediator.Send(new GetShadowShiftsQuery(employeeId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetMonthlySupervisionReportsAsync")]
    public async Task<IActionResult> GetMonthlySupervisionReportsAsync(int employeeId)
    {
        var result = await _mediator.Send(new GetMonthlySupervisionReportsQuery(employeeId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetFeedbacksAsync")]
    public async Task<IActionResult> GetFeedbacksAsync(int employeeId)
    {
        var result = await _mediator.Send(new GetFeedbacksQuery(employeeId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetDbsExpiredEmployeesAsync")]
    public async Task<IActionResult> GetDbsExpiredEmployeesAsync(int pageIndex, int pageSize)
    {
        var result = await _mediator.Send(new GetDbsExpiredEmployeesQuery(pageIndex, pageSize));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetTrainingCertificateExpiredEmployeesAsync")]
    public async Task<IActionResult> GetTrainingCertificateExpiredEmployeesAsync(int pageIndex, int pageSize)
    {
        var result = await _mediator.Send(new GetTrainingCertificateExpiredEmployeesQuery(pageIndex, pageSize));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetAllDocumentsByEmployeeAsync")]
    public async Task<IActionResult> GetAllDocumentsByEmployeeAsync([FromQuery][Required] int employeeId)
    {
        var result = await _mediator.Send(new GetAllDocumentsByEmployeeQuery(employeeId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("ChangeEmployeeDocumentStatusAsync")]
    public async Task<IActionResult> ChangeEmployeeDocumentStatusAsync([FromBody] ChangeEmployeeDocumentStatusRequest request)
    {
        var result = await _mediator.Send(new ChangeEmployeeDocumentStatusQuery(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetAllEmployersByStatusAsync")]
    public async Task<IActionResult> GetAllEmployersByStatusAsync(byte status, int pageIndex, int pageSize)
    {
        var result = await _mediator.Send(new GetAllEmployersByStatusQuery(status, pageIndex, pageSize));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("ChangeEmployerStatusAsync")]
    public async Task<IActionResult> ChangeEmployerStatusAsync(ChangeEmployerStatusRequest request)
    {
        var result = await _mediator.Send(new ChangeEmployerStatusQuery(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddJobCommissionAsync")]
    public async Task<IActionResult> AddJobCommissionAsync([FromBody] AddJobCommissionRequest request)
    {
        var result = await _mediator.Send(new AddJobCommissionCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetAllBookingsAsync")]
    public async Task<IActionResult> GetAllBookingsAsync(int pageIndex, int pageSize)
    {
        var result = await _mediator.Send(new GetAllBookingsQuery(pageSize: pageSize, pageIndex: pageIndex));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("ChangeBookingStatusAsync")]
    public async Task<IActionResult> ChangeJobBookingStatusAsync(ChangeBookingStatusRequest request)
    {
        var result = await _mediator.Send(new ChangeBookingStatusQuery(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    ///api/Category/GetAllCategoryAsync
    [AllowAnonymous]
    [HttpGet("GetEmployeeTypesAsync")]
    public async Task<IActionResult> GetEmployeeTypesAsync()
    {
        var result = await _mediator.Send(new GetEmployeeTypesQuery());

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    ///api/Category/GetCategoryByIDAsync
    [AllowAnonymous]
    [HttpGet("GetEmployeeTypeByIdAsync")]
    public async Task<IActionResult> GetEmployeeTypeByIdAsync(byte emplyeeTypeId)
    {
        var result = await _mediator.Send(new GetEmployeeTypeByIdQuery(emplyeeTypeId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    ///api/Employee/GetAllEmployeeDetailsByCategoryId
    [AllowAnonymous]
    [HttpGet("GetEmployeesByEmployeeTypeIdAsync")]
    public async Task<IActionResult> GetEmployeesByEmployeeTypeIdAsync([Required(ErrorMessage = "Emplyee type id is required")] byte emplyeeTypeId)
    {
        var result = await _mediator.Send(new GetEmployeesByEmployeeTypeIdQuery(emplyeeTypeId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    ///api/Shift/GetAllShiftByUserId
    [AllowAnonymous]
    [HttpGet("GetShiftsByEmployerIdAsync")]
    public async Task<IActionResult> GetShiftsByEmployerIdAsync([Required(ErrorMessage = "Emplyer id is required")] int employerId)
    {
        var result = await _mediator.Send(new GetShiftsByEmployerIdQuery(employerId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddJobAsync")]
    public async Task<IActionResult> AddJobAsync([FromBody] AddJobRequest request)
    {
        var result = await _mediator.Send(new AddJobCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddMultilpleJobAsync")]
    public async Task<IActionResult> AddMultilpleJobAsync([FromBody] List<AddJobRequest> request)
    {
        var result = await _mediator.Send(new AddMultilpleJobCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetJobsByStatusAsync")]
    public async Task<IActionResult> GetJobsByStatusAsync(byte status, int pageIndex, int pageSize)
    {
        var result = await _mediator.Send(new GetJobsByStatusQuery(status, pageIndex, pageSize));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetUnsuccessfulJobsAsync")] // not using API.
    public async Task<IActionResult> GetUnsuccessfulJobsAsync(int pageIndex, int pageSize)
    {
        var result = await _mediator.Send(new GetUnsuccessfulJobsQuery(pageIndex, pageSize));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetConfirmedJobsAsync")] // not using API.
    public async Task<IActionResult> GetConfirmedJobsAsync(int pageIndex, int pageSize)
    {
        var result = await _mediator.Send(new GetConfirmedJobsForAdminQuery(pageIndex, pageSize));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetCompletedJobsAsync")] // not using API.
    public async Task<IActionResult> GetCompletedJobsAsync(int pageIndex, int pageSize)
    {
        var result = await _mediator.Send(new GetCompletedJobsQuery(pageIndex, pageSize));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("ChangeUTRNumberStatusAsync")]
    public async Task<IActionResult> ChangeUTRNumberStatusAsync(ChangeUTRNumberStatusRequest request)
    {
        var result = await _mediator.Send(new ChangeUTRNumberStatusCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("ChangeCompanyNumberStatusAsync")]
    public async Task<IActionResult> ChangeCompanyNumberStatusAsync(ChangeCompanyNumberStatusRequest request)
    {
        var result = await _mediator.Send(new ChangeCompanyNumberStatusCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("ChangeNMCRegistrationStatusAsync")]
    public async Task<IActionResult> ChangeNMCRegistrationStatusAsync(ChangeNMCRegistrationStatusRequest request)
    {
        var result = await _mediator.Send(new ChangeNMCRegistrationStatusCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("ChangeNationalInsuranceNumberStatusAsync")]
    public async Task<IActionResult> ChangeNationalInsuranceNumberStatusAsync(ChangeNationalInsuranceNumberStatusRequest request)
    {
        var result = await _mediator.Send(new ChangeNationalInsuranceNumberStatusCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("ChangePersonalReferenceStatusAsync")]
    public async Task<IActionResult> ChangePersonalReferenceStatusAsync(ChangePersonalReferenceStatusRequest request)
    {
        var result = await _mediator.Send(new ChangePersonalReferenceStatusCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("ChangeProfessionalReferenceStatusAsync")]
    public async Task<IActionResult> ChangeProfessionalReferenceStatusAsync(ChangeProfessionalReferenceStatusRequest request)
    {
        var result = await _mediator.Send(new ChangeProfessionalReferenceStatusCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("ChangeBiometricResidenceCardStatusAsync")]
    public async Task<IActionResult> ChangeBiometricResidenceCardStatusAsync(ChangeBiometricResidenceCardStatusRequest request)
    {
        var result = await _mediator.Send(new ChangeBiometricResidenceCardStatusCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("ChangePassportStatusAsync")]
    public async Task<IActionResult> ChangePassportStatusAsync(ChangePassportStatusRequest request)
    {
        var result = await _mediator.Send(new ChangePassportStatusCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("ChangeDbsDocumentStatusAsync")]
    public async Task<IActionResult> ChangeDbsDocumentStatusAsync(ChangeDbsDocumentStatusRequest request)
    {
        var result = await _mediator.Send(new ChangeDbsDocumentStatusCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("ChangeDbsNumebrStatusAsync")]
    public async Task<IActionResult> ChangeDbsNumebrStatusAsync(ChangeDbsNumebrStatusRequest request)
    {
        var result = await _mediator.Send(new ChangeDbsNumebrStatusCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("ChangeDbsCertificateStatusAsync")]
    public async Task<IActionResult> ChangeDbsCertificateStatusAsync(ChangeDbsCertificateStatusRequest request)
    {
        var result = await _mediator.Send(new ChangeDbsCertificateStatusCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("ChangeAccessNIStatusAsync")]
    public async Task<IActionResult> ChangeAccessNIStatusAsync(ChangeAccessNIStatusRequest request)
    {
        var result = await _mediator.Send(new ChangeAccessNIStatusCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("ChangeNationalInsuranceStatusAsync")]
    public async Task<IActionResult> ChangeNationalInsuranceStatusAsync(ChangeNationalInsuranceStatusRequest request)
    {
        var result = await _mediator.Send(new ChangeNationalInsuranceStatusCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpDelete("DeletePersonalReferenceAsync")]
    public async Task<IActionResult> DeletePersonalReferenceAsync([Required] int empolyeeId)
    {
        var result = await _mediator.Send(new DeletePersonalReferenceCommand(empolyeeId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpDelete("DeleteProfessionalReferenceAsync")]
    public async Task<IActionResult> DeleteProfessionalReferenceAsync([Required] int empolyeeId)
    {
        var result = await _mediator.Send(new DeleteProfessionalReferenceCommand(empolyeeId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetAssignedJobEmployeeByIdAsync")]
    public async Task<IActionResult> GetAssignedJobEmployeeByIdAsync([Required] int assignedJobId)
    {
        var result = await _mediator.Send(new GetAssignedJobEmployeeByIdForAdminQuery(assignedJobId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetEmployersForInvoiceAsync")]
    public async Task<IActionResult> GetEmployersForInvoiceAsync()
    {
        var result = await _mediator.Send(new GetEmployersForInvoiceQuery());

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("GetTotalAmountOfEmployerAsync")]
    public async Task<IActionResult> GetTotalAmountOfEmployerAsync(GetTotalAmountOfEmployerRequest request)
    {
        var result = await _mediator.Send(new GetTotalAmountOfEmployerQuery(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddInvoiceAsync")]
    public async Task<IActionResult> AddInvoiceAsync([FromBody] AddInvoiceRequest request)
    {
        var result = await _mediator.Send(new AddInvoiceCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetNextInvoiceNumberAsync")]
    public async Task<IActionResult> GetNextInvoiceNumberAsync()
    {
        var result = await _mediator.Send(new GetNextInvoiceNumberQuery());

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [AllowAnonymous, HttpGet("GetEmployeeProfileAsPdfAsync")]
    public async Task<IActionResult> GetEmployeeProfileAsPdfAsync(int employeeId)
    {
        var result = await _mediator.Send(new GetEmployeeProfileAsPdfQuery(employeeId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("UpdateEmployeeDetailsAsync")]
    public async Task<IActionResult> UpdateEmployeeDetailsAsync([FromBody] UpdateEmployeeDetailsRequest request)
    {
        var result = await _mediator.Send(new UpdateEmployeeDetailsCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetAPIsForJobAsync")]
    public async Task<IActionResult> GetAPIsForJobAsync([FromQuery, Required] int bookingId)
    {
        var result = await _mediator.Send(new GetAPIsForJobQuery(bookingId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetEmployeesByEmployeeTypeIdAndEmployeeCategoryIdAsync")]
    public async Task<IActionResult> GetEmployeesByEmployeeTypeIdAndEmployeeCategoryIdAsync(byte employeeTypeId, byte employeeCategoryId)
    {
        var result = await _mediator.Send(new GetEmployeesByEmployeeTypeIdAndEmployeeCategoryIdQuery(employeeTypeId, employeeCategoryId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetDashboardDetailsAsync")]
    public async Task<IActionResult> GetDashboardDetailsAsync()
    {
        var result = await _mediator.Send(new GetDashboardDetailsQuery());

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetTimesheetsByStatusAsync")]
    public async Task<IActionResult> GetTimesheetsByStatusAsync([Required] byte status, [Required] int pageIndex, [Required] int pageSize)
    {
        var result = await _mediator.Send(new GetTimesheetsByStatusForAdminQuery(status, pageIndex, pageSize));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetTimesheetByIdAsync")]
    public async Task<IActionResult> GetTimesheetByIdAsync([Required] int timesheetId)
    {
        var result = await _mediator.Send(new GetTimesheetByIdForAdminQuery(timesheetId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("UpdateTimesheetByIdAsync")]
    public async Task<IActionResult> UpdateTimesheetByIdAsync([FromBody] UpdateTimesheetByIdRequest request)
    {
        var result = await _mediator.Send(new UpdateTimesheetByIdCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetAssignedJobByIdAsync")]
    public async Task<IActionResult> GetAssignedJobByIdAsync([Required, FromQuery] int assignedJobId)
    {
        var result = await _mediator.Send(new GetAssignedJobByIdForAdminQuery(assignedJobId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetEmployeesByJobIdAsync")]
    public async Task<IActionResult> GetEmployeesByJobIdAsync([Required, FromQuery] int jobId)
    {
        var result = await _mediator.Send(new GetEmployeesByJobIdQuery(jobId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetEmployeeByAssignedJobIdAsync")]
    public async Task<IActionResult> GetEmployeeByAssignedJobIdAsync([Required, FromQuery] int assignedJobId)
    {
        var result = await _mediator.Send(new GetEmployeeByAssignedJobIdQuery(assignedJobId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetJobHistoryByEmployeeIdAsync")]
    public async Task<IActionResult> GetJobHistoryByEmployeeIdAsync([Required, FromQuery] int employeeId, int pageIndex, int pageSize)
    {
        var result = await _mediator.Send(new GetJobHistoryByEmployeeIdQuery(employeeId, pageIndex, pageSize));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetEmployeeJobDetailsByIdAsync")]
    public async Task<IActionResult> GetEmployeeJobDetailsByIdAsync([Required, FromQuery] int assignedJobId)
    {
        var result = await _mediator.Send(new GetEmployeeJobDetailsByIdQuery(assignedJobId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetEmployerJobsAsync")]
    public async Task<IActionResult> GetEmployerJobsAsync([Required, FromQuery] int employerId, string tabName, int pageIndex, int pageSize)
    {
        var result = await _mediator.Send(new GetEmployerJobsQuery(employerId, tabName, pageIndex, pageSize));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [AllowAnonymous]
    [HttpPost("EmployeeLoginAsync")]
    public async Task<IActionResult> EmployeeLoginAsync([FromBody] EmployeeLoginRequest request)
    {
        var result = await _mediator.Send(new EmployeeLoginCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [AllowAnonymous]
    [HttpPost("EmployerLoginAsync")]
    public async Task<IActionResult> EmployerLoginAsync([FromBody] EmployerLoginRequest request)
    {
        var result = await _mediator.Send(new EmployerLoginForAdminCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetNotifications")]
    public async Task<IActionResult> GetNotifications(string type = "All", int skip = 0)
    {
        var result = await _mediator.Send(new GetNotificationsQuery(type, skip));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("MarkAsReadNotification/{notificationId}")]
    public async Task<IActionResult> MarkAsReadNotification(int notificationId)
    {
        var result = await _mediator.Send(new MarkAsReadNotificationCommand(notificationId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("MarkAllAsReadNotifications")]
    public async Task<IActionResult> MarkAllAsReadNotifications()
    {
        var result = await _mediator.Send(new MarkAllAsReadNotificationsCommand());

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}

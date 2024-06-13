using Domain.DTOs.Requests;
using Domain.Entities;

namespace Web.Controllers;

[Authorize(Roles =nameof(RoleEnum.Employer))]
public class EmployerController(IMediator _mediator) : ApiBaseController
{
    [AllowAnonymous]
    [HttpPost("RegisterAsync")]
    public async Task<IActionResult> RegisterAsync([FromBody] EmployerRegisterRequest request)
    {
        var result = await _mediator.Send(new EmployerRegisterCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [AllowAnonymous]
    [HttpPost("LoginAsync")]
    public async Task<IActionResult> LoginAsync([FromBody] EmployerLoginRequest request)
    {
        var result = await _mediator.Send(new EmployerLoginCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddBookingAsync")]
    public async Task<IActionResult> AddJobBookingAsync([FromForm] AddBookingRequest request)
    {
        request.EmployerId = User.GetEmployerId();
        var result = await _mediator.Send(new AddBookingCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetBookingsAsync")]
    public async Task<IActionResult> GetBookingsAsync(int pageIndex, int pageSize)
    {
        int employerId = User.GetEmployerId();
        var result = await _mediator.Send(new GetBookingsQuery(employerId, pageIndex, pageSize));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPatch("UpdateBookingAsync")]
    public async Task<IActionResult> UpdateBookingAsync([FromForm] UpdateBookingRequest request)
    {
        request.EmployerId = User.GetEmployerId();
        var result = await _mediator.Send(new UpdateBookingCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddShiftAsync")]
    public async Task<IActionResult> AddShiftAsync(AddShiftRequest request)
    {
        request.EmployerId = User.GetEmployerId();
        var result = await _mediator.Send(new AddShiftCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetShiftsAsync")]
    public async Task<IActionResult> GetShiftsAsync(int pageIndex, int pageSize)
    { 
        int employerId = User.GetEmployerId();
        var result = await _mediator.Send(new GetShiftsQuery(employerId, pageIndex, pageSize));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetTimesheetsByStatusAsync")]
    public async Task<IActionResult> GetTimesheetsByStatusAsync([Required] byte status, [Required] int pageIndex, [Required] int pageSize)
    {
        int employerId = User.GetEmployerId();
        var result = await _mediator.Send(new GetTimesheetsByStatusForEmployerQuery(employerId, status, pageIndex, pageSize));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetShiftByIdAsync")]
    public async Task<IActionResult> GetShiftByIdAsync([Required(ErrorMessage = "Shift id is required.")] int shiftId)
    {
        int employerId = User.GetEmployerId();
        var result = await _mediator.Send(new GetShiftByIdQuery(employerId, shiftId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    ///api/Shift/UpdateShiftAsync
    [HttpPost("UpdateShiftAsync")]
    public async Task<IActionResult> UpdateShiftAsync(UpdateShiftRequest request)
    {
        request.EmployerId = User.GetEmployerId();
        var result = await _mediator.Send(new UpdateShiftCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpDelete("DeleteShiftAsync")]
    public async Task<IActionResult> DeleteShiftAsync([FromQuery] int shiftId)
    {
        DeleteShiftRequest request = new DeleteShiftRequest();
        request.EmployerId = User.GetEmployerId();
        request.ShiftId = shiftId;
        var result = await _mediator.Send(new DeleteShiftCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpDelete("DeleteBookingAsync")]
    public async Task<IActionResult> DeleteBookingAsync([FromQuery] int bookingId)
    {
        DeleteBookingRequest request = new DeleteBookingRequest();
        request.EmployerId = User.GetEmployerId();
        request.BookingId = bookingId;
        var result = await _mediator.Send(new DeleteBookingCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("ChangeTimesheetStatusAsync")]
    public async Task<IActionResult> ChangeTimesheetStatusAsync([FromBody] ChangeTimesheetStatusRequest request)
    {
        request.EmployerId = User.GetEmployerId();
        var result = await _mediator.Send(new ChangeTimesheetStatusCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetNextBookingIdAsync")]
    public async Task<IActionResult> GetNextBookingIdAsync()
    {
        var result = await _mediator.Send(new GetNextBookingIdQuery());

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddOrganisationImageAsync")]
    public async Task<IActionResult> AddOrganisationImageAsync([FromForm] AddOrganisationImageRequest request)
    {
        request.EmployerId = User.GetEmployerId();
        var result = await _mediator.Send(new AddOrganisationImageCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddOrganisationLogoAsync")]
    public async Task<IActionResult> AddOrganisationLogoAsync([FromForm] AddOrganisationLogoRequest request)
    {
        request.EmployerId = User.GetEmployerId();
        var result = await _mediator.Send(new AddOrganisationLogoCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetEmployerByIdAsync")]
    public async Task<IActionResult> GetEmployerByIdAsync()
    {
        int employerId = User.GetEmployerId();
        var result = await _mediator.Send(new GetEmployerByIdQuery(employerId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("UpdateEmployerAsync")]
    public async Task<IActionResult> UpdateEmployerAsync([FromBody] UpdateEmployerRequest request)
    {
        request.EmployerId = User.GetEmployerId();
        var result = await _mediator.Send(new UpdateEmployerCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetTimesheetByIdAsync")]
    public async Task<IActionResult> GetTimesheetByIdAsync([Required] int timesheetId)
    {
        int employerId = User.GetEmployerId();
        var result = await _mediator.Send(new GetTimesheetByIdForEmployerQuery(employerId, timesheetId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetJobsByStatusAsync")]
    public async Task<IActionResult> GetJobsByStatusAsync(byte status, int pageIndex, int pageSize)
    {
        var employerId = User.GetEmployerId();
        var result = await _mediator.Send(new GetJobsByStatusForEmployerQuery(employerId, status, pageIndex, pageSize));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddJobAsync")]
    public async Task<IActionResult> AddJobAsync([FromBody] AddJobRequest request)
    {
        request.EmployerId = User.GetEmployerId();
        var result = await _mediator.Send(new AddJobCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetAPIsForJobAsync")]
    public async Task<IActionResult> GetAPIsForJobAsync()
    {
        var employerId = User.GetEmployerId();
        var result = await _mediator.Send(new GetAPIsForJobForEmployerQuery(employerId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetShiftsForJobAsync")]
    public async Task<IActionResult> GetShiftsForJobAsync()
    {
        var employerId = User.GetEmployerId();
        var result = await _mediator.Send(new GetShiftsForJobQuery(employerId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("UpdateJobAsync")]
    public async Task<IActionResult> UpdateJobAsync([FromBody] UpdateJobRequest request)
    {
        request.EmployerId = User.GetEmployerId();
        var result = await _mediator.Send(new UpdateJobCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("DeleteJobAsync")]
    public async Task<IActionResult> DeleteJobAsync([FromBody] DeleteJobRequest request)
    {
        request.EmployerId = User.GetEmployerId();
        var result = await _mediator.Send(new DeleteJobCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetJobByIdAsync")]
    public async Task<IActionResult> GetJobByIdAsync(int jobId)
    {
        int employerId = User.GetEmployerId();
        var result = await _mediator.Send(new GetJobByIdForEmployerQuery(employerId, jobId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("RemoveEmployeeFromAssignedJobByIdAsync")]
    public async Task<IActionResult> RemoveEmployeeFromAssignedJobByIdAsync([Required]int assignedJobId)
    {
        int employerId = User.GetEmployerId();
        var result = await _mediator.Send(new RemoveEmployeeFromAssignedJobByIdQuery(employerId, assignedJobId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetApplicantsByJobIdAsync")]
    public async Task<IActionResult> GetApplicantsByJobIdAsync([Required]int jobId)
    {
        int employerId = User.GetEmployerId();
        var result = await _mediator.Send(new GetApplicantsByJobIdQuery(employerId, jobId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("SelectEmployeeByAssignedJobIdAsync")]
    public async Task<IActionResult> SelectEmployeeByAssignedJobIdAsync([FromBody] SelectEmployeeByAssignedJobIdRequest request)
    {
        request.EmployerId = User.GetEmployerId();
        var result = await _mediator.Send(new SelectEmployeeByAssignedJobIdQuery(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetEmployeeDetailsByIdAsync")]
    public async Task<IActionResult> GetEmployeeDetailsByIdAsync([Required] int employeeId)
    {
        int employerId = User.GetEmployerId();
        var result = await _mediator.Send(new GetEmployeeDetailsByIdQuery(employeeId, employerId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("AddToFavouriteAsync")]
    public async Task<IActionResult> AddToFavouriteAsync([FromBody] AddToFavouriteForEmployerRequest request)
    {
        request.EmployerId = User.GetEmployerId();
        var result = await _mediator.Send(new AddToFavouriteForEmployerCommand(request));

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

    [HttpDelete("DeleteFeedbackAsync")]
    public async Task<IActionResult> DeleteFeedbackAsync(int feedbackId)
    {
        var result = await _mediator.Send(new DeleteFeedbackCommand(feedbackId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("UpdateFeedbackAsync")]
    public async Task<IActionResult> UpdateFeedbackAsync([FromBody] UpdateFeedbackRequest request)
    {
        var result = await _mediator.Send(new UpdateFeedbackCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetEmployeesWorkedUnderEmployerAsync")]
    public async Task<IActionResult> GetEmployeesWorkedUnderEmployerAsync()
    {
        int employerId = User.GetEmployerId();
        var result = await _mediator.Send(new GetEmployeesWorkedUnderEmployerQuery(employerId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("GetFavouriteEmployeesAsync")]
    public async Task<IActionResult> GetFavouriteEmployeesAsync()
    {
        int employerId = User.GetEmployerId();
        var result = await _mediator.Send(new GetFavouriteEmployeesQuery(employerId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [AllowAnonymous, HttpPost("SendEmailForResetPasswordAsync")]
    public async Task<IActionResult> SendEmailForResetPasswordAsync([Required, EmailAddress] string email)
    {
        var userTypeId = (byte)UserTypeEnum.Employer;
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

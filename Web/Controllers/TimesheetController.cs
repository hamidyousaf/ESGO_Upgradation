namespace Web.Controllers;

public class TimesheetController(IMediator _mediator) : ApiBaseController
{
    [AllowAnonymous]
    [HttpPost("AddTimesheetAsync")]
    public async Task<IActionResult> AddTimesheetAsync([FromForm] AddEmployeeRequest request)
    {
        var result = await _mediator.Send(new AddEmployeeCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
    [AllowAnonymous]
    [HttpDelete("DeleteTimesheetAsync")]
    public async Task<IActionResult> DeleteTimesheetAsync([FromQuery] DeleteEmployementRequest request)
    {
        request.EmployeeId = 3 /*User.GetEmployeeId()*/;
        var result = await _mediator.Send(new DeleteEmployementCommand(request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
    [AllowAnonymous]
    [HttpPost("GetTimesheetsAsync")]
    public async Task<IActionResult> GetTimesheetsAsync(int pageIndex, int pageSize)
    {
        int employerId = 3; /*User.GetEmployerId();*/
        var result = await _mediator.Send(new GetTimesheetsQuery(employerId, pageIndex, pageSize));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [AllowAnonymous]
    [HttpPost("UpdateTimesheetAsync")]
    public async Task<IActionResult> UpdateTimesheetAsync(UpdateTimeSheetRequest request)
    {
        int employerId = 3; /*User.GetEmployerId();*/
        var result = await _mediator.Send(new UpdateTimesheatCommand(employerId, request));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [AllowAnonymous]
    [HttpGet("GetTimesheetByJobIdAsync")]
    public async Task<IActionResult> GetTimesheetByJobIdAsync([Required] int jobId)
    {
        int employerId = 3; /*User.GetEmployerId();*/
        var result = await _mediator.Send(new GetTimesheetByJobIdQuery(jobId, employerId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [AllowAnonymous]
    [HttpGet("GetApprovedTimeSheetsByJobIdAsync")]
    public async Task<IActionResult> GetApprovedTimeSheetsByJobIdAsync([Required] int jobId,int pageIndex, int pageSize)
    {
        int employerId = 3; /*User.GetEmployerId();*/
        var result = await _mediator.Send(new GetApprovedTimeSheetsByJobIdQuery(jobId, employerId, pageIndex, pageSize));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [AllowAnonymous]
    [HttpGet("GetRejectedTimeSheetByJobIdAsync")]
    public async Task<IActionResult> GetRejectedTimeSheetByJobIdAsync([Required] int jobId,int pageIndex, int pageSize)
    {
        int employerId = 3; /*User.GetEmployerId();*/
        var result = await _mediator.Send(new GetRejectedTimeSheetByJobIdQuery(jobId, employerId, pageIndex, pageSize));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [AllowAnonymous]
    [HttpGet("GetPendingTimeSheetByJobIdAsync")]
    public async Task<IActionResult> GetPendingTimeSheetByJobIdAsync([Required] int jobId,int pageIndex, int pageSize)
    {
        int employerId = 3; /*User.GetEmployerId();*/
        var result = await _mediator.Send(new GetPendingTimeSheetByJobIdQuery(jobId, employerId, pageIndex, pageSize));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [AllowAnonymous]
    [HttpPost("ChangeTimeSheetVerificationAsync")]
    public async Task<IActionResult> ChangeTimeSheetVerificationAsync([Required] int timesheetId, [Required] byte status)
    {
        var result = await _mediator.Send(new ChangeTimeSheetVerificationCommand(timesheetId, status));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [AllowAnonymous]
    [HttpGet("GetEmployeeListByPaymentStatusAsync")]
    public async Task<IActionResult> GetEmployeeListByPaymentStatusAsync(GetEmployeeListByPaymentStatusRequest request, [Required] int pageIndex, [Required] int pageSize)
    {
        var result = await _mediator.Send(new GetEmployeeListByPaymentStatusQuery(request, pageIndex, pageSize));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result); 
    }

    //[AllowAnonymous]
    //[HttpGet("GetEmployeeListByPaymentStatusAsync")]
    //public async Task<IActionResult> GetEmployeeListByPaymentStatusAsync(GetEmployeeListByPaymentStatusRequest request, [Required] int pageIndex, [Required] int pageSize)
    //{
    //    var result = await _mediator.Send(new GetEmployeeListByPaymentStatusQuery(request, pageIndex, pageSize));

    //    if (result.Succeeded)
    //    {
    //        return Ok(result);
    //    }

    //    return BadRequest(result); 
    //}
}

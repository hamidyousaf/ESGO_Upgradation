namespace Web.Controllers;

[Authorize]
public class NotificationsController(IMediator _mediator) : ApiBaseController
{

    [HttpGet("GetAllNotificationsAsync")]
    public async Task<IActionResult> GetAllNotificationsAsync()
    {
        var result = await _mediator.Send(new GetAllNotificationsQuery());

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}

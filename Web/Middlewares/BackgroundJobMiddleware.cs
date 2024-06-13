namespace Web.Middlewares;

public class BackgroundJobMiddleware : IMiddleware
{
    private readonly IMediator _mediator;

    public BackgroundJobMiddleware(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        RecurringJob.AddOrUpdate(() =>
        ConvertJobStatusFromConfirmToComplete()
        ,
        Cron.Hourly());

        RecurringJob.AddOrUpdate(() =>
        ConvertJobStatusFromOpenToUnsuccessful()
        ,
        Cron.Hourly());
        await next(context);
    }
    public async Task ConvertJobStatusFromConfirmToComplete()
    {
        await _mediator.Send(new ConvertJobStatusFromConfirmToCompleteCommand());
    }
    public async Task ConvertJobStatusFromOpenToUnsuccessful()
    {
        await _mediator.Send(new ConvertJobStatusFromOpenToUnsuccessfulCommand());
    }
}

using AppointmentApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
{
    public int Order { get; } = int.MaxValue - 10;

    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is HttpResponseException exception)
        {

            if(exception.Status == 409)
            {
                context.Result = new ObjectResult(exception.Value)
                {
                    StatusCode = 409
                };
            }
            else if(exception.Status == 404)
            {
                context.Result = new ObjectResult(exception.Value)
                {
                    StatusCode = 404
                };
            }
            else{
                context.Result = new ObjectResult(exception.Value)
                {
                    StatusCode = 400
                };
            }
        }
        else if(context.Exception is Exception ex)
        {
            context.Result = new ObjectResult(new CustomError(){Message="Server Error"})
            {
                StatusCode = 500
            };
        }

        context.ExceptionHandled = true;
    }
}
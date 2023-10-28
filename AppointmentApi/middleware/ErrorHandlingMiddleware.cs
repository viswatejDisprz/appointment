using System.Diagnostics;
using System.Linq.Expressions;
using System.Text.Json;

// CatchBlock exception you have to find out

public class ErrorHandlingMiddleware 
{

    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try{
           await _next(context);
        }
        catch(Exception ex)
        {
           Trace.WriteLine($"{ex}");
        }
       
    }
}
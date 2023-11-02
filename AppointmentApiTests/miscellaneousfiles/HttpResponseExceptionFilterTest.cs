using AppointmentApi;
using AppointmentApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

public class HttpResponseExceptionFilterTest
{
    private HttpResponseExceptionFilter _filter = new HttpResponseExceptionFilter();

    [Fact]
    public void TestOnActionExecutedWithHttpResponseException()
    {
        // Arrange
        var error = new CustomError{Message="Not Found"};
        var actionContext1 = new HttpResponseException(StatusCodes.Status400BadRequest,error);
        var routeData = new RouteData();
        routeData.Values["key"] = "value";
        var actionContext = new ActionContext(new DefaultHttpContext(), routeData , new ActionDescriptor());
        var context = new ActionExecutedContext(
            actionContext,
            new List<IFilterMetadata>(),
            new HttpResponseException(StatusCodes.Status404NotFound, error));

        context.Exception = new Exception();

        // Act
        _filter.OnActionExecuted(context);

        // Assert
        Assert.True(context.ExceptionHandled);
        Assert.IsType<ObjectResult>(context.Result);
        var result = (ObjectResult)context.Result;
    }

    [Fact]
    public void TestOnActionExecutedWithHttpResponseException1()
    {
        // Arrange
        var error = new CustomError{Message="Not Found"};
        var actionContext1 = new HttpResponseException(StatusCodes.Status400BadRequest,error);
        var filter = new HttpResponseExceptionFilter();
        var routeData = new RouteData();
        routeData.Values["key"] = "value";
        var actionContext = new ActionContext(new DefaultHttpContext(), routeData , new ActionDescriptor());
        var context = new ActionExecutedContext(
            actionContext,
            new List<IFilterMetadata>(),
            new Exception("Simulated exception"));

        context.Exception = new HttpResponseException(StatusCodes.Status404NotFound, error);

        // Act
        filter.OnActionExecuted(context);

        // Assert
        Assert.True(context.ExceptionHandled);
        Assert.IsType<ObjectResult>(context.Result);
        var result = (ObjectResult)context.Result;
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
    }


    [Fact]
    public void TestOnActionExecuting()
    {
        // Arrange
        var routeData = new RouteData();
        routeData.Values["key"] = "value";
        var actionContext1 = new ActionContext(new DefaultHttpContext(), routeData , new ActionDescriptor());
        var actionContext = new ActionExecutingContext(
            actionContext1,
            new List<IFilterMetadata>(),
            new Dictionary<string, object>(),
            new object()
        );
        var filter = new HttpResponseExceptionFilter();

        // Act
        filter.OnActionExecuting(actionContext);
    }
}


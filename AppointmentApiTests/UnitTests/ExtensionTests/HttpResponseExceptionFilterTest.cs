using AppointmentApi;
using AppointmentApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using MockAppointmentApiTests;

public class HttpResponseExceptionFilterTest
{
    private MockAppointments mock;
    private HttpResponseExceptionFilter _filter = new HttpResponseExceptionFilter();

    public HttpResponseExceptionFilterTest()
    {
        mock = new MockAppointments();
    }

    [Fact]
    public void TestOnActionExecutedWithHttpResponseException()
    {
        // Arrange
        var error = mock.customError();
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
    }

    [Fact]
    public void TestOnActionExecutedWithHttpResponseException1()
    {
        // Arrange
        var error = mock.customError();
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


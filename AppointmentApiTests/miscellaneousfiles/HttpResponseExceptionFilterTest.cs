// using AppointmentApi.Models;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Filters;
// using Xunit;
// using Moq;
// using AppointmentApi;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc.Abstractions;
// using Microsoft.AspNetCore.Routing;



// public class HttpResponseExceptionFilterTests
// {
//     [Fact]
//     public void OnActionExecuted_should_handle_HttpResponseException_and_return_an_ObjectResult_with_the_correct_status_code()
//     {
//         // Arrange
//         var filter = new HttpResponseExceptionFilter();
//         var exception = new HttpResponseException(404, ResponseErrors.NotFound);
//         var actionContextMock = new Mock<ActionContext>(MockBehavior.Loose);
//         actionContextMock.Setup(m => m.HttpContext.Request.Path).Returns("/api/appointments");
//         var context = new ActionExecutedContext(actionContextMock.Object, new List<IFilterMetadata>(), exception);

//         // Act
//         filter.OnActionExecuted(context);

//         // Assert
//         Assert.True(context.ExceptionHandled);
//         Assert.IsType<ObjectResult>(context.Result);
//         var result = context.Result as ObjectResult;
//         Assert.Equal(404, result.StatusCode);
//         Assert.Equal(ResponseErrors.NotFound, result.Value);
//     }

//     [Fact]
//     public void OnActionExecuted_should_handle_Exception_and_return_an_ObjectResult_with_a_500_status_code()
//     {
//         // Arrange
//         var filter = new HttpResponseExceptionFilter();
//         var exception = new Exception("This is a sample exception.");
//         var actionContextMock = new Mock<ActionContext>(MockBehavior.Loose);
//         actionContextMock.Setup(m => m.HttpContext.Request.Path).Returns("/api/appointments");
//         var context = new ActionExecutedContext(actionContextMock.Object, new List<IFilterMetadata>(), exception);

//         // Act
//         filter.OnActionExecuted(context);

//         // Assert
//         Assert.True(context.ExceptionHandled);
//         Assert.IsType<ObjectResult>(context.Result);
//         var result = context.Result as ObjectResult;
//         Assert.Equal(500, result.StatusCode);
//         Assert.IsType<CustomError>(result.Value);
//         var error = result.Value as CustomError;
//         Assert.Equal(exception.Message, error.Message);
//     }

//     [Fact]
//     public void OnActionExecuted_should_handle_HttpResponseException_and_return_an_ObjectResult_with_the_correct_status_code1()
//     {
//         // Arrange
//         var filter = new HttpResponseExceptionFilter();
//         var exception = new HttpResponseException(404, ResponseErrors.NotFound);
//         // var actionContext = new ActionContext(new DefaultHttpContext(), new List<IFilterMetadata>(), exception);
//         var actionContext = new ActionContext(new DefaultHttpContext(), new RouteData(), exception);

//         // Act
//         filter.OnActionExecuted(actionContext);

//         // Assert
//         Assert.True(actionContext.ExceptionHandled);
//         Assert.IsType<ObjectResult>(actionContext.Result);
//         var result = actionContext.Result as ObjectResult;
//         Assert.Equal(404, result.StatusCode);
//         Assert.Equal(ResponseErrors.NotFound, result.Value);
//     }
// }
using AppointmentApi;
using AppointmentApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

using Xunit;

public class HttpResponseExceptionFilterTest
{
    private HttpResponseExceptionFilter _filter = new HttpResponseExceptionFilter();

    [Fact]
    public void TestOnActionExecutedWithHttpResponseException()
    {
        // Arrange
        var error = new CustomError{Message="Not Found"};
        var actionContext1 = new HttpResponseException(StatusCodes.Status400BadRequest,error);
        // var routeData = new RouteData(typeof(object), new Dictionary<string, object>());
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
        Assert.Equal(404, result.StatusCode);
        Assert.Equal("Not Found", ((CustomError)result.Value).Message);
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

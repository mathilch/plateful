using Events.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Events.Api.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType = "application/problem+json";
        ProblemDetails problemDetails = new();
        ValidationProblemDetails validationProblemDetails = new();

        problemDetails.Instance = httpContext.Request.Path;
        problemDetails.Detail = exception.Message;

        validationProblemDetails.Instance = httpContext.Request.Path;
        validationProblemDetails.Detail = exception.Message;

        switch (exception)
        {
            case UnauthorizedUserException:
                problemDetails.Title = "User is unauthorized";
                problemDetails.Status = StatusCodes.Status401Unauthorized;
                break;
            case EventIdNotFoundException:
                problemDetails.Title = "Event not found";
                problemDetails.Status = StatusCodes.Status404NotFound;
                break;
            case RemoveEventParticipantException:
                problemDetails.Title = "Participant not found registered with Event";
                problemDetails.Status = StatusCodes.Status404NotFound;
                break;
            case UserIsNotSignedUpForEventException:
                problemDetails.Title = "User not signed up for event";
                problemDetails.Status = StatusCodes.Status400BadRequest;
                break;
            case UserIsNotTheRightAgeException:
                problemDetails.Title = "User age is not right for participation";
                problemDetails.Status = StatusCodes.Status400BadRequest;
                break;
            case RequestValidationException:
                var requestValidationException = (RequestValidationException)exception;
                validationProblemDetails.Title = "Request Validation Errors";
                validationProblemDetails.Status = StatusCodes.Status400BadRequest;
                validationProblemDetails.Errors = requestValidationException.ValidationErrors;
                break;
            default:
                problemDetails.Title = "An unexpected error occurred.";
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                problemDetails.Detail = "An unexpected error occurred. Contact support if the problem persists.";
                break;
        }
        if (exception is RequestValidationException)
        {
            httpContext.Response.StatusCode = validationProblemDetails.Status ?? StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(validationProblemDetails, cancellationToken);
        }
        else
        {
            httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        }

        return true;
    }
}

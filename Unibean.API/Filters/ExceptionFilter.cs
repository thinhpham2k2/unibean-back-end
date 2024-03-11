using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Unibean.Service.Models.Exceptions;

namespace Unibean.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is InvalidParameterException exception1)
        {
            var errorMessages = new List<string>();
            var exception = exception1;

            foreach (var entry in exception.ModelState)
            {
                var errors = entry.Value.Errors;

                foreach (var error in errors)
                {
                    errorMessages.Add(error.ErrorMessage);
                }
            }

            context.Result = new BadRequestObjectResult(errorMessages.Distinct());
            context.ExceptionHandled = true;
        }
    }
}

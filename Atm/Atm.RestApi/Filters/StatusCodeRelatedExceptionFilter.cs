namespace Atm.RestApi.Filters
{
    using Atm.Application.Exceptions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// Status code related exception filter.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Filters.IOrderedFilter" />
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Filters.IActionFilter" />
    public class StatusCodeRelatedExceptionFilter : IOrderedFilter, IActionFilter
    {
        public int Order => 1;

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is StatusCodeRelatedException exception)
            {
                context.Result = new ObjectResult(exception.Message)
                {
                    StatusCode = exception.StatusCode
                };

                context.ExceptionHandled = true;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
    }
}
using System.Net;
using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace MyExpManAPI.Helpers
{
    public class ActionFilters :  IActionFilter
    {
        private readonly ILogger<ActionFilters> logger;
        private readonly ILogManager logmanager;
        public ActionFilters(ILogger<ActionFilters> logger, ILogManager _logmanager)
        {
            this.logger = logger;
            this.logmanager = _logmanager;
        
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
        
            logger.LogInformation(new EventId(0), $"OnActionExecuting {DateTime.Today} Message: {context.Result}");
        
        }

    public void OnActionExecuted(ActionExecutedContext context)
        {
            
            logger.LogInformation(new EventId(0), $"OnActionExecuted {DateTime.Today} Message: {context.Result}");
        }

    }
}
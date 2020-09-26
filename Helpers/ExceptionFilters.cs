using System;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DocumentGenAPI.Entities;

namespace DocumentGenAPI.Helpers
{
    public class ExceptionFilters : ExceptionFilterAttribute
    {
        /*private readonly ILogManager logmanager;
        private readonly ILogger logger;
        public ExceptionFilters(ILogManager _logmanager,ILogger _logger)
        {
            logmanager =_logmanager; 
            logger = _logger;
        }*/
        public override void OnException(ExceptionContext context)
        {
            
            //logger.LogError(context.Exception.Message);
            //logmanager.CreateLog("Error",$"A unexpected exception have ocurred. Exception: {context.Exception.Message}");
        }
    }
}
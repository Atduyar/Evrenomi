using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Api.Filters
{
    public class CustomModelAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {

                //context.Result = new OkObjectResult(new ApiBadRequestResponse(context.ModelState));
                context.Result = new BadRequestObjectResult(new ApiBadRequestResponse(context.ModelState));
            }
        }
    }

    public class ApiBadRequestResponse
    {
        //public string Error { get; set; }

        public string Operation { get; set; }
        public string ErrorMessages { get; set; }

        public ApiBadRequestResponse(ModelStateDictionary modelState)
        {
            //Errors = modelState.SelectMany(x => x.Value.Errors)
            //    .Select(x => x.ErrorMessage).ToArray();

            foreach (var modelStateKey in modelState.Keys)
            {
                var modelStateVal = modelState[modelStateKey];
                foreach (var error in modelStateVal.Errors)
                {
                    var key = modelStateKey;
                    var errorMessage = error.ErrorMessage;
                    Operation = key;
                    ErrorMessages = errorMessage;
                }
            }
        }
    }
}
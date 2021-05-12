﻿//TODO GitFix
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Lms.MVC.UI.Filters
{
    public class ModelValid : Attribute
    {

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new ViewResult
                {
                    StatusCode = 400,
                    ViewData = ((Controller)context.Controller).ViewData,
                    TempData = ((Controller)context.Controller).TempData,
                };
            }
        }

    }
}

﻿using eGradeBook.Utilities.WebApi;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace eGradeBook
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Camel case formatting
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Validation filter
            config.Filters.Add(new ValidateModelAttribute());

            // Exception filter
            config.Filters.Add(new CustomExceptionFilter());

            // Exception logging
            config.Services.Add(typeof(IExceptionLogger), new NLogExceptionLogger());

            // Date formatting
            config.Formatters.JsonFormatter.SerializerSettings.DateFormatString = "yyyy-MM-dd";
        }
    }
}

﻿using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace eGradeBook.SwaggerHelpers
{
    /// <summary>
    /// Swagger helper class
    /// </summary>
    public class AssignOAuth2SecurityRequirements : IOperationFilter
    {
        /// <summary>
        /// Apply per operation
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="schemaRegistry"></param>
        /// <param name="apiDescription"></param>
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var actFilters = apiDescription.ActionDescriptor.GetFilterPipeline();
            var allowsAnonymous = actFilters.Select(f => f.Instance).OfType<OverrideAuthorizationAttribute>().Any();
            if (allowsAnonymous)
                return; // must be an anonymous method


            //var scopes = apiDescription.ActionDescriptor.GetFilterPipeline()
            //    .Select(filterInfo => filterInfo.Instance)
            //    .OfType<AllowAnonymousAttribute>()
            //    .SelectMany(attr => attr.Roles.Split(','))
            //    .Distinct();

            if (operation.security == null)
                operation.security = new List<IDictionary<string, IEnumerable<string>>>();

            var oAuthRequirements = new Dictionary<string, IEnumerable<string>>
        {
            {"oauth2", new List<string> {"sampleapi"}}
        };

            operation.security.Add(oAuthRequirements);
        }
    }
}
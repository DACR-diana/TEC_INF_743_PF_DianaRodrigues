using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Api.Extensions
{
    public class LanguageRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {

            if (!values.ContainsKey("culture"))
                return false;

            var culture = values["culture"].ToString();
            return culture == "en-US" || culture == "pt-PT" || culture == "es-ES";
        }
    }
}

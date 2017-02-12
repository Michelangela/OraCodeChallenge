using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using System.Web.Http.Cors;

namespace OraCodeChallenge
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.MessageHandlers.Add(new App_Start.AuthHandler());

            // Enforce HTTPS
            config.Filters.Add(new OraCodeChallenge.Filters.RequireHttpsAttribute());
        }
    }
}

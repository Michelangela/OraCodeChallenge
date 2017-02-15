using OraCodeChallenge.Models.Entities;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.OData.Extensions;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;


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
            //config.SuppressDefaultHostAuthentication();
            //config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();


            //ODataModelBuilder builder = new ODataConventionModelBuilder();
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Chat>("Chats");
            builder.EntitySet<ChatMessage>("ChatMessages");
            //config.AddODataQueryFilter();
            config.MapODataServiceRoute("odata", null, builder.GetEdmModel());
           
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            config.Routes.MapHttpRoute(
                name: "OraApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


            //config.Formatters.Remove(config.Formatters.XmlFormatter);

            //config.MessageHandlers.Add(new App_Start.AuthHandler());

            // Enforce HTTPS
            config.Filters.Add(new OraCodeChallenge.Filters.RequireHttpsAttribute());

        }
    }
}


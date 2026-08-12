using System.Web;
using System.Web.Http;
using System.Web.Routing;
public static class WebApiConfig
{
    public static void Register(HttpConfiguration config)
    {
        config.MapHttpAttributeRoutes();

        config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional }
        );

        // Forzar JSON como formato de respuesta, sin importar el Accept header
        config.Formatters.Remove(config.Formatters.XmlFormatter);
    }
} 
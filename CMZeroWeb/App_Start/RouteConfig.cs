using System.Web.Mvc;
using System.Web.Routing;

namespace CMZeroWeb.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "ApplicationsToEdit", 
                url: "dashboard/application/{id}", 
                defaults: new {controller = "Application", action="index"});

            routes.MapRoute("OhBugger", "ohbugger", new {controller = "Dashboard", action = "Error"});

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sgart.MvcAngularJS
{
  public class RouteConfig
  {
    public static void RegisterRoutes(RouteCollection routes)
    {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

      // cattura tutte le query per usare i path completi lato angularjs
      routes.MapRoute(
        name: "AngularJS_All",
        url: "{*queryvalues}",
        defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
        namespaces: new[] { "Sgart.MvcAngularJS.Controllers" }
       );
      /*
      routes.MapRoute(
          name: "Default",
          url: "{controller}/{action}/{id}",
          defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
      );
      */
    }
  }
}

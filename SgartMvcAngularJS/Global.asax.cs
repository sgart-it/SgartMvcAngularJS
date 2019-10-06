using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
//using System.Web.Optimization;
using System.Web.Routing;

namespace Sgart.MvcAngularJS
{
  public class WebApiApplication : System.Web.HttpApplication
  {
protected void Application_Start()
{
  AreaRegistration.RegisterAllAreas();
  GlobalConfiguration.Configure(WebApiConfig.Register);
  FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
  RouteConfig.RegisterRoutes(RouteTable.Routes);
  //BundleConfig.RegisterBundles(BundleTable.Bundles);

  // forzo la formattazione dei nomi in stile javascript, camelCase
  var formatters = GlobalConfiguration.Configuration.Formatters;
  var jsonFormatter = formatters.JsonFormatter;
  var settings = jsonFormatter.SerializerSettings;
  settings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
}
  }
}

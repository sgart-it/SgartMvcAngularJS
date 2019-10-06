using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sgart.MvcAngularJS.Controllers
{
  public class HomeController : Controller
  {
    public ActionResult Index()
    {
      ViewBag.Title = "TODO";

      //uso la version per invalidare i CSS/JS quando lo modifico
      ViewBag.VERSION = Code.Manager.VERSION;

      return View();
    }
  }
}

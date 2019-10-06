using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sgart.MvcAngularJS.Models.BO
{
  public class SearchInputItem
  {
    public int Page { get; set; }
    public int Size { get; set; }
    public string Text { get; set; }
    public int? IDCategory { get; set; }
    public int? Status { get; set; }
    public string Sort { get; set; }
  }
}
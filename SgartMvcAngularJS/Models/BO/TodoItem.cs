using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sgart.MvcAngularJS.Models.BO
{
  public class TodoItem
  {
    public int ID { get; set; }
    public DateTime Date { get; set; }
    public string Title { get; set; }
    public string Note { get; set; }
    public int IDCategory { get; set; }
    public string Category { get; set; }
    public string Color { get; set; }
    public DateTime? Completed { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }

    public int TotalItems { get; set; }
  }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using BO = Sgart.MvcAngularJS.Models.BO;

namespace Sgart.MvcAngularJS.Controllers
{
  [RoutePrefix("api")]
  public class TodoController : ApiController
  {

[HttpGet]
[Route("todo/search")]
public BO.ServiceStatusList<BO.TodoItem> Search([FromUri] BO.SearchInputItem param)
{

  BO.ServiceStatusList<BO.TodoItem> result = new Models.BO.ServiceStatusList<Models.BO.TodoItem>();
  result.Data = new List<Models.BO.TodoItem>();
  try
  {
    //normalizzo i parametri
    var pageNumber = param.Page;
    if (pageNumber < 1) pageNumber = 1;
    var pageSize = param.Size;
    if (pageSize < 1) pageSize = 10;
    var startIndex = (pageNumber - 1) * pageSize;

    string text = param.Text;
    if (string.IsNullOrWhiteSpace(text))
      text = null;
    int? idCategory = param.IDCategory; // - 1;
    int? status = param.Status;
    string sort = param.Sort;
    result.Data = Code.Manager.Search(startIndex, pageSize, text, idCategory, status, sort);
    result.AddSuccess("Readed startIndex: " + startIndex + " items: " + result.Data.Count);
    result.Success = true;
  }
  catch (Exception ex)
  {
    result.AddError(ex);
  }
  return result;
}

    [HttpGet]
    [Route("todo/{id}")]
    public BO.ServiceStatus<BO.TodoItem> Read(int id)
    {
      BO.ServiceStatus<BO.TodoItem> result = new Models.BO.ServiceStatus<Models.BO.TodoItem>();
      try
      {
        result.Data = Code.Manager.Read(id);
        result.AddSuccess("Readed: " + 1);
        result.Success = true;
      }
      catch (Exception ex)
      {
        result.AddError(ex);
      }
      return result;
    }

    [HttpPost]
    [Route("todo/insert")]
    public BO.ServiceStatus Insert([FromBody] BO.TodoItem param)
    {
      BO.ServiceStatus result = new Models.BO.ServiceStatus();
      try
      {
        //controllo presenza parametri
        if (param.ID != 0) result.AddError("Ivalid `id` in INSERT");
        //if (!req.body.hasOwnProperty('date')) result.addError('`date` required');
        if (string.IsNullOrWhiteSpace(param.Title)) result.AddError("`title` required");
        if (param.IDCategory == 0) result.AddError("`idCategory` required");

        if (result.Messages.Count == 0)
        {
          DateTime date = param.Date;
          string title = param.Title;
          int idCategory = param.IDCategory;
          string note = param.Note;
          int r = Code.Manager.Insert(date, title, note, idCategory);
          if (r == 0)
            result.AddError("Error: not inserted");
          else
          {
            result.AddSuccess("Readed: " + 1);
            result.Success = true;
          }
        }
      }
      catch (Exception ex)
      {
        result.AddError(ex);
      }
      return result;
    }

    [HttpPost]
    [Route("todo/update")]
    public BO.ServiceStatus Updarte([FromBody] BO.TodoItem param)
    {
      BO.ServiceStatus result = new Models.BO.ServiceStatus();
      try
      {
        //controllo presenza parametri
        if (param.ID == 0) result.AddError("`id` in required");
        //if (!req.body.hasOwnProperty('date')) result.addError('`date` required');
        if (string.IsNullOrWhiteSpace(param.Title)) result.AddError("`title` required");
        if (param.IDCategory == 0) result.AddError("`idCategory` required");

        if (result.Messages.Count == 0)
        {
          int id = param.ID;
          DateTime date = param.Date;
          string title = param.Title;
          int idCategory = param.IDCategory;
          string note = param.Note;
          DateTime? completed = param.Completed;
          int r = Code.Manager.Update(id, date, title, note, idCategory, completed);
          if (r == 0)
            result.AddError("Error: not updated");
          else
          {
            result.AddSuccess("Updated: " + 1);
            result.Success = true;
          }

        }
      }
      catch (Exception ex)
      {
        result.AddError(ex);
      }
      return result;
    }

    [HttpPost]
    [Route("todo/delete")]
    public BO.ServiceStatus Delete([FromBody] BO.TodoItem param)
    {
      BO.ServiceStatus result = new Models.BO.ServiceStatus();
      try
      {
        //controllo presenza parametri
        int id = param.ID;
        if (id == 0) result.AddError("`id` in required");

        if (result.Messages.Count == 0)
        {
          int r = Code.Manager.Delete(id);
          if (r == 0)
            result.AddError("Error: not deleted");
          else
          {
            result.AddSuccess("Deleted id: " + id.ToString());
            result.Success = true;
          }
        }
      }
      catch (Exception ex)
      {
        result.AddError(ex);
      }
      return result;
    }

    [HttpPost]
    [Route("todo/toggle")]
    public BO.ServiceStatus<BO.TodoItem> Toggle([FromBody] BO.TodoItem param)
    {
      BO.ServiceStatus<BO.TodoItem> result = new Models.BO.ServiceStatus<BO.TodoItem>();
      try
      {
        int id = param.ID;
        //controllo presenza parametri
        if (id == 0) result.AddError("`id` in required");

        if (result.Messages.Count == 0)
        {
          result.Data = Code.Manager.Toggle(id);
          if (result.Data==null)
            result.AddError("Error: not toggled");
          else
          {
            result.AddSuccess("Toggled id: " + id.ToString());
            result.Success = true;
          }
        }
      }
      catch (Exception ex)
      {
        result.AddError(ex);
      }
      return result;
    }

    [HttpPost]
    [Route("todo/category")]
    public BO.ServiceStatus<BO.TodoCategoryItem> Category([FromBody] BO.TodoCategoryItem param)
    {
      BO.ServiceStatus<BO.TodoCategoryItem> result = new BO.ServiceStatus<BO.TodoCategoryItem>();
      try
      {
        //controllo presenza parametri
        if (param.ID == 0) result.AddError("`id` in required");

        if (result.Messages.Count == 0)
        {
          result.Data = Code.Manager.Category(param.ID, param.IDCategory);
          //result.AddSuccess("Category updated id: " + param.ID.ToString());
          result.Success = true;
        }
      }
      catch (Exception ex)
      {
        result.AddError(ex);
      }
      return result;
    }

    [HttpGet]
    [Route("categories")]
    public BO.ServiceStatusList<BO.CategoryItem> Categories()
    {

      BO.ServiceStatusList<BO.CategoryItem> result = new Models.BO.ServiceStatusList<Models.BO.CategoryItem>();
      result.Data = new List<Models.BO.CategoryItem>();
      try
      {
        result.Data = Code.Manager.Categories();
        result.Success = true;
      }
      catch (Exception ex)
      {
        result.AddError(ex);
      }
      return result;
    }

    [HttpGet]
    [Route("statistics")]
    public BO.ServiceStatusList<BO.CategoryItem> Statistics()
    {

      BO.ServiceStatusList<BO.CategoryItem> result = new Models.BO.ServiceStatusList<Models.BO.CategoryItem>();
      result.Data = new List<Models.BO.CategoryItem>();
      try
      {
        result.Data = Code.Manager.Statistics();
        result.Success = true;
      }
      catch (Exception ex)
      {
        result.AddError(ex);
      }
      return result;
    }

  }
}

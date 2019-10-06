using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sgart.MvcAngularJS.Models.BO
{
  public class ServiceStatus
  {
    private const int SUCCESS_SECONDS = 2;

    public ServiceStatus()
    {
      Success = false;
      Messages = new List<ServiceStatusErrorItem>();
      ReturnValue = -1;
      ErrorCount = 0;
    }
    public bool Success { get; set; }
    public List<ServiceStatusErrorItem> Messages { get; set; }
    public object ReturnValue { get; set; }
    public int ErrorCount { get; set; }

    public void AddError(string message, int seconds = 0)
    {
      Messages.Add(new ServiceStatusErrorItem(ServiceStatusErrorType.Error, message, seconds));
      Success = false;
      ErrorCount++;
    }
    public void AddError(Exception ex, int seconds = 0)
    {
      Messages.Add(new ServiceStatusErrorItem(ServiceStatusErrorType.Error, ex.Message, seconds));
      Success = false;
      ErrorCount++;
    }

    public void AddWarning(string message, int seconds = 0)
    {
      Messages.Add(new ServiceStatusErrorItem(ServiceStatusErrorType.Warning, message, seconds));

    }
    public void AddWarning(Exception ex, int seconds = 0)
    {
      Messages.Add(new ServiceStatusErrorItem(ServiceStatusErrorType.Warning, ex.Message, seconds));
    }
    public void AddSuccess(string message, int seconds = SUCCESS_SECONDS)
    {
      Messages.Add(new ServiceStatusErrorItem(ServiceStatusErrorType.Success, message, seconds));
    }
    public void AddInfo(string message, int seconds = 0)
    {
      Messages.Add(new ServiceStatusErrorItem(ServiceStatusErrorType.Info, message, seconds));
    }
  }

  public class ServiceStatus<T> : ServiceStatus
  {
    public T Data { get; set; }
  }

  public class ServiceStatusList<T> : ServiceStatus<List<T>>
  {
    public ServiceStatusList() : base()
    {
    }
  }
}


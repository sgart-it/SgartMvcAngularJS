using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sgart.MvcAngularJS.Models.BO
{
  public class ServiceStatusErrorType
  {
    public const char Unknow = '-';
    public const char Error = 'E';
    public const char Warning = 'W';
    public const char Success = 'S';
    public const char Info = 'I';
  }

  public class ServiceStatusErrorItem
  {
    public ServiceStatusErrorItem()
    {
      Type = ServiceStatusErrorType.Unknow;
      Message = "";
      Seconds = 0;
    }

    public ServiceStatusErrorItem(char type, string message, int seconds)
    {
      Type = type;
      Message = message;
      Seconds = seconds;
    }

    [JsonProperty("t")]
    public char Type { get; set; }
    [JsonProperty("m")]
    public string Message { get; set; }
    [JsonProperty("s")]
    public int Seconds { get; set; }  //tempo dopo il quale il messaggio viene cancellato
  }

}
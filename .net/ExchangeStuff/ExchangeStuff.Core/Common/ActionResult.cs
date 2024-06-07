using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeStuff.Core.Common;

public class ActionResult
{
    public HttpStatusCode StatusCode { get; set; }
    public object? Data { get; set; }
    public string Detail { get; set; }

    public ActionResult()
    {
        Data = default;
        StatusCode = HttpStatusCode.OK;
        Detail = "";
    }

    public ActionResult BuildResult(object data = default, string detail = "")
    {
        Detail = detail;
        Data = data;
        StatusCode = HttpStatusCode.OK;
        return this;
    }
    public ActionResult BuildError(string detail, HttpStatusCode statusCode)
    {
        Detail = detail;
        StatusCode = statusCode;
        return this;
    }
    //public ActionResult SetInfo(string detail = default)
    //{
    //    Detail = detail;
    //    return this;
    //}
    //public ActionResult SetInfo(HttpStatusCode statusCode, string detail = default)
    //{
    //    Detail = detail;
    //    StatusCode = statusCode;
    //    return this;
    //}
}
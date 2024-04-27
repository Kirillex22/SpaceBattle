using CoreWCF;
using System;
using System.Runtime.Serialization;
using Hwdtech;
using System.Net;
using CoreWCF.OpenApi.Attributes;
using CoreWCF.Web;

namespace SpaceBattle.Server;

[ServiceContract]
[OpenApiBasePath("/api")]
public interface IService
{
    [OperationContract]
    [OpenApiResponse(ContentTypes = new[] { "application/json" }, Description = "Success", StatusCode = HttpStatusCode.OK)]
    [OpenApiTag("Tag")]
    [WebInvoke(Method = "POST", UriTemplate = "/send_message", RequestFormat = WebMessageFormat.Json)]
    public void HandleMessage([OpenApiParameter(ContentTypes = new[] { "application/json" }, Description = "param description.")] MessageContract param);
}


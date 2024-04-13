using System.Net;
using CoreWCF;
using CoreWCF.OpenApi.Attributes;
using CoreWCF.Web;

namespace SpaceBattle.HttpServer;

    [ServiceContract]
    [OpenApiBasePath("/api")]
    internal interface IWebApi
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/body")]
        [OpenApiTag("Tag")]
        [OpenApiResponse(ContentTypes = new[] { "application/json", "text/xml" }, Description = "Success", StatusCode = HttpStatusCode.OK, Type = typeof(ExampleContract)) ]
        ExampleContract BodyEcho(
            [OpenApiParameter(ContentTypes = new[] { "application/json" }, Description = "param description.")] ExampleContract param);
    }
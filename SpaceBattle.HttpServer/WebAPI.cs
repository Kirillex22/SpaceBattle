using CoreWCF;

namespace SpaceBattle.HttpServer;

    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    internal class WebApi : IWebApi
    {
        public string PathEcho(string param) => param;

        public string QueryEcho(string param) => param;

        public ExampleContract BodyEcho(ExampleContract param) => param;
    }
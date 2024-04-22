using System.Net.Http.Json;
using System.Collections.Generic;
using Hwdtech;
using Microsoft.Extensions.ObjectPool;
using System.Runtime.CompilerServices;

namespace SpaceBattle.HttpServer;

public class Endpoint : IDisposable
{
    private WebApplication _app;
    private object _scope;

    public void CreateApp(object scope)
    {
        _scope = scope;
        var builder = WebApplication.CreateBuilder();
        _app = builder.Build();

        _app.MapPost("/send_message", (MessageContract message) =>
        {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", _scope).Execute();
            var queueId = IoC.Resolve<string>("Endpoint.GetServerThreadQueueIdByGameId", message.GameId);
            var cmd = IoC.Resolve<SpaceBattle.Lib.ICommand>("Endpoint.ParseMessageToCmd", message);
            IoC.Resolve<SpaceBattle.Lib.ICommand>("ServerThread.SendCommand", cmd, queueId).Execute();
        });

    }

    public async void StartListening(int port)
    {
        await _app.RunAsync($"http://localhost:{port}");
    }

    public void Dispose()
    {
        _app.StopAsync();
    }
}

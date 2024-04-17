using System.Net.Http.Json;
using System.Collections.Generic;
using Hwdtech;

namespace SpaceBattle.HttpServer;

public class Endpoint
{
    private WebApplication _app;

    public void CreateApp()
    {
        var builder = WebApplication.CreateBuilder();
        _app = builder.Build();
        _app.MapPost("/send_message", (MessageContract message) =>
        {
            var queueId = IoC.Resolve<string>("Endpoint.GetServerThreadQueueIdByGameId", message.GameId);
            var cmd = IoC.Resolve<SpaceBattle.Lib.ICommand>("Endpoint.ParseMessageToCmd", message);
            IoC.Resolve<SpaceBattle.Lib.ICommand>("ServerThread.SendCommand", cmd, queueId).Execute();
            return message;
        });
    }
    public async void StartListening(int port, Action act)
    {
        act();
        _app.Run($"http://localhost:{port}");
    }
    public async void Stop() => await _app.StopAsync();
}


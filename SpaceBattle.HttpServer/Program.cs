using System.Net.Http.Json;
using System.Collections.Generic;
namespace SpaceBattle.HttpServer;

class Endpoint
{
    public void ServerRun()
    {
        var builder = WebApplication.CreateBuilder();
        var app = builder.Build();
        app.MapPost("/", (MessageContract user) =>
        {
            return user;
        });
        app.Run();
    }
}

class Program
{
    static HttpClient httpClient = new HttpClient();
    static async Task Main()
    {
        var t = new Thread(() => new Endpoint().ServerRun());

        t.Start();

        Thread.Sleep(3000);
        var msg = new MessageContract { Type = "fire", GameId = "228", ItemId = "229", InitialValues = new Dictionary<string, object>() { { "string", "object" } } };
        JsonContent content = JsonContent.Create(msg);
        using var response = await httpClient.PostAsync("http://localhost:5266/", content);
        MessageContract? r_msg = await response.Content.ReadFromJsonAsync<MessageContract>();
        Console.WriteLine($"{r_msg.Type} - {r_msg.GameId} - {r_msg.ItemId} - {r_msg.InitialValues}");
    }
}

class MessageContract
{
    public string Type { get; set; } = "";
    public string GameId { get; set; } = "";
    public string ItemId { get; set; } = "";
    public Dictionary<string, object> InitialValues { get; set; }
}

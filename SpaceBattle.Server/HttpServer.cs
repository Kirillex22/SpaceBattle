using Hwdtech;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.Net;

namespace SpaceBattle.Server;

public class HttpServer : IDisposable
{
    private IWebHost _app;
    public HttpServer(int port)
    {
        IWebHostBuilder builder = WebHost.CreateDefaultBuilder()
        .UseKestrel(options =>
        {
            options.ListenAnyIP(port);
        })
        .UseStartup<Startup>();

        _app = builder.Build();
    }

    public async void Start()
    {
        await _app.RunAsync();
    }

    public void Dispose()
    {
        _app.StopAsync();
    }
}
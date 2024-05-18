using Microsoft.AspNetCore;

namespace SpaceBattle.Server;

public class HttpServer : IAsyncDisposable
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

    public async ValueTask DisposeAsync()
    {
        await _app.StopAsync();
    }
}


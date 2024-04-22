using SpaceBattle.HttpServer;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Threading;
using Hwdtech;
using Hwdtech.Ioc;
using System.Collections.Generic;
using SpaceBattle.Lib;
using System.Net.Http.Json;

namespace SpaceBattle.Tests;
internal class ActionCommand : SpaceBattle.Lib.ICommand
{
    private Action _act;
    public ActionCommand(Action act)
    {
        _act = act;
    }

    public void Execute()
    {
        _act();
    }
}

public class EndpointTest
{
    private Queue<SpaceBattle.Lib.ICommand> testQueue;
    private MessageContract buddy;
    public EndpointTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Endpoint.GetServerThreadQueueIdByGameId", (object[] args) =>
        {
            return "guid of st";
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Endpoint.ParseMessageToCmd", (object[] args) =>
        {
            var msg = (MessageContract)args[0];
            var cmd = new ActionCommand(() =>
            {
                buddy = msg;
            });
            return cmd;
        }).Execute();

        testQueue = new Queue<SpaceBattle.Lib.ICommand>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ServerThread.SendCommand", (object[] args) =>
        {
            var cmd = (SpaceBattle.Lib.ICommand)args[0];
            return new ActionCommand(() => testQueue.Enqueue(cmd));
        }).Execute();

    }

    [Fact]
    public async Task SuccesfulSendingTheCmd()
    {
        var scope = IoC.Resolve<object>("Scopes.Current");

        using (var endpoint = new Endpoint())
        {
            endpoint.CreateApp(scope);
            endpoint.StartListening(8000);
            string url = "http://localhost:8000/send_message";
            var client = new HttpClient();
            var msg = new MessageContract { Type = "fire", GameId = "10", ItemId = "190", InitialValues = new Dictionary<string, object>() { { "initialVelocity", 1 } } };
            JsonContent content = JsonContent.Create(msg);

            using var response = await client.PostAsync(url, content);

            Assert.True(testQueue.Count != 0);
            testQueue.Dequeue().Execute();
            Assert.True(
                (buddy.Type == msg.Type) &&
                (buddy.GameId == msg.GameId) &&
                (buddy.ItemId == msg.ItemId)
            );
        };

    }

}
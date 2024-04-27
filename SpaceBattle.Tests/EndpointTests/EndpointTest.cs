using SpaceBattle.Server;
using System.Net;
using System.Text;
using System.Threading;
using Hwdtech;
using Hwdtech.Ioc;
using System.Collections.Generic;
using SpaceBattle.Lib;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.DataProtection;
using System.Web.Services.Description;

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
    private Queue<SpaceBattle.Lib.ICommand> _testQueue;
    public EndpointTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        _testQueue = new Queue<SpaceBattle.Lib.ICommand>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ServerThread.SendCommand", (object[] args) =>
        {
            var cmd = (SpaceBattle.Lib.ICommand)args[0];
            return new ActionCommand(() => _testQueue.Enqueue(cmd));
        }).Execute();
    }

    [Fact]
    public void SuccesfulSendingTheCmd()
    {
        MessageContract result = new MessageContract();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ServerThread.GetQueueIdByGame", (object[] args) =>
        {
            return "TEST QUEUE ID";
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Endpoint.InterpretateMessage", (object[] args) =>
        {
            var msg = (MessageContract)args[0];
            return new ActionCommand(() => { result = msg; });
        }).Execute();

        var ep = new Endpoint();
        var msg = new MessageContract
        {
            Type = "fire",
            GameId = "GUID1",
            ItemId = "GUID2",
            InitialValues = new Dictionary<string, object>()
            {
                { "initialVelocity", 1 }
            }
        };

        ep.HandleMessage(msg);

        Assert.NotEmpty(_testQueue);
        _testQueue.Dequeue().Execute();
        Assert.True(result == msg);
    }
}


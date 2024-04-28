using CoreWCF;
using System;
using System.Runtime.Serialization;
using Hwdtech;
using System.Runtime.CompilerServices;
using System.Net;
using CoreWCF.OpenApi.Attributes;
using CoreWCF.Web;

namespace SpaceBattle.Server;

public class Endpoint : IService
{
    public void HandleMessage(MessageContract msg)
    {
        var queueId = IoC.Resolve<string>("ServerThread.GetQueueIdByGame", msg.GameId);
        var cmd = IoC.Resolve<SpaceBattle.Lib.ICommand>("Endpoint.InterpretateMessage", msg);
        IoC.Resolve<SpaceBattle.Lib.ICommand>("ServerThread.SendCommand", cmd, queueId).Execute();
    }
}


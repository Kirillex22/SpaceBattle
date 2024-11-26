using Hwdtech;

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


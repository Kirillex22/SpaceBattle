using Hwdtech;
using System.Collections.Concurrent;

namespace SpaceBattle.Lib;

public class SendCommand
{
    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Struct.ServerThread.SendCommand", (object[] args) =>
        {
            var queue = IoC.Resolve<ServerThread>("ServerThreadContainer.Find", (Guid)args[0]).GetQueue();
            var cmdToSend = (ICommand)args[1];
            
            var act = () =>
            {
                queue.Add(cmdToSend);
            };

            return new ActionCommand(act);
        }).Execute();
    }
}


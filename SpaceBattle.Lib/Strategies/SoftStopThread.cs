using System.Collections.Concurrent;
using Hwdtech;

namespace SpaceBattle.Lib;

public class SoftStopThread
{
    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Struct.ServerThread.SoftStop", (object[] args) =>
        {
            var thread = IoC.Resolve<ServerThread>("ServerThreadContainer.Find", (Guid)args[0]);
            var queue = thread.GetQueue();
            var ss = new SoftStopCommand(thread, queue, (Action)args[1]);
            var cmd = IoC.Resolve<ICommand>("Game.Struct.ServerThread.SendCommand", (Guid)args[0], ss);
            return cmd;
        }).Execute();
    }
}
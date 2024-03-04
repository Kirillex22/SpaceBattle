using Hwdtech;
using System.Collections.Concurrent;

namespace SpaceBattle.Lib;

public class SendCommand
{
    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Struct.ServerThread.SendCommand", (object[] args) =>
        {
            var id = (int)args[0];
            var queue = IoC.Resolve<Dictionary<int, BlockingCollection<SpaceBattle.Lib.ICommand>>>("Game.Struct.ServerThreadQueue.List")[id];
            var cmdToSend = (ICommand)args[1];
            var act = () =>
            {
                queue.Add(cmdToSend);
            };
            var cmd = new ActionCommand(act);
            return cmd;
        }).Execute();
    }
}


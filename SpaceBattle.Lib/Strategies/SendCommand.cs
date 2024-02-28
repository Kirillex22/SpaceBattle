using Hwdtech;
using System.Collections.Concurrent;

namespace SpaceBattle.Lib;

public class SendCommand
{
    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Struct.ServerThread.SendCommand", (object[] args) =>
        {
            var queue = IoC.Resolve<Dictionary<int, BlockingCollection<SpaceBattle.Lib.ICommand>>>("Game.Struct.ServerThreadQueue.List")[(int)args[0]];
            return new SendCmdCommand(queue, (ICommand)args[1]);
        }).Execute();
    }
}


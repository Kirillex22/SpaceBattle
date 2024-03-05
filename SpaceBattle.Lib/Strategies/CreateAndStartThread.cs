using Hwdtech;
using System.Collections.Concurrent;

namespace SpaceBattle.Lib;

public class CreateAndStartThread
{
    public void Call()
    {
        var threadList = IoC.Resolve<IDictionary<int, ServerThread>>("Game.Struct.ServerThread.List");
        var queueList = IoC.Resolve<IDictionary<int, BlockingCollection<ICommand>>>("Game.Struct.ServerThreadQueue.List");

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Struct.ServerThread.CreateAndStart", (object[] args) =>
        {
            var act = () =>
            {
                var action = (Action)args[1];
                var queue = new BlockingCollection<ICommand>();
                var st = new ServerThread(queue, action);
                threadList.Add((int)args[0], st);
                queueList.Add((int)args[0], queue);
                st.Start();
            };
            var cmd = new ActionCommand(act);
            return cmd;
        }).Execute();
    }
}


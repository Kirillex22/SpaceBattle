using Hwdtech;
using System.Collections.Concurrent;

namespace SpaceBattle.Lib;

public class CreateAndStartThread
{
    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Struct.ServerThread.CreateAndStart", (object[] args) =>
        {
            var act = () =>
            {
                var enterHook = (Action)args[1];
                var exitHook = (Action)args[2];
                var queue = new BlockingCollection<ICommand>();
                var st = new ServerThread(queue, enterHook, exitHook);
                IoC.Resolve<ICommand>("ServerThreadContainer.Add", (Guid)args[0], st).Execute();
                st.Start();
            };
            var cmd = new ActionCommand(act);
            return cmd;
        }).Execute();
    }
}


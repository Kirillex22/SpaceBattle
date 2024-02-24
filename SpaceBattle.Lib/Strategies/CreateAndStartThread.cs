using Hwdtech;
using System.Collections.Concurrent;

namespace SpaceBattle.Lib;

public class CreateAndStartThread
{
    public void Call()
    {
        var threadList = IoC.Resolve<IDictionary<int, ServerThread>>("Game.Struct.ServerThread.List");

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Struct.ServerThread.CreateAndStart", (object[] args) =>
        {
            var scope = args[1];
            var action = (Action)args[2];
            var st = new ServerThread(new BlockingCollection<ICommand>(), scope, action);
            threadList.Add((int)args[0], st);
            st.Start();
            return new object();
        }).Execute();
    }
}


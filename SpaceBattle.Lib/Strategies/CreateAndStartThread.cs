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
            var st = new ServerThread(new BlockingCollection<ICommand>());
            var action = (Action)args[1];
            threadList.Add((int)args[0], st);
            st.Start();
            action();

            return new object();
        }).Execute();
    }
}


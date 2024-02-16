using Hwdtech;
using System.Collections.Concurrent;

namespace SpaceBattle.Lib;

class CreateAndStartThread
{
    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Struct.ServerThread.CreateAndStart", (object[] args) =>
        {
            var st = new ServerThread(new BlockingCollection<ICommand>());
            var action = (Action)args[1];

            IoC.Resolve<IDictionary<int, ServerThread>>("Game.Struct.ServerThread.List").Add((int)args[0], st);
            st.Start();
            action();
        }).Execute();
    }
}
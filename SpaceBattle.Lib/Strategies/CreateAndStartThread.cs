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
                var guid = (Guid)args[0];
                var enterHook = (Action)args[1];
                var exitHook = (Action)args[2];
                var queue = new BlockingCollection<ICommand>();
                var st = new ServerThread(queue, enterHook, exitHook);

                IoC.Resolve<Hwdtech.ICommand>("IoC.Register", $"Game.Struct.ServerThread.HardStop{guid}", (object[] args) =>
                {
                    var hs = new HardStopCommand(st, (Action)args[0]);
                    var cmd = IoC.Resolve<ICommand>("Game.Struct.ServerThread.SendCommand", guid, hs);
                    return cmd;
                }).Execute();

                IoC.Resolve<Hwdtech.ICommand>("IoC.Register", $"Game.Struct.ServerThread.SoftStop{guid}", (object[] args) =>
                {
                    var ss = new SoftStopCommand(st, queue, (Action)args[0]);
                    var cmd = IoC.Resolve<ICommand>("Game.Struct.ServerThread.SendCommand", guid, ss);
                    return cmd;
                }).Execute();

                IoC.Resolve<Hwdtech.ICommand>("IoC.Register", $"Game.Struct.ServerThread.Queue{guid}", (object[] args) =>
                {
                    return queue;
                }).Execute();

                IoC.Resolve<Hwdtech.ICommand>("IoC.Register", $"Game.Struct.ServerThread.Get{guid}", (object[] args) =>
                {
                    return st;
                }).Execute();

                st.Start();
            };

            return new ActionCommand(act);
     
        }).Execute();
    }
}


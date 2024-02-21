using System.Runtime.Intrinsics.X86;
using Hwdtech;

namespace SpaceBattle.Lib;

public class SoftStopThread
{
    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Struct.ServerThread.SoftStop", (object[] args) =>
        {
            var thread = IoC.Resolve<Dictionary<int, ServerThread>>("Game.Struct.ServerThread.List")[(int)args[0]];
            var ss = new SoftStopCommand(thread, (Action)args[1]);
            var cmd = IoC.Resolve<ICommand>("Game.Struct.ServerThread.SendCommand", (int)args[0], ss);
            return cmd;
        }).Execute();
    }
}
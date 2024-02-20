using System.Runtime.Intrinsics.X86;
using Hwdtech;

namespace SpaceBattle.Lib;

public class SoftStopThread
{
    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Struct.ServerThread.SoftStop", (object[] args) =>
        {
            var id = (int)args[0];
            var ss = new SoftStopCommand(id);
            IoC.Resolve<object>("Game.Struct.ServerThread.SendCommand", id, ss);
            return new object();
        }).Execute();
    }
}
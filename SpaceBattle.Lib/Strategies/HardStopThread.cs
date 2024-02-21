using Hwdtech;

namespace SpaceBattle.Lib;

public class HardStopThread
{
    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Struct.ServerThread.HardStop", (object[] args) =>
        {
            var thread = IoC.Resolve<Dictionary<int, ServerThread>>("Game.Struct.ServerThread.List")[(int)args[0]];
            var hs = new HardStopCommand(thread, (Action)args[1]);
            var cmd = IoC.Resolve<ICommand>("Game.Struct.ServerThread.SendCommand", (int)args[0], hs);
            return cmd;
        }).Execute();
    }
}


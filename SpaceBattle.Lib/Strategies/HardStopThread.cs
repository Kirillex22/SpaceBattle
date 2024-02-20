using Hwdtech;

namespace SpaceBattle.Lib;

public class HardStopThread
{
    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Struct.ServerThread.HardStop", (object[] args) =>
        {
            var id = (int)args[0];
            var hs = new HardStopCommand(id, () => { });
            var cmd = IoC.Resolve<object>("Game.Struct.ServerThread.SendCommand", id, hs);
            return cmd;
        }).Execute();
    }
}


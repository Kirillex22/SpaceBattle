using Hwdtech;

namespace SpaceBattle.Lib;

public class SendCommand
{
    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Struct.ServerThread.SendCommand", (object[] args) =>
        {
            var thread = IoC.Resolve<Dictionary<int, ServerThread>>("Game.Struct.ServerThread.List")[(int)args[0]];
            return new SendCmdCommand(thread, (ICommand)args[1]);
        }).Execute();
    }
}


using Hwdtech;

namespace SpaceBattle.Lib;

public class SendCommand
{
    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Struct.ServerThread.SendCommand", (object[] args) =>
        {
            return new SendCmdCommand((int)args[0], (ICommand)args[1]);
        }).Execute();
    }
}


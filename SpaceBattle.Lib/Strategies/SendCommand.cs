using Hwdtech;

namespace SpaceBattle.Lib;

class SendCommand
{
    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Struct.ServerThread.SendCommand", (object[] args) =>
        {
            IoC.Resolve<Dictionary<int, ServerThread>>("Game.Struct.ServerThread.List")[(int)args[0]].Send((ICommand)args[1]);
        }).Execute();
    }
}
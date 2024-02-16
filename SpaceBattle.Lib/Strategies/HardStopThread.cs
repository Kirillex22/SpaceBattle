using Hwdtech;

namespace SpaceBattle.Lib;

class HardStopThread
{
    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Struct.ServerThread.HardStop", (object[] args) =>
        {
            IoC.Resolve<Dictionary<int, ServerThread>>("Game.Struct.ServerThread.List")[(int)args[0]].Send(new HardStopCommand((int)args[0]));
        }).Execute();
    }
}
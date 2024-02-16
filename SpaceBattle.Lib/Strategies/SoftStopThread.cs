using Hwdtech;

namespace SpaceBattle.Lib;

class SoftStopThread
{
    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Struct.ServerThread.HardStop", (object[] args) =>
        {
            IoC.Resolve<Dictionary<int, ServerThread>>("Game.Struct.ServerThread.List")[(int)args[0]].Send(new SoftStopCommand((int)args[0]));
        }).Execute();
    }
}
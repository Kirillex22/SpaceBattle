using Hwdtech;

namespace SpaceBattle.Lib;

public class CreateThreadList
{
    private Dictionary<int, ServerThread> _stList;
    public void Call()
    {
        _stList = new Dictionary<int, ServerThread>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Struct.ServerThread.List", (object[] args) =>
        {
            return _stList;
        }).Execute();
    }
}
using Hwdtech;
using System.Collections.Concurrent;

namespace SpaceBattle.Lib;

public class CreateThreadList
{
    private Dictionary<int, ServerThread> _stList;
    private Dictionary<int, BlockingCollection<SpaceBattle.Lib.ICommand>> _qList;
    public void Call()
    {
        _stList = new Dictionary<int, ServerThread>();
        _qList = new Dictionary<int, BlockingCollection<SpaceBattle.Lib.ICommand>>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Struct.ServerThread.List", (object[] args) =>
        {
            return _stList;
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Struct.ServerThreadQueue.List", (object[] args) =>
        {
            return _qList;
        }).Execute();
    }
}
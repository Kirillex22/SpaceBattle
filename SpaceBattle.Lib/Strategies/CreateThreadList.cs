using Hwdtech;
using System.Collections.Concurrent;

namespace SpaceBattle.Lib;

public class CreateThreadList
{
    private Dictionary<int, ServerThread> _stList = new();
    private Dictionary<int, BlockingCollection<SpaceBattle.Lib.ICommand>> _qList = new();
    public void Call()
    {
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
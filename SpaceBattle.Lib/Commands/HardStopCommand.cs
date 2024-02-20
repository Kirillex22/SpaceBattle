using Hwdtech;

namespace SpaceBattle.Lib;

public class HardStopCommand : ICommand
{
    private int _threadId;
    private Action _exitAction = () => { };
    public HardStopCommand(int threadId)
    {
        _threadId = threadId;
    }

    public HardStopCommand(int threadId, Action exitAction)
    {
        _threadId = threadId;
        _exitAction = exitAction;
    }

    public void Execute()
    {
        IoC.Resolve<Dictionary<int, ServerThread>>("Game.Struct.ServerThread.List")[_threadId].Stop();
        Console.Write($"HS AT {_threadId} WAS EXECUTED");
        _exitAction();
    }
}
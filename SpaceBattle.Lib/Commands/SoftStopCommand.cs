using System.Collections.Concurrent;
using Hwdtech;

namespace SpaceBattle.Lib;

class SoftStopCommand : ICommand
{
    private int _threadId;
    private Action _exitAction = () => { };
    public SoftStopCommand(int threadId)
    {
        _threadId = threadId;
    }

    public SoftStopCommand(int threadId, Action exitAction)
    {
        _threadId = threadId;
        _exitAction = exitAction;
    }

    public void Execute()
    {
        var threadQueue = IoC.Resolve<BlockingCollection<ICommand>>($"Game.Struct.ServerThreadQueue_{_threadId}");

        var thread = IoC.Resolve<Dictionary<int, ServerThread>>($"Game.Struct.ServerThread.List")[_threadId];

        thread.ChangeBehaviour(() =>
        {
            if (threadQueue.Count == 0)
            {
                thread.Stop();
                _exitAction();
            }

            var cmd = threadQueue.Take();

            try
            {
                cmd.Execute();
            }
            catch (Exception e)
            {
                IoC.Resolve<ICommand>("Exception.Handle", cmd, e).Execute();
            }
        });
    }
}
using System.Collections.Concurrent;
using Hwdtech;

namespace SpaceBattle.Lib;

class SoftStopCommand : ICommand
{
    private int _threadId;
    public SoftStopCommand(int threadId)
    {
        _threadId = threadId;
    }

    public void Execute()
    {
        var threadQueue = IoC.Resolve<BlockingCollection<ICommand>>($"Game.Struct.ServerThreadQueue_{_threadId}");

        var thread = IoC.Resolve<ServerThread>($"Game.Struct.ServerThread_{_threadId}");

        thread.ChangeBehaviour(() =>
        {
            if (threadQueue.Count == 0)
            {
                thread.Stop();
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
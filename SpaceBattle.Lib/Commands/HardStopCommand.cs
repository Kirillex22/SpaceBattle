using System.Collections.Concurrent;
using Hwdtech;

namespace SpaceBattle.Lib;

class HardStopCommand : ICommand
{
    private int _threadId;
    public HardStopCommand(int threadId)
    {
        _threadId = threadId;
    }

    public void Execute()
    {
        IoC.Resolve<ServerThread>($"Game.Struct.ServerThread_{_threadId}").Stop();
    }
}

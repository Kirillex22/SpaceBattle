using Hwdtech;

namespace SpaceBattle.Lib;

public class SendCmdCommand : ICommand
{
    int _threadId;
    ICommand _cmd;
    public SendCmdCommand(int threadId, ICommand cmd)
    {
        _threadId = threadId;
        _cmd = cmd;
    }

    public void Execute()
    {
        IoC.Resolve<Dictionary<int, ServerThread>>("Game.Struct.ServerThread.List")[_threadId].GetQueue().Add(_cmd);
    }
}
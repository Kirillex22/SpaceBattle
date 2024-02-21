using Hwdtech;

namespace SpaceBattle.Lib;

public class SendCmdCommand : ICommand
{
    private ServerThread _thread;
    private ICommand _cmd;
    public SendCmdCommand(ServerThread thread, ICommand cmd)
    {
        _thread = thread;
        _cmd = cmd;
    }

    public void Execute()
    {
        _thread.ThreadQueue.Add(_cmd);
    }
}
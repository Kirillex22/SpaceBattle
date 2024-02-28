using Hwdtech;
using System.Collections.Concurrent;

namespace SpaceBattle.Lib;

public class SendCmdCommand : ICommand
{
    private BlockingCollection<ICommand> _queue;
    private ICommand _cmd;
    public SendCmdCommand(BlockingCollection<ICommand> queue, ICommand cmd)
    {
        _queue = queue;
        _cmd = cmd;
    }

    public void Execute()
    {
        _queue.Add(_cmd);
    }
}
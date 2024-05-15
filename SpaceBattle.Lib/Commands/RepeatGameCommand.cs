using System.Collections;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;

public class RepeatGameCommand
{
    private ICommand _cmd;

    public RepeatGameCommand(ICommand cmd)
    {
        _cmd = cmd;
    }

    public void Execute()
    {
        var threadQeue = IoC.Resolve<BlockingCollection<ICommand>>("Server.Thread.Queue");
        threadQeue.Add(_cmd);
        
        return threadQeue;
    }
}


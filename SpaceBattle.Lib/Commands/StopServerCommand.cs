using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;

public class StopServerCommand : ICommand
{
    public void Execute()
    {
        IoC.Resolve<Dictionary<int, object>>("Server.Thread.Handle").ToList().ForEach(i => 
        IoC.Resolve<ICommand>("Server.Command.Send", i.Key, IoC.Resolve<ICommand>("Server.Thread.Stop", i.Key)).Execute());
    }
}


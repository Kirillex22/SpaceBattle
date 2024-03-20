using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;

public class StopServerCommand : ICommand
{
    public void Execute()
    {
        IoC.Resolve<Dictionary<int, object>>("Server.Thread.Map").ToList().ForEach(i => 
        IoC.Resolve<ICommand>("Server.Command.Send", i.Key, IoC.Resolve<ICommand>("Server.Thread.Stop", i.Key, () =>
        {IoC.Resolve<ICommand>("Server.Thread.Barrier.Create").Execute();})).Execute());

        IoC.Resolve<ICommand>("Server.Thread.Barrier.Check").Execute();
    }
}


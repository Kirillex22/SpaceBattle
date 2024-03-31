using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;

public class StartServerCommand : ICommand
{ 
    private int _count;
    
    public StartServerCommand(int count)
    {
        _count = count;
    }

    public void Execute()
    {
        var threadsId = Enumerable.Range(0, _count).ToArray();

        Array.ForEach(threadsId, i =>
        {
            IoC.Resolve<ICommand>("Server.Create.Thread", i, () => 
            {IoC.Resolve<ICommand>("Server.Thread.Barrier.Create").Execute();}).Execute();
        });

        IoC.Resolve<ICommand>("Server.Thread.Barrier.Check").Execute();
    }
}


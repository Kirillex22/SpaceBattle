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
        for (int i = 0; i<_count; i++)
        {
            IoC.Resolve<ICommand>("Server.Create.Thread", i).Execute();
        }
    }
}


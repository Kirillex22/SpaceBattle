using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;

public class MacroCommand : ICommand
{
    private ICommand[] _innerCommands;
    public MacroCommand(ICommand[] innerCommands)
    {
        _innerCommands = innerCommands;
    }

    public void Execute()
    {
        _innerCommands.ToList().ForEach((ICommand cmd) => cmd.Execute());
    }
}


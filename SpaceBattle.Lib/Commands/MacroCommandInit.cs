using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;

public class MacroCommandInit : ICommand
{
    private IEnumerable<ICommand> _innerCommands;

    public MacroCommandInit(IEnumerable<ICommand> innerCommands)
    {
        _innerCommands = innerCommands;
    }

    public void Execute()
    {
        _innerCommands.ToList().ForEach((ICommand cmd) => cmd.Execute());
    }
}


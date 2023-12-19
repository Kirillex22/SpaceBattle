using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;

public class MacroCommand : ICommand
{
    private List<ICommand> _innerCommands;
    public MacroCommand(List<ICommand> innerCommands)
    {
        _innerCommands = innerCommands;
    }

    public void Execute()
    {
        _innerCommands.ForEach((ICommand cmd) => cmd.Execute());
    }
}


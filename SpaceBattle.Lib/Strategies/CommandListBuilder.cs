using System.Collections;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;
public class CommandListBuilder : IStrategy
{
    private string _dependency;

    public CommandListBuilder(string dependency)
    {
        _dependency = dependency;
    }

    public object Call()
    {
        var commands = new List<ICommand>();
        var commandsNames = IoC.Resolve<string[]>(_dependency);

        commandsNames.ToList().ForEach((string name) => commands.Add(IoC.Resolve<ICommand>(name)));

        return commands;
    }
}


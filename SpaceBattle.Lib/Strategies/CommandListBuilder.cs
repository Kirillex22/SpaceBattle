using System.Collections;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;
public class CommandListBuilder
{
    public ICommand[] Call(string dependency)
    {
        var commands = new List<ICommand>();
        var commandsNames = IoC.Resolve<string[]>(dependency);

        commandsNames.ToList().ForEach((string name) => commands.Add(IoC.Resolve<ICommand>(name)));

        return commands.ToArray();
    }
}


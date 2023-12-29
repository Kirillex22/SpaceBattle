using System.Collections;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;
public class MacroCommandBuilder
{
    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.MacroCommand.Create", (object[] args) =>
        {
            var cmdbuilder = new CommandListBuilder();
            var cmds = cmdbuilder.Call((string)args[0]);
            var macrocmd = new MacroCommand(cmds);
            return macrocmd;

        }).Execute();
    }
}


using System.Collections;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;

public class CreateNewGameStrategy
{
    public object Run(params object[] args)
    {
        var gameId = (int)args[0];
        var quant = (double)args[1];
        var mainScope = (object)args[2];

        var gameQueue = IoC.Resolve<object>("Game.Queue.Create");
        var gameScope = IoC.Resolve<object>("Game.Scope.Create", gameId, quant, mainScope);
        var gameLikeCmd = IoC.Resolve<ICommand>("Game.Command", gameQueue, gameScope);

        var cmdList = new List<ICommand> {gameLikeCmd};
        var macroCmd = IoC.Resolve<ICommand>("Game.Command.Macro", cmdList);
        var injectCmd = IoC.Resolve<ICommand>("Game.Command.Inject", macroCmd);
        var repeatCmd = IoC.Resolve<ICommand>("Game.Command.Repeat", injectCmd);
        cmdList.Add(repeatCmd);

        var gameMap = IoC.Resolve<IDictionary<int, ICommand>>("Game.Map");
        gameMap.Add(gameId, injectCmd);

        return injectCmd;
    }
}


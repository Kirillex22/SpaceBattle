using System.Collections;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;

public class CreateNewScopeStrategy
{
    public object Run(params object[] args)
    {
        var gameId = (int)args[0];
        var quant = (double)args[1];
        var mainScope = (object)args[2];

        var gameScope = IoC.Resolve<object>("Scopes.New", mainScope);
        var scopeMap = IoC.Resolve<IDictionary<int, object>>("Scope.Map");
        scopeMap.Add(gameId, gameScope);
        
        IoC.Resolve<ICommand>("Scopes.Current.Set", gameScope).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Game.Get.Quantum", (object[] args) => (object)quant).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Queue.Push", (object[] args) => new PushQueueStrategy().Run(args)).Execute();
        
        IoC.Resolve<ICommand>("IoC.Register", "Game.Queue.Pop", (object[] args) => new GameQueueDeleteStrategy().Run(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.UObject", (object[] args) => new GetUObjectStrategy().Run(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.UObject.Delete", (object[] args) => new DeleteUObjectStrategy().Run(args)).Execute();
        
        IoC.Resolve<ICommand>("Scopes.Current.Set", mainScope).Execute();

        return gameScope;
    }
}


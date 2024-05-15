using System;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;

public class GameQueueDeleteStrategyTests
{
    public GameQueueDeleteStrategyTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();        
    }

    [Fact]
    public void GameQueueDeleteStrategySuccessful()
    {
        var queueMap = new Dictionary<int, Queue<SpaceBattle.Lib.ICommand>>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Queue.Delete", (object[] args) => new GameQueueDeleteStrategy().Run(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Get.Queue", (object[] args) => new GetGameQueueStrategy().Run(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Queue.Dict", (object[] args) => queueMap).Execute();
        
        var moqCmd = new Mock<SpaceBattle.Lib.ICommand>();
        var queue = new Queue<SpaceBattle.Lib.ICommand>();

        queue.Enqueue(moqCmd.Object);
        queueMap.Add(0, queue);

        Assert.Single(queue);
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Queue.Delete", 0).Execute();

        Assert.Empty(queue);
    }   
}


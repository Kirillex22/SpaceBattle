using System;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Test;

public class RegisterCommandTests
{
    public RegisterCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();        
    }

    [Fact]
    public void ShootCommandTestSuccessful()
    {
        var queue = new Queue<SpaceBattle.Lib.ICommand>();
        var moqCmd = new Mock<SpaceBattle.Lib.ICommand>();
        var moqShoot = new Mock<IShootable>();
        var moqGetId = new Mock<IStrategy>();
        var moqUObj = new Mock<object>();
        moqGetId.Setup(i => i.Run()).Returns(123).Verifiable();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Queue.Push", (object[] args) => new ActionCommand( () => queue.Enqueue((SpaceBattle.Lib.ICommand)args[1]))).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Get.GameID", (object[] args) => moqGetId.Object.Run(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Create.Bullet", (object[] args) => moqUObj.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Create.Bullet.Move", (object[] args) => moqCmd.Object).Execute();

        new ShootCommand(moqShoot.Object).Execute();

        Assert.True(queue.Count == 1);

        moqGetId.Verify(i => i.Run(), Times.Once);
    }
}


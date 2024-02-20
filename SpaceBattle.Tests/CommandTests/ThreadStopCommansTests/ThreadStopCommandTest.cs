using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests;

public class ThreadStopCommandsTest
{
    public ThreadStopCommandsTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Exception.Handler", (object[] args) => new EmptyCommand()).Execute();

        var cmd = new Mock<SpaceBattle.Lib.ICommand>();
        cmd.Setup(c => c.Execute()).Verifiable();

        new CreateThreadList().Call();
        new CreateAndStartThread().Call();
        new SendCommand().Call();
        new HardStopThread().Call();
        new SoftStopThread().Call();
    }

    [Fact]
    public void SuccesfulHardStop()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        var threadList = IoC.Resolve<Dictionary<int, ServerThread>>("Game.Struct.ServerThread.List");

        IoC.Resolve<object>("Game.Struct.ServerThread.CreateAndStart", 1, () => { });
        IoC.Resolve<object>("Game.Struct.ServerThread.CreateAndStart", 2, () => { });

        var cmds = new List<SpaceBattle.Lib.ICommand>(){
            new EmptyCommand(),
            new EmptyCommand(),
            new EmptyCommand(),
            new EmptyCommand()
        };

        //cmds.ForEach(c => IoC.Resolve<object>("Game.Struct.ServerThread.SendCommand", 1, c));
        //cmds.ForEach(c => IoC.Resolve<object>("Game.Struct.ServerThread.SendCommand", 2, c));

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.HardStop", 1).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.HardStop", 2).Execute();

        Assert.True(threadList[1].Status());
        Assert.True(threadList[2].Status());
    }
}
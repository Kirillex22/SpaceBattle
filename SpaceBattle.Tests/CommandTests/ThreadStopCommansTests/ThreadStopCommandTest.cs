using Hwdtech;
using Moq;
using Hwdtech.Ioc;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests;

public class ThreadStopCommandsTest
{
    public ThreadStopCommandsTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Exception.Handler", (object[] args) =>
        {
            return new EmptyCommand();

        }).Execute();

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
        var threadList = IoC.Resolve<Dictionary<int, ServerThread>>("Game.Struct.ServerThread.List");
        var currentScope = IoC.Resolve<object>("Scopes.Current");
        IoC.Resolve<object>("Game.Struct.ServerThread.CreateAndStart", 1, currentScope, () => { });
        IoC.Resolve<object>("Game.Struct.ServerThread.CreateAndStart", 2, currentScope, () => { });

        var cmds = new List<SpaceBattle.Lib.ICommand>(){
            new EmptyCommand(),
            new EmptyCommand(),
            new EmptyCommand(),
            new EmptyCommand()
        };

        cmds.ForEach(c => IoC.Resolve<object>("Game.Struct.ServerThread.SendCommand", 1, c));
        cmds.ForEach(c => IoC.Resolve<object>("Game.Struct.ServerThread.SendCommand", 2, c));

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.HardStop", 1, () => { }).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.HardStop", 2, () => { }).Execute();

        Thread.Sleep(300);

        Assert.False(threadList[1].Status());
        Assert.False(threadList[2].Status());
        threadList.Remove(1);
        threadList.Remove(2);
    }

    [Fact]
    public void SuccesfulSoftStopWithIocWokingIntoThread()
    {
        var currentScope = IoC.Resolve<object>("Scopes.Current");
        var threadList = IoC.Resolve<Dictionary<int, ServerThread>>("Game.Struct.ServerThread.List");

        IoC.Resolve<object>("Game.Struct.ServerThread.CreateAndStart", 11, currentScope, () => { });
        IoC.Resolve<object>("Game.Struct.ServerThread.CreateAndStart", 23, currentScope, () => { });

        var c1 = new Mock<SpaceBattle.Lib.ICommand>();
        var c2 = new Mock<SpaceBattle.Lib.ICommand>();
        var c3 = new Mock<SpaceBattle.Lib.ICommand>();
        var c11 = new Mock<SpaceBattle.Lib.ICommand>();
        var c21 = new Mock<SpaceBattle.Lib.ICommand>();
        var c31 = new Mock<SpaceBattle.Lib.ICommand>();


        var th1 = new Thread(() =>
        {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", currentScope).Execute();
            c1.Setup(c => c.Execute()).Throws(new ArgumentException()).Verifiable();
            c2.Setup(c => c.Execute()).Verifiable();
            c3.Setup(c => c.Execute()).Verifiable();

            IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", 11, c1.Object).Execute();
            IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SoftStop", 11, () => { }).Execute();
            IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", 11, c2.Object).Execute();
            IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", 11, c3.Object).Execute();
        });

        var th2 = new Thread(() =>
        {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", currentScope).Execute();
            c11.Setup(c => c.Execute()).Throws(new ArgumentException()).Verifiable();
            c21.Setup(c => c.Execute()).Verifiable();
            c31.Setup(c => c.Execute()).Verifiable();

            IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", 23, c11.Object).Execute();
            IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SoftStop", 23, () => { }).Execute();
            IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", 23, c21.Object).Execute();
            IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", 23, c31.Object).Execute();
        });

        th1.Start();
        th2.Start();

        Mock.Verify(c1, c2, c3, c11, c21, c31);

        Assert.False(threadList[11].Status());
        Assert.False(threadList[23].Status());

        threadList.Remove(11);
        threadList.Remove(23);
    }
}


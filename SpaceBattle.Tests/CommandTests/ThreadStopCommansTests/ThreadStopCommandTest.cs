using Hwdtech;
using Hwdtech.Ioc;
using Microsoft.VisualBasic;
using Moq;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests;

public class ThreadStopCommandsTest
{
    public ThreadStopCommandsTest()
    {
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

        IoC.Resolve<object>("Game.Struct.ServerThread.CreateAndStart", 1, () => { });
        IoC.Resolve<object>("Game.Struct.ServerThread.CreateAndStart", 2, () => { });

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

        Assert.True(threadList[1].Status());
        Assert.True(threadList[2].Status());
        threadList.Remove(1);
        threadList.Remove(2);
    }

    [Fact]
    public void SuccesfulSoftStop()
    {
        var threadList = IoC.Resolve<Dictionary<int, ServerThread>>("Game.Struct.ServerThread.List");

        IoC.Resolve<object>("Game.Struct.ServerThread.CreateAndStart", 11, () => { });
        IoC.Resolve<object>("Game.Struct.ServerThread.CreateAndStart", 23, () => { });

        var c1 = new Mock<SpaceBattle.Lib.ICommand>();
        var c2 = new Mock<SpaceBattle.Lib.ICommand>();
        var c3 = new Mock<SpaceBattle.Lib.ICommand>();

        c1.Setup(c => c.Execute()).Verifiable();
        c2.Setup(c => c.Execute()).Verifiable();
        c3.Setup(c => c.Execute()).Verifiable();

        var cmds = new List<Mock<SpaceBattle.Lib.ICommand>>() { c1, c2, c3 };

        cmds.ForEach(c => IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", 11, c.Object).Execute());
        cmds.ForEach(c => IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", 23, c.Object).Execute());

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SoftStop", 11, () => { }).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SoftStop", 23, () => { }).Execute();

        cmds.ForEach(c => IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", 11, c.Object).Execute());
        cmds.ForEach(c => IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", 23, c.Object).Execute());

        Thread.Sleep(300);

        Mock.Verify(c1, c2, c3);
        Assert.True(threadList[11].Status());
        Assert.True(threadList[23].Status());

        threadList.Remove(11);
        threadList.Remove(23);
    }
}


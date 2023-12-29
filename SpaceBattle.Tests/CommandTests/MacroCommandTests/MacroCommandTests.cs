using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests;

public class MacroCommandTests
{
    public MacroCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.MacroCommandNames.SimpleMacroCommand", (object[] args) =>
        {
            return new string[]
            {
                "Game.Command.SomeCommand1",
                "Game.Command.SomeCommand2",
                "Game.Command.SomeCommand3"
            };
        }).Execute();

        new MacroCommandBuilder().Call();
    }

    [Fact]
    public void SuccesfulMacroCommandExecuting()
    {
        var cmd1 = new Mock<SpaceBattle.Lib.ICommand>();
        var cmd2 = new Mock<SpaceBattle.Lib.ICommand>();
        var cmd3 = new Mock<SpaceBattle.Lib.ICommand>();

        cmd1.Setup(c => c.Execute()).Callback(() => { }).Verifiable();
        cmd2.Setup(c => c.Execute()).Callback(() => { }).Verifiable();
        cmd3.Setup(c => c.Execute()).Callback(() => { }).Verifiable();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.SomeCommand1", (object[] args) => cmd1.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.SomeCommand2", (object[] args) => cmd2.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.SomeCommand3", (object[] args) => cmd3.Object).Execute();

        var macro = IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.MacroCommand.Create", "Game.MacroCommandNames.SimpleMacroCommand");

        macro.Execute();

        Mock.Verify(cmd1, cmd2, cmd3);
    }

    [Fact]
    public void SomeCommandDoesntExecuting()
    {
        var cmd1 = new Mock<SpaceBattle.Lib.ICommand>();
        var cmd2 = new Mock<SpaceBattle.Lib.ICommand>();
        var cmd3 = new Mock<SpaceBattle.Lib.ICommand>();

        cmd1.Setup(c => c.Execute()).Callback(() => { });
        cmd2.Setup(c => c.Execute()).Throws(new Exception("run error")).Verifiable();
        cmd3.Setup(c => c.Execute()).Callback(() => { });

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.SomeCommand1", (object[] args) => cmd1.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.SomeCommand2", (object[] args) => cmd2.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.SomeCommand3", (object[] args) => cmd3.Object).Execute();

        var macro = IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.MacroCommand.Create", "Game.MacroCommandNames.SimpleMacroCommand");
        var exc = Assert.Throws<Exception>(() => macro.Execute());

        Assert.Equal("run error", exc.Message);
    }
}


using System;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Test;

public class CreateMacroCommandTests
{
    public CreateMacroCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();        
    }

    [Fact]
    public void CreateMacroCommandTestsSuccessful()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Create.MacroCommand", (object[] args) => new CreateMacroCommandStrategy().Run(args)).Execute();

        List<string> dependencies = new List<string>{"Test"};
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Dependencies.Get.Macro.Test", (object[] args) => dependencies).Execute();

        var moqCmd = new Mock<Lib.ICommand>();
        moqCmd.Setup(i => i.Execute()).Verifiable();
        var moqUObj = new Mock<IUObject>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Test", (object[] args) => moqCmd.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Macro", (object[] args) => new MacroCommandInit((IEnumerable<SpaceBattle.Lib.ICommand>)args[0])).Execute();

        var macroCmd = IoC.Resolve<Lib.ICommand>("Game.Create.MacroCommand", moqUObj.Object, "Test");
        macroCmd.Execute();

        moqCmd.Verify(i => i.Execute(), Times.Once);
    }
}


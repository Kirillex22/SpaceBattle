using SpaceBattle.Lib;
using TechTalk.SpecFlow;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Tests;

public class MacroLongOperationTests
{
    private Mock<SpaceBattle.Lib.ICommand> _MoqCommand;
    private Mock<IUObject> _MoqObj;
    private string _dependency;

    public MacroLongOperationTests()
    {
        _MoqCommand = new Mock<SpaceBattle.Lib.ICommand>();
        _MoqObj = new Mock<IUObject>();
        var MoqDelegate = new Mock<Func<object[], object>>();
        MoqDelegate.Setup(i => i(It.IsAny<object[]>())).Returns(_MoqCommand.Object);

        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Create.LongMacro", (object[] args) => _MoqCommand.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.LongOperation", (object[] args) => _MoqCommand.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Queue.Push", (object[] args) => _MoqCommand.Object).Execute();
    }

    [Fact]
    public void CreateMacroLongOperationSuccess()
    {       
        _dependency = "NewDependency";

        _MoqCommand.Setup(i => i.Execute()).Verifiable();
        var LongOperatin = new MacroLongOperation(_dependency, _MoqObj.Object);
        LongOperatin.Execute();

        _MoqCommand.Verify(i => i.Execute(), Times.Exactly(3));
    }
}


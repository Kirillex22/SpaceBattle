using SpaceBattle.Lib;
using TechTalk.SpecFlow;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Tests;

public class RegisterExcHandlerTests
{
    public RegisterExcHandlerTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var DecisionTree = new Dictionary<object, object>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Get.ExceptionTree", (object[] args) => {
            return DecisionTree;
        }).Execute();
    }

    [Fact]
    public void RegisterExcHandlerSuccess()
    {
        var cmd = typeof(Turn);
        var exc = typeof(ArgumentException);
        var MoqCommand = new Mock<SpaceBattle.Lib.ICommand>();

        var NewExcHandler = new RegisterExcHandler(MoqCommand.Object, cmd.ToString(), exc.ToString());
        NewExcHandler.Execute();

        var ExceptionTree = IoC.Resolve<IDictionary<object, object>>("Game.Get.ExceptionTree");
        Assert.NotNull(ExceptionTree);
        Assert.True(ExceptionTree.ContainsKey(cmd.ToString()));

        var NextExceptionTree = (IDictionary<object, object>)ExceptionTree[cmd.ToString()];
        Assert.True(NextExceptionTree.ContainsKey(exc.ToString()));
    }

    [Fact]
    public void RegisterDefaultTypeCommand()
    {
        var NewExc = typeof(ArgumentException);
        var MoqCommand = new Mock<SpaceBattle.Lib.ICommand>();

        var NewExcHandler = new RegisterExcHandler(MoqCommand.Object, exc: NewExc.ToString());
        NewExcHandler.Execute();

        var ExceptionTree = IoC.Resolve<IDictionary<object, object>>("Game.Get.ExceptionTree");
        Assert.NotNull(ExceptionTree);
        Assert.True(ExceptionTree.ContainsKey("default"));

        var NextExceptionTree = (IDictionary<object, object>)ExceptionTree["default"];
        Assert.True(NextExceptionTree.ContainsKey(NewExc.ToString()));
    }

    [Fact]
    public void RegisterDefaultTypeException()
    {
        var TurnCommand = typeof(Turn);
        var MoqCommand = new Mock<SpaceBattle.Lib.ICommand>();

        var NewExcHandler = new RegisterExcHandler(MoqCommand.Object, cmd: TurnCommand.ToString());
        NewExcHandler.Execute();

        var ExceptionTree = IoC.Resolve<IDictionary<object, object>>("Game.Get.ExceptionTree");
        Assert.NotNull(ExceptionTree);
        Assert.True(ExceptionTree.ContainsKey(TurnCommand.ToString()));

        var NextExceptionTree = (IDictionary<object, object>)ExceptionTree[TurnCommand.ToString()];
        Assert.True(NextExceptionTree.ContainsKey("default"));
    }

    [Fact]
    public void RegisterDefaultTypeExceptionAndDefaultTypeCommand()
    {
        var MoqCommand = new Mock<SpaceBattle.Lib.ICommand>();

        var NewExcHandler = new RegisterExcHandler(MoqCommand.Object);
        NewExcHandler.Execute();

        var ExceptionTree = IoC.Resolve<IDictionary<object, object>>("Game.Get.ExceptionTree");
        Assert.NotNull(ExceptionTree);
        Assert.True(ExceptionTree.ContainsKey("default"));

        var NextExceptionTree = (IDictionary<object, object>)ExceptionTree["default"];
        Assert.True(NextExceptionTree.ContainsKey("default"));
    }
}


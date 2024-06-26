using Hwdtech;
using Hwdtech.Ioc;
using SpaceBattle.Lib;
using System.Collections;
using Moq;

namespace SpaceBattle.Tests;

public class StartMoveCommandTest
{
    public StartMoveCommandTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
             IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.IUObject.SetProperty",
            (object[] args) =>
            {
                var target = (IUObject)args[0];
                var key = (string)args[1];
                var value = args[2];

                target.SetProperty(key, value);
                return new Mock<SpaceBattle.Lib.ICommand>().Object;
            }
        ).Execute();

        var initCmd = new Mock<SpaceBattle.Lib.ICommand>();
        var bridgeCmd = new BridgeCommand(initCmd.Object);

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.Inject",
            (object[] args) =>
            {
                var cmd = (SpaceBattle.Lib.ICommand)args[0];
                bridgeCmd.Inject(cmd);
                return bridgeCmd;
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.Move",
            (object[] args) =>
            {
                var target = (IUObject)args[0];
                var moveCmd = new Mock<SpaceBattle.Lib.ICommand>();
                return moveCmd.Object;
            }
        ).Execute();
    }

    [Fact]
    public void SuccefulExecuting()
    {
        var queue = new Mock<IQueue>();
        var realQueue = new Queue<SpaceBattle.Lib.ICommand>();

        queue.Setup(q => q.Push(It.IsAny<SpaceBattle.Lib.ICommand>())).Callback(realQueue.Enqueue);

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Queue",
            (object[] args) =>
            {
                return queue.Object;
            }
        ).Execute();

        var startable = new Mock<IMoveStartable>();
        var target = new Mock<IUObject>();
        var initialValues = new Dictionary<string, object> { { "position", new object() } };
        var settedValues = new Dictionary<string, object>();

        startable.SetupGet(s => s.InitialValues).Returns(initialValues);
        startable.SetupGet(s => s.Target).Returns(target.Object);
        startable.SetupGet(s => s.Command).Returns("Move");

        target.Setup(
            t => t.SetProperty(
                It.IsAny<string>(),
                It.IsAny<object>()
                )
        ).Callback<string, object>(settedValues.Add);

        var smc = new StartMoveCommand(startable.Object);

        smc.Execute();

        Assert.True(settedValues.ContainsKey("position") && settedValues.ContainsKey("command"));
        Assert.NotEmpty(realQueue);
    }

    [Fact]
    public void InitialValuesSetException()
    {
        var startable = new Mock<IMoveStartable>();
        var target = new Mock<IUObject>();
        var initialValues = new Dictionary<string, object> { { "velocity", new object() } };
        var settedValues = new Dictionary<string, object>();

        startable.SetupGet(s => s.InitialValues).Returns(initialValues);
        startable.SetupGet(s => s.Target).Returns(target.Object);

        target.Setup(
            t => t.SetProperty(
                It.IsAny<string>(),
                It.IsAny<object>()
                )
        ).Callback(() => throw new Exception());

        var smc = new StartMoveCommand(startable.Object);

        Assert.Throws<Exception>(() => smc.Execute());
    }

    [Fact]
    public void InitialValuesGetException()
    {
        var startable = new Mock<IMoveStartable>();
        startable.SetupGet(s => s.InitialValues).Throws(new Exception());
        var smc = new StartMoveCommand(startable.Object);

        Assert.Throws<Exception>(() => smc.Execute());
    }

    [Fact]
    public void TargetGetException()
    {
        var startable = new Mock<IMoveStartable>();
        var initialValues = new Dictionary<string, object> { { "position", new object() } };

        startable.SetupGet(s => s.Target).Throws(new Exception());
        startable.SetupGet(s => s.InitialValues).Returns(initialValues);

        var smc = new StartMoveCommand(startable.Object);

        Assert.Throws<Exception>(() => smc.Execute());
    }

    [Fact]
    public void OperationNameGetException()
    {
        var startable = new Mock<IMoveStartable>();
        var target = new Mock<IUObject>();
        var initialValues = new Dictionary<string, object> { { "velocity", new object() } };
        var settedValues = new Dictionary<string, object>();

        startable.SetupGet(s => s.InitialValues).Returns(initialValues);
        startable.SetupGet(s => s.Target).Returns(target.Object);
        startable.SetupGet(s => s.Command).Throws(new Exception());

        target.Setup(
            t => t.SetProperty(
                It.IsAny<string>(),
                It.IsAny<object>()
                )
        ).Callback(settedValues.Add);

        var smc = new StartMoveCommand(startable.Object);

        Assert.Throws<Exception>(() => smc.Execute());
    }
}


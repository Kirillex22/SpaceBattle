using System;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;

public class InterpretatorCommandTests
{   
    Dictionary<int, IUObject> uobjDict = new Dictionary<int, IUObject>(){{1, new Mock<IUObject>().Object}};
    Dictionary<int, Queue<SpaceBattle.Lib.ICommand>> queueDict = new Dictionary<int, Queue<SpaceBattle.Lib.ICommand>>();

    public InterpretatorCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();        
    }

    [Fact]
    public void TestExecute()
    {
        var moqCmd = new Mock<ICommand>();
        moqCmd.Setup(i => i.Execute());

        var moqMsg = new Mock<IMessage>();
        moqMsg.Setup(i => i.GameId).Throws<Exception>();

        var moqReturnStrategy = new Mock<IStrategy>();
        moqReturnStrategy.Setup(i => i.Run(It.IsAny<IMessage>())).Returns(moqCmd.Object);

        var queue = new Queue<ICommand>();

        var moqPushCmd = new Mock<ICommand>();
        moqPushCmd.Setup(i => i.Execute()).Callback(() => { queue.Enqueue(moqCmd.Object); });

        var moqPushStrategy = new Mock<IStrategy>();
        moqPushStrategy.Setup(i => i.Run(It.IsAny<int>(), It.IsAny<ICommand>())).Returns(moqPushCmd.Object).Verifiable();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Create", (object[] args) => moqReturnStrategy.Object.Run(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Queue.Push", (object[] args) => moqPushStrategy.Object.Run(args)).Execute();

        var interpreterCommand = new InterpretatorCommand(moqMsg.Object);

        Assert.Throws<Exception>(() => interpreterCommand.Execute());
    }

    [Fact]
    public void SuccessfulBuildAndPushInterpretatorCommand()
    {
        var moqMsg = new Mock<IMessage>();
        moqMsg.Setup(i => i.GameId).Returns(1);

        var moqCmd = new Mock<ICommand>();
        moqCmd.Setup(i => i.Execute());

        var queue = new Queue<ICommand>();

        var moqPushCmd = new Mock<ICommand>();
        moqPushCmd.Setup(i => i.Execute()).Callback(() => { queue.Enqueue(moqCmd.Object); });

        var moqPushStrategy = new Mock<IStrategy>();
        moqPushStrategy.Setup(i => i.Run(It.IsAny<int>(), It.IsAny<ICommand>())).Returns(moqPushCmd.Object);

        var moqReturnStrategy = new Mock<IStrategy>();
        moqReturnStrategy.Setup(i => i.Run(It.IsAny<IMessage>())).Returns(moqCmd.Object).Verifiable();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Create", (object[] args) => moqReturnStrategy.Object.Run(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Queue.Push", (object[] args) => moqPushStrategy.Object.Run(args)).Execute();

        var interpretatorCmd = new InterpretatorCommand(moqMsg.Object);
        interpretatorCmd.Execute();

        moqPushStrategy.VerifyAll();
        Assert.Equal(1, queue.Count);
    }

    [Fact]
    public void GetUObjectStrategyThrowsException()
    {
        int falseId = 2;

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.UObject", (object[] args) => this.uobjDict).Execute();
        var strategy = new GetUObjectStrategy();
        
        Assert.Throws<Exception>(() => strategy.Run(falseId));
    }

    [Fact]
    public void GetQueuetStrategyThrowsException()
    {
        int falseId = 2;
        this.queueDict.Add(1, new Queue<SpaceBattle.Lib.ICommand>());

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Queue.Dict", (object[] args) => this.queueDict).Execute();
        var strategy = new GetGameQueueStrategy();
        
        Assert.Throws<Exception>(() => strategy.Run(falseId));
    }

    [Fact]
    public void CreateCommandStrategyThrowsException()
    {
        var moqUobj = new Mock<IUObject>();

        var moqGetUobjStrategy = new Mock<IStrategy>();
        moqGetUobjStrategy.Setup(i => i.Run(It.IsAny<int>())).Returns(moqUobj.Object).Verifiable();

        var moqSetPropertCmd = new Mock<ICommand>();
        moqSetPropertCmd.Setup(i => i.Execute()).Callback(() => {});

        var moqSetPropertStrategy = new Mock<IStrategy>();
        moqSetPropertStrategy.Setup(i => i.Run(It.IsAny<object[]>())).Returns(moqSetPropertCmd.Object).Verifiable();

        var moqCmd = new Mock<ICommand>();

        var moqReturnStrategy = new Mock<IStrategy>();
        moqReturnStrategy.Setup(i => i.Run(It.IsAny<IUObject>())).Returns(moqCmd.Object).Verifiable();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Get.UObject", (object[] args) => moqGetUobjStrategy.Object.Run(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.SetProperties", (object[] args) => moqSetPropertStrategy.Object.Run(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Move", (object[] args) => moqReturnStrategy.Object.Run(args)).Execute();

        var moqMsg = new Mock<IMessage>();
        moqMsg.Setup(i => i.ItemId).Throws<Exception>();

        var strategy = new CreateCommandStrategy();

        Assert.Throws<Exception>(() => strategy.Run(moqMsg.Object));
    }

}

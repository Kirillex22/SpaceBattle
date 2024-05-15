using System;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;

public class DeleteUObjectCommandTests
{
    public DeleteUObjectCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();        
    }

    [Fact]
    public void DeleteUObjectCommandTestsSuccessful()
    {
        var uobjDict = new Dictionary<int, IUObject>();

        IoC.Resolve<Hwdtech.ICommand>("Game.UObject.Delete", (object[] args) => new DeleteUObjectStrategy().Run(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("Game.UObject", (object[] args) => uobjDict).Execute();

        var moqUobj = new Mock<IUObject>();

        Assert.Empty(uobjDict);

        uobjDict.Add(1, moqUobj.Object);
        Assert.Single(uobjDict);

        IoC.Resolve<Hwdtech.ICommand>("Game.UObject.Delete", 1).Execute();
        Assert.Empty(uobjDict);
    }
}


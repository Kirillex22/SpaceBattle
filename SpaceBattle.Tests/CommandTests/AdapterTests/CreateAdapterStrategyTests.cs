using System;
using System.Reflection;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;

public class CreateAdapterTests
{
    public CreateAdapterTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    public void CreateAdapterTestsSuccessfulCreate()
    {
        var moqUobj = new Mock<IUObject>();
        var targetType = typeof(Type);
        var assembly = Assembly.LoadFrom("SpaceBattle.Tests.dll");

        var assemblyDict = new Dictionary<KeyValuePair<Type, Type>, Assembly>();
        assemblyDict[new KeyValuePair<Type, Type>(moqUobj.Object.GetType(), targetType)] = assembly;

        var moqCmd = new Mock<Lib.ICommand>();
        moqCmd.Setup(i => i.Execute()).Verifiable();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Assembly.Map", (object[] args) => assemblyDict).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Compile", (object[] args) => moqCmd.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Find", (object[] args) => (object)0).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Create", (object[] args) => new CreateAdapterStrategy().Run(args)).Execute();

        Assert.NotEmpty(assemblyDict);
        moqCmd.Verify(i => i.Execute(), Times.Never());

        var adapter = IoC.Resolve<object>("Game.Adapter.Create", moqUobj.Object, targetType);
        Console.WriteLine(adapter);
        Assert.Equal(0, adapter);
        moqCmd.Verify(i => i.Execute(), Times.Never());
    }

    [Fact]
    public void CreateAdapterTestsSuccessfulCreateAndComile()
    {
        var moqUobj = new Mock<IUObject>();
        var targetType = typeof(Type);
        var assembly = Assembly.LoadFrom("SpaceBattle.Tests.dll");

        var assemblyDict = new Dictionary<KeyValuePair<Type, Type>, Assembly>();

        var moqCmd = new Mock<Lib.ICommand>();
        moqCmd.Setup(i => i.Execute()).Callback(() => assemblyDict[new KeyValuePair<Type, Type>(moqUobj.Object.GetType(), targetType)] = assembly).Verifiable();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Assembly.Map", (object[] args) => assemblyDict).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Compile", (object[] args) => moqCmd.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Find", (object[] args) => (object)1).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Create", (object[] args) => new CreateAdapterStrategy().Run(args)).Execute();

        Assert.Empty(assemblyDict);
        moqCmd.Verify(i => i.Execute(), Times.Never());

        var adapter = IoC.Resolve<ICommand>("Game.Adapter.Create", moqUobj.Object, targetType);

        Assert.Single(assemblyDict);
        Assert.Equal(1, adapter);
        moqCmd.Verify(i => i.Execute(), Times.Once());
    }
}


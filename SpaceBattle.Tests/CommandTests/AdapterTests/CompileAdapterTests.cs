using System;
using System.Reflection;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;

public class CompileAdapterTests
{
    public CompileAdapterTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    public void CompileAdapterTestsSuccessful()
    {
        var assembly = Assembly.LoadFrom("SpaceBattle.Tests.dll");
        var assemblyDict = new Dictionary<KeyValuePair<Type, Type>, Assembly>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Code", (object[] args) => ";").Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Compile", (object[] args) => assembly).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Assembly", (object[] args) => assemblyDict).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Compile", (object[] args) => new CompileAdapterStrategy().Run(args)).Execute();

        Assert.Empty(assemblyDict);

        IoC.Resolve<Lib.ICommand>("Game.Adapter.Compile", typeof(Type), typeof(Type)).Execute();

        Assert.Single(assemblyDict);
        Assert.Equal(new KeyValuePair<Type, Type>(typeof(Type), typeof(Type)), assemblyDict.First().Key);
        Assert.Equal(assembly, assemblyDict.First().Value);
    }
}


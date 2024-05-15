using System;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;

public class CreateNewScopeStrategyTests
{
    public CreateNewScopeStrategyTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }
/*
    [Fact]
    public void CreateNewScopeStrategySuccessful()
    {
        var scopeMap = new Dictionary<int, object>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register","Game.Scope.Create", (object[] args) => new CreateNewScopeStrategy().Run(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Scope.Map", (object[] args) => scopeMap).Execute();

        var mainScope = IoC.Resolve<object>("Scopes.Current");
        var currentScope = IoC.Resolve<object>("Game.Scope.Create", 123, 300d, mainScope);

        Assert.Throws<ArgumentException>(() => IoC.Resolve<object>("Game.Get.Quantum"));

        IoC.Resolve<ICommand>("Scopes.Current.Set", currentScope).Execute();
        try
        {
            var quantum = IoC.Resolve<object>("Game.Get.Time.Quantum");
            Assert.Equal(400d, quantum);
        }
        catch (Exception e) { Assert.Fail(e.Message); }

        IoC.Resolve<ICommand>("Scopes.Current.Set", mainScope).Execute();
        Assert.Throws<ArgumentException>(() => IoC.Resolve<object>("Game.Get.Quantum"));
    }*/
}


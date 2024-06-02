using System;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;

public class FindAdapterStrategyTests
{
    public FindAdapterStrategyTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var reference = new List<MetadataReference> {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(Assembly.LoadFrom("SpaceBattle.Tests.dll").Location)};

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Assembly.Name.Create", (object[] args) => Guid.NewGuid().ToString()).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Compile.References", (object[] args) => reference).Execute();
    }

    [Fact]
    public void FindAdapterStrategyTestsSuccessful()
    {
        Assembly assembly = null;
        var moqUobj = new Mock<IUObject>();
        var targetType = typeof(Type);
        var assemblyDict = new Dictionary<KeyValuePair<Type, Type>, Assembly>();
        var code = @"namespace SpaceBattle.Lib.Test;
                    public class ITestableAdapter
                    {
                        public ITestableAdapter(IUObject uObject) { }
                    }";

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Compile", (object[] args) => new CompileStrategy().Run(args)).Execute();
        assembly = IoC.Resolve<Assembly>("Compile", code);
        assemblyDict[new KeyValuePair<Type, Type>(moqUobj.Object.GetType(), targetType)] = assembly;

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Assembly.Map", (object[] args) => assemblyDict).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Name.Create", (object[] args) => "SpaceBattle.Lib.Test.ITestableAdapter").Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Find", (object[] args) => new FindAdapterStrategy().Run(args)).Execute();

        var adapter = IoC.Resolve<object>("Game.Adapter.Find", moqUobj.Object, targetType);

        Assert.NotNull(adapter);
        Assert.Equal("SpaceBattle.Lib.Test.ITestableAdapter", adapter.GetType().ToString());
    }
}


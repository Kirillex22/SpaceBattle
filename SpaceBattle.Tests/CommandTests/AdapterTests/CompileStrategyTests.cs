using System;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;

public class CompileStrategyTests
{
    public CompileStrategyTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    public void CompileStrategyTestsSuccessful()
    {
        Assembly assembly = null;
        var references = new List<MetadataReference> {MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(Assembly.LoadFrom("SpaceBattle.Tests.dll").Location)};

        var code = @"namespace SpaceBattle.Lib.Test;
                            public class TestFunc {
                            public TestFunc() {}}";

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Assembly.Name.Create", (object[] args) => Guid.NewGuid().ToString()).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Compile.References", (object[] args) => references).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Compile", (object[] args) => new CompileStrategy().Run(args)).Execute();

        assembly = IoC.Resolve<Assembly>("Compile", code);
        
        var func = Activator.CreateInstance(assembly.GetType("SpaceBattle.Lib.Test.TestFunc")!)!;

        Assert.Equal("SpaceBattle.Lib.Test.TestFunc", func.GetType().ToString());
    }
}


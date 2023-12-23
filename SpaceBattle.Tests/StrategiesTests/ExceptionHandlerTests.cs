using Hwdtech;
using Hwdtech.Ioc;
using SpaceBattle.Lib;
using System.Collections;
using Moq;

namespace SpaceBattle.Tests;

public class ExceptionHandlerTests
{
    public ExceptionHandlerTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
             IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccefulCall()
    {
        var empty = new EmptyCommand();
        var cmd = new Mock<SpaceBattle.Lib.ICommand>();
        var exc = new Exception();

        var resolveCmd1 = new Mock<SpaceBattle.Lib.ICommand>();
        resolveCmd1.Setup(c => c.Execute()).Verifiable();

        var resolveCmd2 = new Mock<SpaceBattle.Lib.ICommand>();
        resolveCmd2.Setup(c => c.Execute()).Verifiable();

        var concreteTree = new Hashtable(){
                {"SpaceBattle.Lib.EmptyCommand", new Hashtable(){{"System.Exception", resolveCmd1.Object}}},
                {"SpaceBattle.Lib.CheckCollisionCommand", new Hashtable(){{"System.Exception", resolveCmd2.Object}}},
        };

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Struct.ExceptionTree",
            (object[] args) => concreteTree
        ).Execute();

        var excHandlerFndr = new ExceptionHandlerFinder();

        excHandlerFndr.Call(empty, exc).Execute();
        //excHandlerFndr.Call(cmd.Object, exc).Execute();

        Mock.Verify(resolveCmd1, resolveCmd2);
    }
}


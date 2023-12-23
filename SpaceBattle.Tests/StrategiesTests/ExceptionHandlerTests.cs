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
        var emptyCmd = new EmptyCommand();
        var checkColCmd = new CheckCollisionCommand(
            new Mock<IUObject>().Object, new Mock<IUObject>().Object
        );

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

        var res1 = excHandlerFndr.Call(emptyCmd, exc);
        var res2 = excHandlerFndr.Call(checkColCmd, exc);

        res1.Execute();
        res2.Execute();

        Mock.Verify(resolveCmd1, resolveCmd2);
    }
}


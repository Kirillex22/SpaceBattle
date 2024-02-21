using Hwdtech;
using Hwdtech.Ioc;
using Moq;

using SpaceBattle.Lib;

public class StartServerCommandTests
{
    [Fact]
    public void SuccefulStartServer()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
             IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }
}
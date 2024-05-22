using Hwdtech;
using Hwdtech.Ioc;
using SpaceBattle.Lib;
using Scriban;
using System.Text.RegularExpressions;

namespace SpaceBattle.Tests;

public class SomeAdapterBuilderTests
{
    public SomeAdapterBuilderTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
             IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccesfulCodeGeneration()
    {
        new AdaptersGenerator().Call();

        var type = typeof(IMovable);

        var result = IoC.Resolve<string>("Game.Adapters.Generate", type);

        var expected = @"
        public class IMovableAdapter : IMovable
        {
            private IUObject _obj;
            public IMovableAdapter(IUObject obj) => _obj = obj;
            public Vector Position
            {
                get
                {
                    return IoC.Resolve<Vector>(""Game.IUObject.GetProperty"", _obj, ""Position"");
                }
                set
                {
                    IoC.Resolve<ICommand>(""Game.IUObject.SetProperty"", _obj, ""Position"", value).Execute();
                }
            }
            public Vector Velocity
            {
                get
                {
                    return IoC.Resolve<Vector>(""Game.IUObject.GetProperty"", _obj, ""Velocity"");
                }
            }
        }
        ";

        Assert.Equal(expected, result);
    }
}


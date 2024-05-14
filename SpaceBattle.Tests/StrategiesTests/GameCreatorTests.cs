using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using NuGet.Frameworks;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests;

public class GameCreatorTests
{
    private Dictionary<Guid, IUObject> _container = new();
    public GameCreatorTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
             IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.IUObject.SetProperty",
            (object[] args) =>
            {
                var obj = (IUObject)args[0];
                var key = (string)args[1];
                var value = args[2];

                var act = () =>
                {
                    obj.SetProperty(key, value);
                };

                return new ActionCommand(act);
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Adapters.IFuelable",
            (object[] args) =>
            {
                var obj = (IUObject)args[0];
                var fble = new Mock<IFuelable>();

                fble.SetupSet(f => f.Capacity = It.IsAny<int>()).Callback((int cpcty) =>
                {
                    IoC.Resolve<SpaceBattle.Lib.ICommand>(
                        "Game.IUObject.SetProperty",
                        obj,
                        "Capacity",
                        cpcty
                    ).Execute();
                });

                return fble.Object;
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Adapters.IMovable",
            (object[] args) =>
            {
                var obj = (IUObject)args[0];
                var mvble = new Mock<IMovable>();

                mvble.SetupSet(m => m.Position = It.IsAny<Vector>()).Callback((Vector pos) =>
                {
                    IoC.Resolve<SpaceBattle.Lib.ICommand>(
                        "Game.IUObject.SetProperty",
                        obj,
                        "Position",
                        pos
                    ).Execute();
                });

                return mvble.Object;
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Create.IUObject",
            (object[] args) =>
            {
                var dict = new Dictionary<string, object>();
                var uobj = new Mock<IUObject>();

                uobj.Setup(u => u.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Callback((string key, object value) =>
                {
                    dict.Add(key, value);
                });

                uobj.Setup(u => u.GetProperty(It.IsAny<string>())).Returns((string key) =>
                {
                    return dict[key];
                });

                return uobj.Object;
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.IUObject.Container.Push",
            (object[] args) =>
            {
                var id = (Guid)args[0];
                var obj = (IUObject)args[1];

                var act = () =>
                {
                    _container.Add(id, obj);
                };

                return new ActionCommand(act);
            }
        ).Execute();

        new EmptyUObjectCreator().Call();
        new EmptyUObjectsGenerator().Call();
        new InitialPositionSetter().Call();
        new InitialFuelSetter().Call();
        new GameCreator().Call();
    }

    [Fact]
    public void SuccesfulCreationOfInitGameStateWithSixObjects()
    {
        var count = 6;
        var startX = 0;
        var stepY = 1;
        var stepX = 10;
        var expectedFuel = 100;

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.SetInitialState", count, startX, stepY, stepX, expectedFuel).Execute();


        var posList = new List<int[]>()
        {
            new int[] {0, 0},
            new int[] {0, 1},
            new int[] {0, 2},
            new int[] {10, 0},
            new int[] {10, 1},
            new int[] {10, 2}
        };

        var isPositionOk = new List<bool>();
        var isFuelOk = new List<bool>();

        var idx = 0;

        _container.ToList().ForEach(x =>
        {
            var pos = (Vector)x.Value.GetProperty("Position");
            isPositionOk.Add((pos.Coords[0] == posList[idx][0]) && (pos.Coords[1] == posList[idx][1]));
            isFuelOk.Add((int)x.Value.GetProperty("Capacity") == expectedFuel);
            idx++;
        });

        Assert.True(_container.Count == 6 && isPositionOk.All(x => x == true) && isFuelOk.All(x => x == true));
    }
}


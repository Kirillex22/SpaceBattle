using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using NuGet.Frameworks;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests;

public class GameCreatorTests
{
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

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Initialize.Fuelable.GetFuel", (object[] args) =>
        {
            return "100";
        }).Execute();

        new GameObjectsContainer().Call();
        new ObjectCreator().Call();
    }

    [Fact]
    public void SuccesfulCreatingUObjects()
    {
        var uobj = new Mock<IUObject>().Object;

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Create.IUObject",
            (object[] args) => uobj
        ).Execute();

        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var id3 = Guid.NewGuid();

        new MacroCommand(new Lib.ICommand[]
        {
            new CreateObjectCommand(id1),
            new CreateObjectCommand(id2),
            new CreateObjectCommand(id3)
        }).Execute();

        var container = IoC.Resolve<Dictionary<Guid, IUObject>>("Game.IUObject.Container");

        Assert.True(
            container[id1] == uobj &&
            container[id2] == uobj &&
            container[id3] == uobj
        );
    }

    [Fact]
    public void SuccesfulSettingPositions()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Initialize.Movable.StartPosition", (object[] args) => new Vector(new int[] { 0, 0 })).Execute();

        var step = new Vector(new int[] { 0, 1 });

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Initialize.Movable.Position", (object[] args) =>
        {
            var previousPos = (Vector)args[0];
            return previousPos + step;
        }).Execute();

        new PositionGetter().Call();

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

        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var id3 = Guid.NewGuid();

        new MacroCommand(new Lib.ICommand[]{
            new CreateObjectCommand(id1),
            new PositionSetCommand(id1),

            new CreateObjectCommand(id2),
            new PositionSetCommand(id2),

            new CreateObjectCommand(id3),
            new PositionSetCommand(id3)
        }).Execute();

        var expected = new List<int[]>()
        {
            new int[] {0, 0},
            new int[] {0, 1},
            new int[] {0, 2}
        };

        var uobjects = IoC.Resolve<Dictionary<Guid, IUObject>>("Game.IUObject.Container").Values.ToList();

        var actual = new List<int[]>();

        uobjects.ForEach(obj =>
        {
            var vector = (Vector)obj.GetProperty("Position");
            actual.Add(vector.Coords);
        });

        var isPositionOk = new List<bool>();
        var idx = 0;
        actual.ForEach(pos =>
        {
            isPositionOk.Add(pos == expected[idx]);
            idx++;
        });

        Assert.All(isPositionOk, (val) => val.Equals(true));
    }

    [Fact]
    public void SuccesfulSettingFuel()
    {
        new FuelGetter().Call();

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

        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var id3 = Guid.NewGuid();

        new MacroCommand(new Lib.ICommand[]{
            new CreateObjectCommand(id1),
            new FuelSetCommand(id1),

            new CreateObjectCommand(id2),
            new FuelSetCommand(id2),

            new CreateObjectCommand(id3),
            new FuelSetCommand(id3)
        }).Execute();

        var expectedFuel = new List<int>() { 100, 100, 100 };

        var uobjects = IoC.Resolve<Dictionary<Guid, IUObject>>("Game.IUObject.Container").Values.ToList();

        var actualFuel = new List<int>();

        uobjects.ForEach(obj =>
        {
            var capacity = (int)obj.GetProperty("Capacity");
            actualFuel.Add(capacity);
        });

        Assert.True(actualFuel.SequenceEqual(expectedFuel));
    }

    [Fact]
    public void SuccesfulCreatingGameWithTwoPlayersWhoHaveThreeShips()
    {
        new LinearDisplacer().Call();
        new GameCreator().Call();
        new PositionGetter().Call();
        new FuelGetter().Call();

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

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Initialize.LinearPositionsWithFuel", 3).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Initialize.LinearPositionsWithFuel", 3).Execute();

        var expectedPositions = new List<int[]>()
        {
            new int[] {0, 0},
            new int[] {0, 1},
            new int[] {0, 2},
            new int[] {1, 0},
            new int[] {1, 1},
            new int[] {1, 2}
        };

        var expectedFuel = new List<int>() { 100, 100, 100, 100, 100, 100 };


        var uobjects = IoC.Resolve<Dictionary<Guid, IUObject>>("Game.IUObject.Container").Values.ToList();

        var actualPositions = new List<int[]>();
        var actualFuel = new List<int>();

        uobjects.ForEach(obj =>
        {
            var vector = (Vector)obj.GetProperty("Position");
            actualPositions.Add(vector.Coords);
        });

        uobjects.ForEach(obj =>
        {
            var capacity = (int)obj.GetProperty("Capacity");
            actualFuel.Add(capacity);
        });

        var isPositionOk = new List<bool>();
        var idx = 0;
        actualPositions.ForEach(pos =>
        {
            isPositionOk.Add(pos == expectedPositions[idx]);
            idx++;
        });

        Assert.All(isPositionOk, (val) => val.Equals(true));
        Assert.True(actualFuel.SequenceEqual(expectedFuel));
    }

}


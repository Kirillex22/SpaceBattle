namespace SpaceBattle.Tests;

using SpaceBattle.Lib;
using System.Collections;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;

public class FireCommandTest
{
    public FireCommandTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

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

        new AmmoCreator().Call();
        new ObjectCreator().Call();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.StartMove", (object[] args) => new Mock<SpaceBattle.Lib.ICommand>().Object).Execute();

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
            "Game.IUObject.GetProperty",
            (object[] args) =>
            {
                var obj = (IUObject)args[0];
                var key = (string)args[1];

                return obj.GetProperty(key);
            }
        ).Execute();
    }

    [Fact]
    public void SuccesfulExecutingOfTheFireCommand()
    {
        new GameObjectsContainer().Call();

        var shipId = Guid.NewGuid();

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Create.EmptyObject", shipId).Execute();
        var ship = IoC.Resolve<Dictionary<Guid, IUObject>>("Game.IUObject.Container")[shipId];

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.IUObject.SetProperty", ship, "AmmoType", "105mm").Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.IUObject.SetProperty", ship, "Position", new Vector(new int[] { 0, 0 })).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.IUObject.SetProperty", ship, "Velocity", new Vector(new int[] { 1, 1 })).Execute();

        new FireCommand(new FireableAdapter(ship)).Execute();

        var ids = IoC.Resolve<Dictionary<Guid, IUObject>>("Game.IUObject.Container").Keys.ToList();

        var query = from id in ids
                    where id != shipId
                    select id;

        var ammoId = query.Last();

        var ammo = IoC.Resolve<Dictionary<Guid, IUObject>>("Game.IUObject.Container")[ammoId];

        var pos = IoC.Resolve<Vector>("Game.IUObject.GetProperty", ammo, "Position");
        var vel = IoC.Resolve<Vector>("Game.IUObject.GetProperty", ammo, "Velocity");
        var ammoType = IoC.Resolve<string>("Game.IUObject.GetProperty", ammo, "AmmoType");

        Assert.True("105mm" == ammoType);
        Assert.True(pos.Coords[0] == 0 && pos.Coords[1] == 0);
        Assert.True(vel.Coords[1] == 1 && vel.Coords[1] == 1);
    }

    [Fact]
    public void SuccesfulSettingPptsOfFbleAdapter()
    {
        new GameObjectsContainer().Call();

        var shipId = Guid.NewGuid();

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Create.EmptyObject", shipId).Execute();
        var ship = IoC.Resolve<Dictionary<Guid, IUObject>>("Game.IUObject.Container")[shipId];

        var fbleAdapter = new FireableAdapter(ship);

        fbleAdapter.AmmoType = "105mm";
        fbleAdapter.AmmoPosition = new Vector(new int[] { 0, 0 });
    }
}


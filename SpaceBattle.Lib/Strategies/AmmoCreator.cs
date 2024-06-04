using Hwdtech;

namespace SpaceBattle.Lib;

public class AmmoCreator
{
    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.Create.Ammo",
        (object[] args) =>
        {
            var type = (string)args[0];
            var sPos = (Vector)args[1];
            var sVel = (Vector)args[2];
            var id = Guid.NewGuid();

            IoC.Resolve<ICommand>("Game.Create.EmptyObject", id).Execute();
            var obj = IoC.Resolve<Dictionary<Guid, IUObject>>("Game.IUObject.Container")[id];

            IoC.Resolve<ICommand>("Game.IUObject.SetProperty", obj, "AmmoType", type).Execute();
            IoC.Resolve<ICommand>("Game.IUObject.SetProperty", obj, "Position", sPos).Execute();
            IoC.Resolve<ICommand>("Game.IUObject.SetProperty", obj, "Velocity", sVel).Execute();

            return obj;
        }).Execute();
    }
}


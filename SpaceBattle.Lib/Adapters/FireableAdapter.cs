using Hwdtech;

namespace SpaceBattle.Lib;

public class FireableAdapter : IFireable
{
    private IUObject _obj;

    public FireableAdapter(IUObject obj) => _obj = obj;

    public string AmmoType
    {
        get => IoC.Resolve<string>("Game.IUObject.GetProperty", _obj, "AmmoType");

        set => IoC.Resolve<ICommand>("Game.IUObject.SetProperty", _obj, "AmmoType", value).Execute();
    }
    public Vector AmmoPosition
    {
        get => IoC.Resolve<Vector>("Game.IUObject.GetProperty", _obj, "Position");

        set => IoC.Resolve<ICommand>("Game.IUObject.SetProperty", _obj, "Position", value).Execute();
    }
    public Vector AmmoVelocity
    {
        get => IoC.Resolve<Vector>("Game.IUObject.GetProperty", _obj, "Velocity");
    }

    public IUObject Fire() => IoC.Resolve<IUObject>("Game.Create.Ammo", AmmoType, AmmoPosition, AmmoVelocity);
}


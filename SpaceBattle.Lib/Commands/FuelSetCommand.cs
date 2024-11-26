using Hwdtech;

namespace SpaceBattle.Lib;

public class FuelSetCommand : ICommand
{
    private Guid _id;
    public FuelSetCommand(Guid id) => _id = id;

    public void Execute()
    {
        var obj = IoC.Resolve<IUObject>("Game.IUObject.Container.Get", _id);
        var flble = IoC.Resolve<IFuelable>("Game.Adapters.IFuelable", obj);
        var objectFuel = (int)IoC.Resolve<object>("Game.Generators.Fuelable.Capacity");

        flble.Capacity = objectFuel;
    }
}


using Hwdtech;

namespace SpaceBattle.Lib;
public class FuelGetter
{
    private FuelGenerator _fuelGenerator;

    public FuelGetter() => _fuelGenerator = new();

    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.Generators.Fuelable.Capacity",
        (object[] args) =>
        {
            return (object)_fuelGenerator.GetCapacity();
        }
        ).Execute();
    }
}


using System.Collections;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;
public class FuelGenerator
{
    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.Generators.Fuelable.Capacity",
        (object[] args) =>
        {
            var objs = (List<IUObject>)args[0];
            var cpcty = (int)args[1];

            var objsWithFuel = GenerateFuel(objs, cpcty);
            return objsWithFuel.ToList();
        }
        ).Execute();
    }

    private IEnumerable<IUObject> GenerateFuel(List<IUObject> objs, int capacity)
    {
        foreach (var obj in objs)
        {
            var fble = IoC.Resolve<IFuelable>("Game.Adapters.IFuelable", obj);
            fble.Capacity = capacity;
            yield return obj;
        }
    }
}


using System.Collections;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;

public class InitialFuelSetter
{
    private FuelGenerator _fuelGen = new();

    public InitialFuelSetter() => _fuelGen.Call();

    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.Initialize.StartFuelCapacity",
        (object[] args) =>
        {
            var objs = (IEnumerable<IUObject>)args[0];
            var capacity = (int)args[1];
            var updObjs = IoC.Resolve<IEnumerable<IUObject>>("Game.Generators.Fuelable.Capacity", objs, capacity);
            return updObjs;
        }
        ).Execute();
    }
}
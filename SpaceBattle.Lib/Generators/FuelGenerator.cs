using System.Collections;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;
public class FuelGenerator
{
    private int _currentFuel;

    public int GetCapacity() => GenerateFuel().Last();

    private IEnumerable<int> GenerateFuel()
    {
        _currentFuel = int.Parse(IoC.Resolve<string>("Game.Initialize.Fuelable.GetFuel"));
        yield return _currentFuel;
    }
}


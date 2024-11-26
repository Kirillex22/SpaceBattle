using System.Collections;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;
public class PositionGenerator
{
    private Vector _currentPosition;

    public PositionGenerator() => _currentPosition = IoC.Resolve<Vector>("Game.Initialize.Movable.StartPosition");

    public void Reset() => _currentPosition = IoC.Resolve<Vector>("Game.Initialize.Movable.StartPosition");

    public Vector GetNewPosition() => GeneratePosition().Last();
    private IEnumerable<Vector> GeneratePosition()
    {
        yield return _currentPosition;
        _currentPosition = IoC.Resolve<Vector>("Game.Initialize.Movable.Position", _currentPosition);
    }
}


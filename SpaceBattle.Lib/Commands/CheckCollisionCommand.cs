using Hwdtech;
using Hwdtech.Ioc;
using System.Collections;
using System.Linq;

namespace SpaceBattle.Lib;

public class CheckCollisionCommand
{
    private int[]? _state;
    private Func<IDictionary, int, IDictionary>? _searcher;
    private IUObject _obj1;
    private IUObject _obj2;

    public CheckCollisionCommand(IUObject obj1, IUObject obj2)
    { 
        _obj1 = obj1;
        _obj2 = obj2;     
    }

    public void Execute()
    {
        var positions = new List<int[]>{
            IoC.Resolve<int[]>("Game.UObject.GetProperty", _obj1, "Position"),
            IoC.Resolve<int[]>("Game.UObject.GetProperty", _obj2, "Position")
        };
        
        var velocities = new List<int[]>{
            IoC.Resolve<int[]>("Game.UObject.GetProperty", _obj1, "Velocity"),      
            IoC.Resolve<int[]>("Game.UObject.GetProperty", _obj2, "Velocity")
        }; 

        _state = positions[1].ToList().Select((value, index) => value - positions[0][index]).Concat(
                velocities[1].ToList().Select((value, index) => value - velocities[0][index])
        ).ToArray();

        var collTree = IoC.Resolve<IDictionary>("Game.Struct.CollisionTree");

        _searcher = (IDictionary currentTree, int parameter) => {
            return currentTree.Contains(parameter) ? (IDictionary)currentTree[parameter] : throw new ArgumentException();
        };

        _state.ToList().ForEach(parameter => collTree = _searcher(collTree, parameter));


        IoC.Resolve<ICommand>("Game.Event.Collision", _obj1, _obj2).Execute();   
    }
}


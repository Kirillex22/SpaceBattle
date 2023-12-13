using Hwdtech;
using Hwdtech.Ioc;
using System.Collections;
using Linq;

namespace SpaceBattle.Lib;

class CheckCollisionCommand
{
    private int[] _state;
    private Searcher _searcher;

    public CheckCollisionCommand(IMovable obj1, IMovable obj2)
    {
        var rx = obj1.Position.Coords[0] - obj2.Position.Coords[0];
        var ry = obj1.Position.Coords[1] - obj2.Position.Coords[1];
        var rdx = obj1.Velocity.Coords[0] - obj2.Velocity.Coords[0];
        var rdy = obj1.Velocity.Coords[1] - obj2.Velocity.Coords[1];

        _state = new int[] {rx, ry, rdx, rdy};     

        _searcher = (int param, ref Hashtable currLvl) => 
        {         
            if(!currLvl.ContainsKey(param))
            {
                throw new Exception("collision interception");
            }

            else
            {
                currLvl = currLvl[param];
            }
        }
    }

    public void Execute()
    {
        var collisionTree = IoC.Resolve<ITree>("Game.Struct.CollisionTree");

        _state.Select(value => _searcher(value, collisionTree));
    }

    delegate void Searcher(int p, Hashtable l);

}
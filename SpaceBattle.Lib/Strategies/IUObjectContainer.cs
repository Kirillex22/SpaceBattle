using Hwdtech;
namespace SpaceBattle.Lib;

public class IUObjectContainer
{
    private Dictionary<Guid, IUObject> _container;

    public IUObjectContainer() => _container = new();

    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.IUObject.Container.Push", (object[] args) => 
        {
            var id = (Guid)args[0];
            var obj = (IUObject)args[1];
            _container.Add(id, obj);
        });

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.IUObject.Container.Get", (object[] args) => 
        {
            var id = (Guid)args[0];
            return _container[id];
        });
    }
}
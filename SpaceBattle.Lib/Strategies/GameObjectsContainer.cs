using Hwdtech;
namespace SpaceBattle.Lib;

public class GameObjectsContainer
{
    private Dictionary<Guid, IUObject> _container;

    public GameObjectsContainer() => _container = new();

    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.IUObject.Container.Push", (object[] args) =>
        {
            var id = (Guid)args[0];
            var obj = (IUObject)args[1];
            var act = () =>
            {
                _container.Add(id, obj);
            };

            return new ActionCommand(act);
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.IUObject.Container.Get", (object[] args) =>
        {
            var id = (Guid)args[0];
            return _container[id];
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.IUObject.Container", (object[] args) =>
        {
            return _container;
        }).Execute();
    }
}


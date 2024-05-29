using Hwdtech;

namespace SpaceBattle.Lib;

public class ShootCommand : ICommand
{
    private IShootable _shoot;

    public ShootCommand(IShootable shoot)
    {
        _shoot = shoot;
    }

    public void Execute()
    {
        var bullet = IoC.Resolve<object>("Game.Create.Bullet", _shoot.BulletType);
        var cmd = IoC.Resolve<ICommand>("Game.Create.Bullet.Move", bullet, _shoot.Position, _shoot.Velocity);
        var gameId = IoC.Resolve<int>("Game.Get.GameID");

        IoC.Resolve<ICommand>("Game.Queue.Push", gameId, cmd).Execute();
    }
}


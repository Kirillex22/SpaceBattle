namespace SpaceBattle.Lib;

public interface IShootable
{
    Vector Position {get; set;}
    Vector Velocity {get; set;}
    string BulletType {get; set;}
}


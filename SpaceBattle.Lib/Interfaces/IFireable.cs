namespace SpaceBattle.Lib;

public interface IFireable
{
    public string AmmoType { set; get; }
    public Vector AmmoPosition { get; set; }
    public Vector AmmoVelocity { get; }

    public IUObject Fire();
}


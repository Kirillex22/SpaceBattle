namespace SpaceBattle.Lib;

public interface IUObject
{
    public object GetProperty(string name);
    public void SetProperty(string key, object value);
}


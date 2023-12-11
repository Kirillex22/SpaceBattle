namespace SpaceBattle.Lib;

public interface IUobject
{
    public object GetProperty(string name);
    public void SetProperty(string key, object value);
}


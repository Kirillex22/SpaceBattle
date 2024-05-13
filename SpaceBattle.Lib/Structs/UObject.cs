namespace SpaceBattle.Lib;

public class UObject : IUObject
{
    private Dictionary<string, object> _properties;

    public UObject() => _properties = new();

    public void SetProperty(string key, object value) => _properties[key] = value;

    public object GetProperty(string key) => _properties[key];
}
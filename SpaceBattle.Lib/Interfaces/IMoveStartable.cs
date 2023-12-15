namespace SpaceBattle.Lib;

public interface IMoveStartable
{
    public IUObject Target {get;}  
    public string Command {get;}
    public IDictionary<string, object> InitialValues {get;}
}


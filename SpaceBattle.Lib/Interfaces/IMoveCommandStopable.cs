namespace SpaceBattle.Lib;

public interface IMoveCommandStopable
{
    public IUObject Uobject { get; }
    public string NameCommand { get; }
    public IEnumerable<string> Properties { get; }
}


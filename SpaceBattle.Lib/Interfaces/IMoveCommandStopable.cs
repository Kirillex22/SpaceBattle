namespace SpaceBattle.Lib;

public interface IMoveCommandStopable
{
    public IUobject Uobject { get; }
    public string NameCommand { get; }
    public IEnumerable<string> Properties{ get; }   
}


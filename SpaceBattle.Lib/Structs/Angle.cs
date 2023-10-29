namespace SpaceBattle.Lib;

public class Angle
{
    int x {get; set;}
    int y {get; set;}

    public Angle(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static Angle operator +(Angle u1, Angle u2)
    {
        return new Angle((u1.x + u2.x)%u1.y, u1.y);
    }
}
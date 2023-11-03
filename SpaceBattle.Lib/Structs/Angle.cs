namespace SpaceBattle.Lib;

public class Angle
{
    public int sector;
    public int separation;

    public Angle(int sector, int separation = 8)
    {
        this.sector = sector;
        this.separation = separation;
    }

    public static Angle operator +(Angle u1, Angle u2)
    {
        return new Angle((u1.sector + u2.sector)%u1.separation, u1.separation);
    }
}
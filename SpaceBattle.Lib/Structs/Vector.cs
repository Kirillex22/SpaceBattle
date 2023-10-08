namespace SpaceBattleLib;

 public class Vector
    {
        public double x {get; set;}

        public double y {get; set;}

        public Vector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.x + v2.x, v1.y + v2.y);                 
        }

        public static bool Equals(Vector v1, Vector v2)
        {
            return ((v1.x == v2.x)&&(v1.y == v2.y));
        }
    }
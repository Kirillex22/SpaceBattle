namespace SpaceBattleLib;

 public class Vector
    {
        double x {get; set;}

        double y {get; set;}

        public Vector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            try
            {
                return new Vector(v1.x + v2.x, v1.y + v2.y);
            }

            catch
            {
                throw new ArgumentException("wrong position or velocity");
            }
            
        }

        public bool IsNan()
        {
            if (x == double.NaN || y == double.NaN)
                return true;
            else
                return false;
        }
    }
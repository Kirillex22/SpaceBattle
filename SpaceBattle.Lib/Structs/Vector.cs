namespace SpaceBattleLib;

 public class Vector
    {
        public double x;

        public double y;

        public Vector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            if (Double.IsNaN(v1.x)||Double.IsNaN(v1.y)||Double.IsNaN(v2.x)||Double.IsNaN(v2.y))
                throw new ArgumentException("One of the vectors contains some NaN value");

            return new Vector(v1.x + v2.x, v1.y + v2.y);                 
        }

        public List<double> ToList()
        {
            return new List<double>() {x,y};
        }
    }
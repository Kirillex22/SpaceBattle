using System.ComponentModel;

namespace SpaceBattleLib;

 public class Vector
    {
        public double[] coords;

        public Vector(double[] coords)
        {
            this.coords = coords;
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            if (v1.coords.Contains(double.NaN)||v2.coords.Contains(double.NaN))
            {
                throw new ArgumentException();
            }

            var coords1 = v1.coords;
            var coords2 = v2.coords;
            var result = new double[v1.coords.Length];

            for(int i = 0; i < v1.coords.Length; i++)
                result[i] = coords1[i] + coords2[i];

            return new Vector(result);
                             
        }
    }